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
	[Command(PackageIds.RecipeExtensions_AspNetMvc_5x_AddController_MenuItemId)]
	public class RecipeExtensions_AspNetMvc_5x_AddController_Command : BaseCommand<RecipeExtensions_AspNetMvc_5x_AddController_Command>
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

				showCommand = RecipeExtensionsHelper.IsControllersFolder(project, solutionItem);
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			try
			{
				var inputDialog = new InputDialog("New Mvc Controller");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var controllerKey = inputDialog.Value.Replace(" ", string.Empty);

					if (!string.IsNullOrWhiteSpace(controllerKey))
					{
						var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

						await outputWindowPane.ActivateAsync();

						await outputWindowPane.ClearAsync();

						await outputWindowPane.WriteLineAsync("New Mvc Controller");

						var solutionItem = await VS.Solutions.GetActiveItemAsync();
						var solution = await VS.Solutions.GetCurrentSolutionAsync();
						var project = await VS.Solutions.GetActiveProjectAsync();
						
						await project?.SaveAsync();

						var @namespace = RecipeExtensionsHelper.GetRootNamespace(project);
						var areaName = RecipeExtensionsHelper.GetAreaName(solutionItem);

						var routePath = System.Text.RegularExpressions.Regex.Replace(controllerKey, @"(?<begin>(\w*?))(?<end>[A-Z]+)", string.Format(@"${{begin}}{0}${{end}}", "-")).Substring(1).Trim().ToLower();

						var routeUrl = ((string.IsNullOrWhiteSpace(areaName) && string.Equals(controllerKey, "Public", StringComparison.InvariantCultureIgnoreCase)) ? string.Empty : string.Format(" + \"{0}/\"", routePath));

						var contentReplacements = new Dictionary<string, string>
						{
							{ "${Namespace}", @namespace },
							{ "${Namespace.Area}", string.Format("{0}{1}", @namespace, (string.IsNullOrWhiteSpace(areaName) ? string.Empty : string.Format(".Areas.{0}", areaName))) },
							{ "${AreaName}", areaName },
							{ "${Areas.AreaName}", (string.IsNullOrWhiteSpace(areaName) ? string.Empty : string.Format("Areas.{0}.", areaName)) },
							{ "${ControllerKey}", controllerKey },
							{ "${BaseUrl}", routePath },
							{ "${RouteUrl}", routeUrl }
						};

						var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
						var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

						var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);

						var areasDirectory = RecipeExtensionsHelper.GetAreasDirectory(project);
						var areaDirectory = RecipeExtensionsHelper.GetAreaDirectory(solutionItem);

						var controllersDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_5x_Helper.ControllersFolderName);
						var controllerDirectory = System.IO.Path.Combine(controllersDirectory, controllerKey);

						if (!System.IO.Directory.Exists(controllersDirectory) || !System.IO.Directory.Exists(controllerDirectory))
						{
							System.IO.Directory.CreateDirectory(controllersDirectory);
							System.IO.Directory.CreateDirectory(controllerDirectory);
							var modelBindersDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_5x_Helper.ModelBindersFolderName);
							System.IO.Directory.CreateDirectory(modelBindersDirectory);
							var modelBindersControllerDirectory = System.IO.Path.Combine(modelBindersDirectory, controllerKey);
							System.IO.Directory.CreateDirectory(modelBindersControllerDirectory);
							var modelsDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_5x_Helper.ModelsFolderName);
							System.IO.Directory.CreateDirectory(modelsDirectory);
							var modelsControllerDirectory = System.IO.Path.Combine(modelsDirectory, controllerKey);
							System.IO.Directory.CreateDirectory(modelsControllerDirectory);
							var routesDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_5x_Helper.RoutesFolderName);
							System.IO.Directory.CreateDirectory(routesDirectory);

							var recipes = new ExtensionsHelper.RecipeItem[]
							{
								new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(controllersDirectory, "__Controller.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.Controller_ControllerRoot_Template)), controllersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(controllerDirectory, string.Format("__{0}Controller.cs", controllerKey)), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.Controller_Controller_Template)), controllerDirectory, controllersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(modelBindersDirectory, "_ModelBinders.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.Controller_ModelBindersRoot_Template)), modelBindersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory), false,
									(projectItems, fullName, content, replacementValues) =>
									{
										RecipeExtensionsHelper.ReplaceFileContent(fullName, new Dictionary<string, string>
										{
											{ "//${ModelBinders}", string.Format("{0}.RegisterModelBinders(binders);\r\n			//${{ModelBinders}}", controllerKey) }
										});
									}),
								new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(modelBindersControllerDirectory, "_ModelBinders.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.Controller_ModelBinders_Template)), modelBindersControllerDirectory, modelBindersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(modelsDirectory, "_BaseModel.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.Controller_BaseModelRoot_Template)), modelsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(modelsControllerDirectory, "_BaseModel.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.Controller_BaseModel_Template)), modelsControllerDirectory, modelsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(routesDirectory, "__Routes.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.Controller_RoutesRoot_Template)), routesDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory), false,
									(projectItems, fullName, content, replacementValues) =>
									{
										RecipeExtensionsHelper.ReplaceFileContent(fullName, new Dictionary<string, string>
										{
											{ "//${Routes}", string.Format("{0}.RegisterRoutes(routes);\r\n			//${{Routes}}", controllerKey) }
										});
									}),
								new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(routesDirectory, string.Format("{0}.cs", controllerKey)), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.Controller_Routes_Template)), routesDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
							};

							await RecipeExtensionsHelper.AddFromRecipesAsync(project, recipes, contentReplacements);

							Package.GetDTE2().GetSelectedProject().RunT4LocalContents();

							await outputWindowPane.WriteLineAsync("Done\n");
							await outputWindowPane.ActivateAsync();
						}
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
