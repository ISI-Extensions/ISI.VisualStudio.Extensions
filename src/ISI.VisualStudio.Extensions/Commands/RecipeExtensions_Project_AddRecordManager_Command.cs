#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using ISI.Extensions.Repository;
using static ISI.VisualStudio.Extensions.Extensions.SolutionExtensions;
using ISI.VisualStudio.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.RecipeExtensions_Project_AddRecordManager_MenuItemId)]
	public class RecipeExtensions_Project_AddRecordManager_Command : BaseCommand<RecipeExtensions_Project_AddRecordManager_Command>
	{
		private static RecipeExtensions_Project_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_Project_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_Project_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			if (RecipeExtensionsHelper.IsProjectRoot(solutionItem))
			{
				var project = solutionItem as Project;

				showCommand = project.UsesISIExtensionsRepositorySqlServer() || project.UsesISILibrariesRepositorySqlServer();
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			try
			{
				var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

				await outputWindowPane.ActivateAsync();

				await outputWindowPane.ClearAsync();
				await outputWindowPane.WriteLineAsync("Add Record Manager Class");

				var solution = await VS.Solutions.GetCurrentSolutionAsync();
				var project = await VS.Solutions.GetActiveProjectAsync();
				var solutionItem = await VS.Solutions.GetActiveItemAsync();

				await project?.SaveAsync();

				var projectDescriptions = solution.GetProjectDescriptions();

				var contractProjectDescription = (SolutionExtensions.ProjectDescription)null;

				var rootNamespace = project.GetRootNamespace();
				var rootNamespaceQueue = new List<string>(rootNamespace.Split(new[] { '.' }));

				while (rootNamespaceQueue.Any() && (contractProjectDescription == null))
				{
					rootNamespaceQueue.RemoveAt(rootNamespaceQueue.Count - 1);
					var contractProjectRootNamespace = string.Join(".", rootNamespaceQueue);

					contractProjectDescription = projectDescriptions.FirstOrDefault(projectDescription => string.Equals(projectDescription.RootNamespace, contractProjectRootNamespace, StringComparison.InvariantCultureIgnoreCase));
				}

				contractProjectDescription ??= projectDescriptions.FirstOrDefault(projectDescription => string.Equals(projectDescription.RootNamespace, rootNamespace, StringComparison.InvariantCultureIgnoreCase));

				var addRecordManagerDialog = new AddRecordManagerDialog(projectDescriptions, contractProjectDescription);

				var addRecordManagerDialogResult = await addRecordManagerDialog.ShowDialogAsync();

				if (addRecordManagerDialogResult.GetValueOrDefault())
				{
					var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
					var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

					var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);
					var @namespace = project.GetRootNamespace();

					var codeExtensionProvider = project.GetCodeExtensionProvider();

					var recipeName = string.Empty;
					switch (codeExtensionProvider)
					{
						case ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Extensions.CodeExtensionProvider isiExtensionsCodeExtensionProvider:
							recipeName = nameof(RecipeOptions.Project_ISI_Extensions_SqlServer_RecordManager_Template);
							break;

						case ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Libraries.CodeExtensionProvider isiLibrariesCodeExtensionProvider:
							recipeName = nameof(RecipeOptions.Project_ISI_Libraries_SqlServer_RecordManager_Template);
							break;
					}

					if (!string.IsNullOrWhiteSpace(recipeName))
					{
						contractProjectDescription = projectDescriptions.FirstOrDefault(projectDescription => string.Equals(projectDescription.Description, addRecordManagerDialog.ContractProjectDescription, StringComparison.InvariantCultureIgnoreCase));

						await contractProjectDescription.Project?.SaveAsync();

						var contractProjectDirectory = System.IO.Path.GetDirectoryName(contractProjectDescription.Project.FullPath);

						var directory = solutionItem.FullPath;
						while (!System.IO.Directory.Exists(directory))
						{
							directory = System.IO.Path.GetDirectoryName(directory);
						}

						directory = System.IO.Path.Combine(directory, string.Format("{0}Manager", addRecordManagerDialog.NewRecordManagerName));

						System.IO.Directory.CreateDirectory(directory);

						var recordManager = string.Format("RecordManager<{0}>", addRecordManagerDialog.NewRecordManagerName);
						if (!string.IsNullOrWhiteSpace(addRecordManagerDialog.PrimaryKeyType))
						{
							if (addRecordManagerDialog.HasArchive)
							{
								recordManager = string.Format("RecordManagerPrimaryKey<{0}, {1}>", addRecordManagerDialog.NewRecordManagerName, addRecordManagerDialog.PrimaryKeyType);
							}
							else
							{
								recordManager = string.Format("RecordManagerPrimaryKeyWithArchive<{0}, {1}>", addRecordManagerDialog.NewRecordManagerName, addRecordManagerDialog.PrimaryKeyType);
							}
						}

						var sortedUsingStatements = RecipeExtensionsHelper.GetSortedUsings(codeExtensionProvider, null, null);

						var contentReplacements = new Dictionary<string, string>
						{
							{ "${Usings}", string.Join(Environment.NewLine, sortedUsingStatements.GetFormatted()) },
							{ "${Namespace}", @namespace },
							{ "${RecordName}", addRecordManagerDialog.NewRecordManagerName },
							{ "${RecordManager}", recordManager },
						};
						
						await RecipeExtensionsHelper.AddFromRecipesAsync(project, new [] { new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, string.Format("__{0}Manager.cs", addRecordManagerDialog.NewRecordManagerName)), RecipeExtensionsHelper.GetContent(recipeName, projectDirectory, solutionRecipesDirectory, solutionDirectory), true) }, contentReplacements);

						contentReplacements["${Namespace}"] = contractProjectDescription.RootNamespace;
						await RecipeExtensionsHelper.AddFromRecipesAsync(contractProjectDescription.Project, new [] { new Extensions_Helper.RecipeItem(System.IO.Path.Combine(contractProjectDirectory, string.Format("I{0}Manager.cs", addRecordManagerDialog.NewRecordManagerName)), RecipeExtensionsHelper.GetContent( nameof(RecipeOptions.Project_RecordManagerInterface_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true) }, contentReplacements);

					}
				}

				await outputWindowPane.WriteLineAsync("Done\n");
				await outputWindowPane.ActivateAsync();
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
