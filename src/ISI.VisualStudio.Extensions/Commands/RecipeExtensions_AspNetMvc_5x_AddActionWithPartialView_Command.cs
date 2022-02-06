﻿using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.RecipeExtensions_AspNetMvc_5x_AddActionWithPartialView_MenuItemId)]
	public class RecipeExtensions_AspNetMvc_5x_AddActionWithPartialView_Command : BaseCommand<RecipeExtensions_AspNetMvc_5x_AddActionWithPartialView_Command>
	{
		private static RecipeExtensions_AspNetMvc_5x_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_AspNetMvc_5x_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_AspNetMvc_5x_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var project = VS.Solutions.GetActiveProjectAsync().GetAwaiter().GetResult();
			if (RecipeExtensionsHelper.IsAspNetMvc_5x_Project(project))
			{
				var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

				showCommand = RecipeExtensionsHelper.IsControllerFolder(solutionItem);
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			try
			{
				var inputDialog = new InputDialog("New Action With Partial View");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var controllerActionKey = inputDialog.Value.Replace(" ", string.Empty);

					var isAsync = controllerActionKey.EndsWith("Async", StringComparison.InvariantCulture);
					if (isAsync)
					{
						controllerActionKey = controllerActionKey.Substring(0, controllerActionKey.Length - "Async".Length);
					}

					if (!string.IsNullOrWhiteSpace(controllerActionKey))
					{
						var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

						await outputWindowPane.ActivateAsync();

						await outputWindowPane.ClearAsync();

						await outputWindowPane.WriteLineAsync("New Action With View");

						var solutionItem = await VS.Solutions.GetActiveItemAsync();
						var solution = await VS.Solutions.GetCurrentSolutionAsync();
						var project = await VS.Solutions.GetActiveProjectAsync();

						await project?.SaveAsync();

						var @namespace = RecipeExtensionsHelper.GetRootNamespace(project);
						var areaName = RecipeExtensionsHelper.GetAreaName(solutionItem);
						var controllerKey = RecipeExtensionsHelper.GetControllerName(solutionItem);

						var viewTitle = System.Text.RegularExpressions.Regex.Replace((string.Equals(controllerActionKey, "Index", StringComparison.InvariantCultureIgnoreCase) ? controllerKey : controllerActionKey), @"(?<begin>(\w*?))(?<end>[A-Z]+)", string.Format(@"${{begin}}{0}${{end}}", " ")).Trim();

						var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
						var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

						var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);

						var areasDirectory = RecipeExtensionsHelper.GetAreasDirectory(project);
						var areaDirectory = RecipeExtensionsHelper.GetAreaDirectory(solutionItem);

						var controllersDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_5x_Helper.ControllersFolderName);
						var controllerDirectory = System.IO.Path.Combine(controllersDirectory, controllerKey);

						var usings = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ISI.Libraries.Web.Mvc.Extensions;
using ISI.Libraries.Extensions;
";

						{
							var fileName = System.IO.Directory.GetFiles(controllerDirectory).OrderBy(controllerFileName => controllerFileName, StringComparer.InvariantCultureIgnoreCase).FirstOrDefault();

							if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
							{
								usings = string.Join("\r\n", System.IO.File.ReadAllLines(fileName).Where(line => line.StartsWith("using ", StringComparison.InvariantCulture)));
							}
						}

						var contentReplacements = new Dictionary<string, string>
						{
							{ "${Usings}", usings },
							{ "${Namespace}", @namespace },
							{ "${Namespace.Area}", string.Format("{0}{1}", @namespace, (string.IsNullOrWhiteSpace(areaName) ? string.Empty : string.Format(".Areas.{0}", areaName))) },
							{ "${AreaName}", areaName },
							{ "${Areas.AreaName}", (string.IsNullOrWhiteSpace(areaName) ? string.Empty : string.Format("Areas.{0}.", areaName)) },
							{ "${ControllerKey}", controllerKey },
							{ "${ControllerActionKey}", controllerActionKey },
							{ "${ControllerActionKey.pascalCase}", string.Format("{0}{1}", controllerActionKey.Substring(0, 1).ToLower(), controllerActionKey.Substring(1)) },
							{ "${ViewTitle}", viewTitle },
						};

						var modelsDirectory = System.IO.Path.Combine(areaDirectory, "Models");
						System.IO.Directory.CreateDirectory(modelsDirectory);
						var modelsControllerDirectory = System.IO.Path.Combine(modelsDirectory, controllerKey);
						System.IO.Directory.CreateDirectory(modelsControllerDirectory);
						var viewsDirectory = System.IO.Path.Combine(areaDirectory, "Views");
						System.IO.Directory.CreateDirectory(viewsDirectory);
						var viewsSharedDirectory = System.IO.Path.Combine(viewsDirectory, "_Shared");
						System.IO.Directory.CreateDirectory(viewsSharedDirectory);
						var viewsControllerDirectory = System.IO.Path.Combine(viewsDirectory, controllerKey);
						System.IO.Directory.CreateDirectory(viewsControllerDirectory);
						var partialViewsControllerDirectory = System.IO.Path.Combine(viewsControllerDirectory, "Partials");
						System.IO.Directory.CreateDirectory(partialViewsControllerDirectory);
						var routesDirectory = System.IO.Path.Combine(areaDirectory, "Routes");

						var recipes = new[]
						{
							new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(controllerDirectory, string.Format("{0}{1}.cs", controllerActionKey, (isAsync ? "Async" : string.Empty))), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", (isAsync ? nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.AsyncActionWithPartialView_Action_Template) : nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.ActionWithPartialView_Action_Template))), controllerDirectory, controllersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory), true),

							new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(modelsControllerDirectory, string.Format("{0}Model.cs", controllerActionKey)), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.ActionWithPartialView_Model_Template)), modelsControllerDirectory, modelsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),

							new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(viewsDirectory, "web.config"), RecipeExtensionsHelper.FilterWebConfig(project, RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.ViewsWebConfig_Template)), viewsControllerDirectory, viewsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory))),
							new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(partialViewsControllerDirectory, string.Format("{0}.cshtml", controllerActionKey)), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.ActionWithPartialView_PartialView_Template)), partialViewsControllerDirectory, viewsControllerDirectory, viewsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),

							new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(routesDirectory, string.Format("{0}.cs", controllerKey)), null, false,
								(projectItems, fullName, content, replacementValues) =>
								{
									var routePath = System.Text.RegularExpressions.Regex.Replace(controllerActionKey, @"(?<begin>(\w*?))(?<end>[A-Z]+)", string.Format(@"${{begin}}{0}${{end}}", "-")).Substring(1).Trim().ToLower();

									var routeUrl = (string.Equals(controllerActionKey, "Index", StringComparison.InvariantCultureIgnoreCase) ? string.Empty : string.Format(" + \"{0}\"", routePath));

									RecipeExtensionsHelper.ReplaceFileContent(fullName, new Dictionary<string, string>
									{
										{ "//${RouteNames}", string.Format("[RouteName] public static readonly string {0};\r\n\t\t\t\t//${{RouteNames}}", controllerActionKey) },
										{ "//${Routes}", string.Format("//${{Routes}}\r\n\t\t\t\troutes.MapRoute<Controllers.{0}Controller>(RouteNames.{1}, UrlRoot{2}, controller => controller.{1}());", controllerKey, controllerActionKey, routeUrl) }
									});
								}),
						};

						await RecipeExtensionsHelper.AddFromRecipesAsync(project, recipes, contentReplacements);

						Package.GetDTE2().GetSelectedProject().RunT4LocalContents();

						await outputWindowPane.WriteLineAsync("Done\n");
						await outputWindowPane.ActivateAsync();
					}
				}
			}
			catch (Exception exception)
			{
				var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

				await outputWindowPane.WriteLineAsync(exception.ErrorMessageFormatted());

				throw;
			}
		}
	}
}
