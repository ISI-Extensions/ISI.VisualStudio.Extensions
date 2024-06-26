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
	[Command(PackageIds.RecipeExtensions_AspNetMvc_6x_AddArea_MenuItemId)]
	public class RecipeExtensions_AspNetMvc_6x_AddArea_Command : BaseCommand<RecipeExtensions_AspNetMvc_6x_AddArea_Command>
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
						var @namespace = project.GetRootNamespace();

						var areasDirectory = RecipeExtensionsHelper.GetAreasDirectory(project);
						System.IO.Directory.CreateDirectory(areasDirectory);

						var areaDirectory = System.IO.Path.Combine(areasDirectory, areaKey);

						if (!System.IO.Directory.Exists(areaDirectory))
						{
							System.IO.Directory.CreateDirectory(areaDirectory);

							var routeUrl = System.Text.RegularExpressions.Regex.Replace(areaKey, @"(?<begin>(\w*?))(?<end>[A-Z]+)", string.Format(@"${{begin}}{0}${{end}}", "-")).Substring(1).Trim().ToLower();

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
								{ "${Namespace.Area}", string.Format("{0}{1}", @namespace, (string.IsNullOrWhiteSpace(areaKey) ? string.Empty : string.Format(".Areas.{0}", areaKey))) },
								{ "${AreaName}", areaKey },
								{ "${Areas.AreaName}", string.Format("Areas.{0}.", areaKey) },
								{ "${BaseUrl}", routeUrl }
							};

							var controllersDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_6x_Helper.ControllersFolderName);
							System.IO.Directory.CreateDirectory(controllersDirectory);
							var modelsDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_6x_Helper.ModelsFolderName);
							System.IO.Directory.CreateDirectory(modelsDirectory);
							var routesDirectory = System.IO.Path.Combine(areaDirectory, RecipeExtensions_AspNetMvc_6x_Helper.RoutesFolderName);
							System.IO.Directory.CreateDirectory(routesDirectory);

							var recipes = new[]
							{
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(controllersDirectory, "__Controller.cs"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_Area_Controller_Template), controllersDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(modelsDirectory, "_BaseModel.cs"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_Area_BaseModel_Template), modelsDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(routesDirectory, "__Routes.cs"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.AspNetMvc_6x_Area_Routes_Template), routesDirectory, areaDirectory, areasDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory), false, 
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
