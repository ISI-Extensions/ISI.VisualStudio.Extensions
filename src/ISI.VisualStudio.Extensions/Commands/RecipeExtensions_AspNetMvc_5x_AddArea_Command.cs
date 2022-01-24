using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;

namespace ISI.VisualStudio.Extensions
{
	[Community.VisualStudio.Toolkit.Command(PackageIds.RecipeExtensions_AspNetMvc_5x_AddArea_MenuItemId)]
	internal sealed class RecipeExtensions_AspNetMvc_5x_AddArea_Command : BaseCommand<RecipeExtensions_AspNetMvc_5x_AddArea_Command>
	{
		protected RecipeExtensionsHelper RecipeExtensionsHelper { get; } = new RecipeExtensionsHelper();

		protected override void BeforeQueryStatus(System.EventArgs eventArgs)
		{
			var showCommand = false;

			var project = VS.Solutions.GetActiveProjectAsync().GetAwaiter().GetResult();
			if (RecipeExtensionsHelper.IsAspNetMvc_5x_Project(project))
			{
				var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

				showCommand = RecipeExtensionsHelper.IsProjectRoot(solutionItem) || RecipeExtensionsHelper.IsAreasFolder(project, solutionItem);
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			try
			{
				var inputDialog = new InputDialog("New Mvc Area");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var areaKey = inputDialog.Value;

					if (!string.IsNullOrWhiteSpace(areaKey))
					{
						var solition = await VS.Solutions.GetCurrentSolutionAsync();
						var project = await VS.Solutions.GetActiveProjectAsync();


						var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

						await outputWindowPane.ActivateAsync();

						await outputWindowPane.ClearAsync();

						await outputWindowPane.WriteLineAsync("New Mvc Area");

						areaKey = areaKey.Replace(" ", string.Empty);

						var solutionDirectory = System.IO.Path.GetDirectoryName(solition.FullPath);
						var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

						var projectDirectory = project.FullPath.Substring(0, project.FullPath.LastIndexOf('\\', project.FullPath.Length - 2));
						var @namespace = await project.GetAttributeAsync("DefaultNamespace");

						var areasDirectory = string.Format("{0}\\Areas", projectDirectory);
						System.IO.Directory.CreateDirectory(areasDirectory);

						var areaDirectory = System.IO.Path.Combine(areasDirectory, areaKey);

						if (!System.IO.Directory.Exists(areaDirectory))
						{
							//_recipeExtensions.Package.GetDte2().Documents.SaveAll();

							System.IO.Directory.CreateDirectory(areaDirectory);

							var routeUrl = System.Text.RegularExpressions.Regex.Replace(areaKey, @"(?<begin>(\w*?))(?<end>[A-Z]+)", string.Format(@"${{begin}}{0}${{end}}", "-")).Substring(1).Trim().ToLower();

							var contentReplacements = new Dictionary<string, string>
							{
								{ "${Namespace}", @namespace },
								{ "${Namespace.Area}", string.Format("{0}{1}", @namespace, (string.IsNullOrWhiteSpace(areaKey) ? string.Empty : string.Format(".Areas.{0}", areaKey))) },
								{ "${AreaName}", areaKey },
								{ "${Areas.AreaName}", string.Format("Areas.{0}.", areaKey) },
								{ "${BaseUrl}", routeUrl }
							};

							var controllersDirectory = System.IO.Path.Combine(areaDirectory, "Controllers");
							System.IO.Directory.CreateDirectory(controllersDirectory);
							var modelBindersDirectory = System.IO.Path.Combine(areaDirectory, "ModelBinders");
							System.IO.Directory.CreateDirectory(modelBindersDirectory);
							var modelsDirectory = System.IO.Path.Combine(areaDirectory, "Models");
							System.IO.Directory.CreateDirectory(modelsDirectory);
							var routesDirectory = System.IO.Path.Combine(areaDirectory, "Routes");
							System.IO.Directory.CreateDirectory(routesDirectory);

							var recipes = new[]
							{
								new RecipeExtensionsHelper.RecipeItem(System.IO.Path.Combine(controllersDirectory, "__Controller.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.AddArea_Controller)), controllersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new RecipeExtensionsHelper.RecipeItem(System.IO.Path.Combine(modelBindersDirectory, "_ModelBinders.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.AddArea_ModelBinders)), modelBindersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new RecipeExtensionsHelper.RecipeItem(System.IO.Path.Combine(modelsDirectory, "_BaseModel.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.AddArea_BaseModel)), modelsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new RecipeExtensionsHelper.RecipeItem(System.IO.Path.Combine(routesDirectory, "__Routes.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.AddArea_Routes)), routesDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new RecipeExtensionsHelper.RecipeItem(System.IO.Path.Combine(areaDirectory, "AreaRegistration.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(ISI.VisualStudio.Extensions.RecipeExtensions.AspNetMvc_5x_Recipes.AddArea_AreaRegistration)), areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
							};

							await RecipeExtensionsHelper.AddFromRecipesAsync(project, recipes, contentReplacements);

							//T4Manager.T4Extensions.Instance.CheckProject(_recipeExtensions.GetSelectedProject());

							await outputWindowPane.WriteLineAsync("Done\n");
							await outputWindowPane.ActivateAsync();
						}
					}
				}
			}
			catch (Exception exception)
			{
				//_recipeExtensions.AddToExceptionLog(exception);

				throw;
			}
		}
	}
}
