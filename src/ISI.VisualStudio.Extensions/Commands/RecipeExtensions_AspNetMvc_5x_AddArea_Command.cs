﻿using Community.VisualStudio.Toolkit;
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
	[Command(PackageIds.RecipeExtensions_AspNetMvc_5x_AddArea_MenuItemId)]
	public class RecipeExtensions_AspNetMvc_5x_AddArea_Command : BaseCommand<RecipeExtensions_AspNetMvc_5x_AddArea_Command>
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

				showCommand = RecipeExtensionsHelper.IsProjectRoot(solutionItem) || RecipeExtensionsHelper.IsAreasFolder(project, solutionItem);
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			try
			{
				var inputDialog = new InputDialog("New Mvc Area");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var areaKey = inputDialog.Value.Replace(" ", string.Empty);

					if (!string.IsNullOrWhiteSpace(areaKey))
					{
						var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

						await outputWindowPane.ActivateAsync();

						await outputWindowPane.ClearAsync();

						await outputWindowPane.WriteLineAsync("New Mvc Area");

						var solution = await VS.Solutions.GetCurrentSolutionAsync();
						var project = await VS.Solutions.GetActiveProjectAsync();
						
						await project?.SaveAsync();

						var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
						var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

						var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);
						var @namespace = RecipeExtensionsHelper.GetRootNamespace(project);

						var areasDirectory = RecipeExtensionsHelper.GetAreasDirectory(project);
						System.IO.Directory.CreateDirectory(areasDirectory);

						var areaDirectory = System.IO.Path.Combine(areasDirectory, areaKey);

						if (!System.IO.Directory.Exists(areaDirectory))
						{
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

							var controllersDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_5x_Helper.ControllersFolderName);
							System.IO.Directory.CreateDirectory(controllersDirectory);
							var modelBindersDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_5x_Helper.ModelBindersFolderName);
							System.IO.Directory.CreateDirectory(modelBindersDirectory);
							var modelsDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_5x_Helper.ModelsFolderName);
							System.IO.Directory.CreateDirectory(modelsDirectory);
							var routesDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_5x_Helper.RoutesFolderName);
							System.IO.Directory.CreateDirectory(routesDirectory);

							var recipes = new[]
							{
								new RecipeExtensions_Helper.RecipeItem(System.IO.Path.Combine(controllersDirectory, "__Controller.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(RecipeExtensions.AspNetMvc_5x_Recipes.AddArea_Controller)), controllersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new RecipeExtensions_Helper.RecipeItem(System.IO.Path.Combine(modelBindersDirectory, "_ModelBinders.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(RecipeExtensions.AspNetMvc_5x_Recipes.AddArea_ModelBinders)), modelBindersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new RecipeExtensions_Helper.RecipeItem(System.IO.Path.Combine(modelsDirectory, "_BaseModel.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(RecipeExtensions.AspNetMvc_5x_Recipes.AddArea_BaseModel)), modelsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new RecipeExtensions_Helper.RecipeItem(System.IO.Path.Combine(routesDirectory, "__Routes.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(RecipeExtensions.AspNetMvc_5x_Recipes.AddArea_Routes)), routesDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new RecipeExtensions_Helper.RecipeItem(System.IO.Path.Combine(areaDirectory, "AreaRegistration.cs"), RecipeExtensionsHelper.GetContent(string.Format("AspNetMvc_5x_{0}", nameof(RecipeExtensions.AspNetMvc_5x_Recipes.AddArea_AreaRegistration)), areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
							};

							await RecipeExtensionsHelper.AddFromRecipesAsync(project, recipes, contentReplacements);

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
