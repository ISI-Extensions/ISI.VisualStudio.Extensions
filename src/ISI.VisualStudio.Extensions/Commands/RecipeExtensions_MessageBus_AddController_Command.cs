#region Copyright & License
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
	[Command(PackageIds.RecipeExtensions_MessageBus_AddController_MenuItemId)]
	public class RecipeExtensions_MessageBus_AddController_Command : BaseCommand<RecipeExtensions_MessageBus_AddController_Command>
	{
		private static RecipeExtensions_MessageBus_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_MessageBus_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_MessageBus_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var project = VS.Solutions.GetActiveProjectAsync().GetAwaiter().GetResult();
			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			showCommand = RecipeExtensionsHelper.IsControllersFolder(project, solutionItem);

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			try
			{
				var inputDialog = new AddMessageBusControllerDialog();

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.NewControllerName))
				{
					var controllerKey = inputDialog.NewControllerName.Replace(" ", string.Empty);

					if (!string.IsNullOrWhiteSpace(controllerKey))
					{
						var addIsAuthorized = inputDialog.AddIsAuthorized;

						var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

						await outputWindowPane.ActivateAsync();

						await outputWindowPane.ClearAsync();

						await outputWindowPane.WriteLineAsync("New Controller");

						var solutionItem = await VS.Solutions.GetActiveItemAsync();
						var solution = await VS.Solutions.GetCurrentSolutionAsync();
						var project = await VS.Solutions.GetActiveProjectAsync();

						await project?.SaveAsync();

						var @namespace = $"{project.GetRootNamespace()}.MessageBus";

						var codeExtensionProvider = project.GetCodeExtensionProvider();

						var usings = new List<string>();
						usings.Add("Microsoft.Extensions.DependencyInjection");
						usings.Add("ISI.Extensions.Extensions");

						var sortedUsingStatements = RecipeExtensionsHelper.GetSortedUsings(codeExtensionProvider, usings);

						var contentReplacements = new Dictionary<string, string>
						{
							{ "${Usings}", sortedUsingStatements.GetFormatted() },
							{ "${Namespace}", @namespace },
							{ "${ControllerKey}", controllerKey },
						};

						var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
						var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

						var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);

						var controllersDirectory = System.IO.Path.Combine(projectDirectory, RecipeExtensions_MessageBus_Helper.MessageBusFolderName, RecipeExtensions_MessageBus_Helper.ControllersFolderName);
						var controllerDirectory = System.IO.Path.Combine(controllersDirectory, controllerKey);

						var subscriptionsDirectory = System.IO.Path.Combine(projectDirectory, RecipeExtensions_MessageBus_Helper.MessageBusFolderName, RecipeExtensions_MessageBus_Helper.SubscriptionsFolderName);

						if (!System.IO.Directory.Exists(controllersDirectory) || !System.IO.Directory.Exists(controllerDirectory))
						{
							System.IO.Directory.CreateDirectory(controllersDirectory);
							System.IO.Directory.CreateDirectory(controllerDirectory);

							System.IO.Directory.CreateDirectory(subscriptionsDirectory);

							var recipes = new Extensions_Helper.RecipeItem[]
							{
								new(System.IO.Path.Combine(controllerDirectory, string.Format("__{0}Controller.cs", controllerKey)), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.MessageBus_Controller_Controller_Template), controllerDirectory, controllersDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory)),
								new(System.IO.Path.Combine(subscriptionsDirectory, "__Subscriptions.cs"), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.MessageBus_Controller_SubscriptionsRoot_Template), subscriptionsDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory), false),
								new(System.IO.Path.Combine(subscriptionsDirectory, string.Format("{0}.cs", controllerKey)), RecipeExtensionsHelper.GetContent((addIsAuthorized ? nameof(RecipeOptions.MessageBus_Controller_SubscriptionsWithAuthentication_Template) : nameof(RecipeOptions.MessageBus_Controller_Subscriptions_Template)), subscriptionsDirectory, projectDirectory, solutionRecipesDirectory, solutionDirectory), false),
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
