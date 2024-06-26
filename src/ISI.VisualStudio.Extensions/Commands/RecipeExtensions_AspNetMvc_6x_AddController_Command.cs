﻿#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
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
	[Command(PackageIds.RecipeExtensions_AspNetMvc_6x_AddController_MenuItemId)]
	public class RecipeExtensions_AspNetMvc_6x_AddController_Command : BaseCommand<RecipeExtensions_AspNetMvc_6x_AddController_Command>
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

				showCommand = RecipeExtensionsHelper.IsControllersFolder(project, solutionItem);
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			try
			{
				var inputDialog = new InputDialog("New Controller");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var controllerKey = inputDialog.Value.Replace(" ", string.Empty);

					if (!string.IsNullOrWhiteSpace(controllerKey))
					{
						var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

						await outputWindowPane.ActivateAsync();

						await outputWindowPane.ClearAsync();

						await outputWindowPane.WriteLineAsync("New Controller");

						var solutionItem = await VS.Solutions.GetActiveItemAsync();
						var solution = await VS.Solutions.GetCurrentSolutionAsync();
						var project = await VS.Solutions.GetActiveProjectAsync();

						await project?.SaveAsync();

						var @namespace = project.GetRootNamespace();
						var areaName = RecipeExtensionsHelper.GetAreaName(solutionItem);

						var routePath = System.Text.RegularExpressions.Regex.Replace(controllerKey, @"(?<begin>(\w*?))(?<end>[A-Z]+)", string.Format(@"${{begin}}{0}${{end}}", "-")).Substring(1).Trim().ToLower();

						var routeUrl = ((string.IsNullOrWhiteSpace(areaName) && string.Equals(controllerKey, "Public", StringComparison.InvariantCultureIgnoreCase)) ? string.Empty : string.Format(" + \"{0}/\"", routePath));

						var codeExtensionProvider = project.GetCodeExtensionProvider();

						var usings = new List<string>();
						usings.Add("Microsoft.AspNetCore.Mvc");
						usings.Add("Microsoft.Extensions.DependencyInjection");
						usings.Add("ISI.Extensions.Extensions");

						var sortedUsingStatements = RecipeExtensionsHelper.GetSortedUsings(codeExtensionProvider, usings);

						var contentReplacements = new Dictionary<string, string>
						{
							{ "${Usings}", sortedUsingStatements.GetFormatted() },
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

						var controllersDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_6x_Helper.ControllersFolderName);
						var controllerDirectory = System.IO.Path.Combine(controllersDirectory, controllerKey);

						if (!System.IO.Directory.Exists(controllersDirectory) || !System.IO.Directory.Exists(controllerDirectory))
						{
							System.IO.Directory.CreateDirectory(controllersDirectory);
							System.IO.Directory.CreateDirectory(controllerDirectory);
							var modelsDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_6x_Helper.ModelsFolderName);
							System.IO.Directory.CreateDirectory(modelsDirectory);
							var modelsControllerDirectory = System.IO.Path.Combine(modelsDirectory, controllerKey);
							System.IO.Directory.CreateDirectory(modelsControllerDirectory);
							var routesDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_6x_Helper.RoutesFolderName);
							System.IO.Directory.CreateDirectory(routesDirectory);

							var recipes = new Extensions_Helper.RecipeItem[]
							{
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(controllersDirectory, "__Controller.cs"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_Controller_ControllerRoot_Template), controllersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(controllerDirectory, string.Format("__{0}Controller.cs", controllerKey)), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_Controller_Controller_Template), controllerDirectory, controllersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(modelsDirectory, "_BaseModel.cs"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_Controller_BaseModelRoot_Template), modelsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(modelsControllerDirectory, "_BaseModel.cs"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_Controller_BaseModel_Template), modelsControllerDirectory, modelsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(routesDirectory, "__Routes.cs"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_Controller_RoutesRoot_Template), routesDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory), false,
									(projectItems, fullName, content, replacementValues) =>
									{
										var usings = new ProjectExtensions_Helper.Usings(sortedUsingStatements);
										usings.Add("ISI.Extensions.AspNetCore");
										usings.Add("ISI.Extensions.AspNetCore.Extensions");

										replacementValues.Remove("${Usings}");
										replacementValues.Add("${Usings}", usings.GetFormatted());
									},
									(projectItems, fullName, content, replacementValues) =>
									{
										RecipeExtensionsHelper.ReplaceFileContent(fullName, new Dictionary<string, string>
										{
											{ "//${Routes}", string.Format("{0}.RegisterRoutes(routes);\r\n			//${{Routes}}", controllerKey) }
										});
									}),
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(routesDirectory, string.Format("{0}.cs", controllerKey)), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_Controller_Routes_Template), routesDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory), false,
									(projectItems, fullName, content, replacementValues) =>
									{
										var usings = new ProjectExtensions_Helper.Usings(sortedUsingStatements);
										usings.Add("ISI.Extensions.AspNetCore");
										usings.Add("ISI.Extensions.AspNetCore.Extensions");

										replacementValues.Remove("${Usings}");
										replacementValues.Add("${Usings}", usings.GetFormatted());
									}),
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
