using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.RecipeExtensions_AspNetMvc_6x_AddActionWithView_MenuItemId)]
	public class RecipeExtensions_AspNetMvc_6x_AddActionWithView_Command : BaseCommand<RecipeExtensions_AspNetMvc_6x_AddActionWithView_Command>
	{
		private static RecipeExtensions_AspNetMvc_6x_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_AspNetMvc_6x_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_AspNetMvc_6x_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var project = VS.Solutions.GetActiveProjectAsync().GetAwaiter().GetResult();
			if (RecipeExtensionsHelper.IsAspNetMvc_6x_Project(project))
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
				var inputDialog = new InputDialog("New Action With View");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var controllerActionKey = inputDialog.Value.Replace(" ", string.Empty);

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

						var @namespace = project.GetRootNamespace();
						var areaName = RecipeExtensionsHelper.GetAreaName(solutionItem);
						var areaNamespace = string.Format("{0}{1}", @namespace, (string.IsNullOrWhiteSpace(areaName) ? string.Empty : string.Format(".Areas.{0}", areaName)));
						var controllerKey = RecipeExtensionsHelper.GetControllerName(solutionItem);

						var viewTitle = System.Text.RegularExpressions.Regex.Replace((string.Equals(controllerActionKey, "Index", StringComparison.InvariantCultureIgnoreCase) ? controllerKey : controllerActionKey), @"(?<begin>(\w*?))(?<end>[A-Z]+)", string.Format(@"${{begin}}{0}${{end}}", " ")).Trim();

						var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
						var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

						var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);

						var areasDirectory = RecipeExtensionsHelper.GetAreasDirectory(project);
						var areaDirectory = RecipeExtensionsHelper.GetAreaDirectory(solutionItem);

						var controllersDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_6x_Helper.ControllersFolderName);
						var controllerDirectory = System.IO.Path.Combine(controllersDirectory, controllerKey);

						var routePath = System.Text.RegularExpressions.Regex.Replace(controllerActionKey, @"(?<begin>(\w*?))(?<end>[A-Z]+)", string.Format(@"${{begin}}{0}${{end}}", "-")).Substring(1).Trim().ToLower();

						var routeUrl = (string.Equals(controllerActionKey, "Index", StringComparison.InvariantCultureIgnoreCase) ? string.Empty : routePath);

						var codeExtensionProvider = project.GetCodeExtensionProvider();

						var usings = new List<string>();
						usings.Add("Microsoft.AspNetCore.Mvc");
						usings.Add("Microsoft.Extensions.DependencyInjection");
						usings.Add("ISI.Extensions.Extensions");

						var controllerFileName = System.IO.Directory.GetFiles(controllerDirectory).OrderBy(controllerFileName => controllerFileName, StringComparer.InvariantCultureIgnoreCase).FirstOrDefault();
						var sortedUsingStatements = RecipeExtensionsHelper.GetSortedUsings(codeExtensionProvider, usings, new []{controllerFileName});

						var contentReplacements = new Dictionary<string, string>
						{
							{ "${Usings}", sortedUsingStatements.GetFormatted() },
							{ "${Namespace}", @namespace },
							{ "${Namespace.Area}", areaNamespace },
							{ "${AreaName}", areaName },
							{ "${Areas.AreaName}", (string.IsNullOrWhiteSpace(areaName) ? string.Empty : string.Format("Areas.{0}.", areaName)) },
							{ "${ControllerKey}", controllerKey },
							{ "${ControllerActionKey}", controllerActionKey },
							{ "${ControllerActionKey.pascalCase}", string.Format("{0}{1}", controllerActionKey.Substring(0, 1).ToLower(), controllerActionKey.Substring(1)) },
							{ "${RouteUrl}", routeUrl },
							{ "${ViewTitle}", viewTitle },
						};

						var modelsDirectory = System.IO.Path.Combine(areaDirectory, "Models");
						System.IO.Directory.CreateDirectory(modelsDirectory);
						var modelsControllerDirectory = System.IO.Path.Combine(modelsDirectory, controllerKey);
						System.IO.Directory.CreateDirectory(modelsControllerDirectory);
						var javaScriptsDirectory = System.IO.Path.Combine(areaDirectory, "JavaScripts");
						System.IO.Directory.CreateDirectory(javaScriptsDirectory);
						var javaScriptsSharedDirectory = System.IO.Path.Combine(javaScriptsDirectory, "_Shared");
						System.IO.Directory.CreateDirectory(javaScriptsSharedDirectory);
						var javaScriptsControllerDirectory = System.IO.Path.Combine(javaScriptsDirectory, controllerKey);
						System.IO.Directory.CreateDirectory(javaScriptsControllerDirectory);
						var styleSheetsDirectory = System.IO.Path.Combine(areaDirectory, "StyleSheets");
						System.IO.Directory.CreateDirectory(styleSheetsDirectory);
						var styleSheetsSharedDirectory = System.IO.Path.Combine(styleSheetsDirectory, "_Shared");
						System.IO.Directory.CreateDirectory(styleSheetsSharedDirectory);
						var styleSheetsControllerDirectory = System.IO.Path.Combine(styleSheetsDirectory, controllerKey);
						System.IO.Directory.CreateDirectory(styleSheetsControllerDirectory);
						var viewsDirectory = System.IO.Path.Combine(areaDirectory, "Views");
						System.IO.Directory.CreateDirectory(viewsDirectory);
						var viewsSharedDirectory = System.IO.Path.Combine(viewsDirectory, "_Shared");
						System.IO.Directory.CreateDirectory(viewsSharedDirectory);
						var viewsControllerDirectory = System.IO.Path.Combine(viewsDirectory, controllerKey);
						System.IO.Directory.CreateDirectory(viewsControllerDirectory);
						var routesDirectory = System.IO.Path.Combine(areaDirectory, "Routes");

						var recipes = new[]
						{
							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(controllerDirectory, string.Format("{0}.cs", controllerActionKey)), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_ActionWithView_Action_Template), controllerDirectory, controllersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory), true),

							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(modelsControllerDirectory, string.Format("{0}Model.cs", controllerActionKey)), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_ActionWithView_Model_Template), modelsControllerDirectory, modelsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),

							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(javaScriptsSharedDirectory, "_Layout.js"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_JavaScriptsSharedLayout_Template), javaScriptsSharedDirectory, javaScriptsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(javaScriptsControllerDirectory, "_Layout.js"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_JavaScriptsControllerLayout_Template), javaScriptsControllerDirectory, javaScriptsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(javaScriptsControllerDirectory, string.Format("{0}.js", controllerActionKey)), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_ActionWithView_JavaScript_Template), javaScriptsControllerDirectory, javaScriptsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),

							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(styleSheetsSharedDirectory, "_Layout.css"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_StyleSheetsSharedLayout_Template), styleSheetsSharedDirectory, styleSheetsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(styleSheetsControllerDirectory, "_Layout.css"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_StyleSheetsControllerLayout_Template), styleSheetsControllerDirectory, styleSheetsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(styleSheetsControllerDirectory, string.Format("{0}.css", controllerActionKey)), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_ActionWithView_StyleSheet_Template), styleSheetsControllerDirectory, styleSheetsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),

							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(viewsSharedDirectory, "_Layout.cshtml"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_ViewsSharedLayout_Template), viewsSharedDirectory, viewsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(viewsControllerDirectory, "_Layout.cshtml"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_ViewsControllerLayout_Template), viewsControllerDirectory, viewsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(viewsControllerDirectory, string.Format("{0}.cshtml", controllerActionKey)), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_ActionWithView_View_Template), viewsControllerDirectory, viewsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),

							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(routesDirectory, string.Format("{0}.cs", controllerKey)), null, false,
								(projectItems, fullName, content, replacementValues) =>
								{
									RecipeExtensionsHelper.ReplaceFileContent(fullName, new Dictionary<string, string>
									{
										{ "//${RouteNames}", string.Format("[RouteName] public const string {0} = \"{0}-{1}\";\r\n\t\t\t\t//${{RouteNames}}", controllerActionKey, Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens)) },
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
