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
using ISI.VisualStudio.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_ProjectPartialClass_Helper
	{
		public async Task AddPartialClassMethodAsync()
		{
			try
			{
				var solution = await VS.Solutions.GetCurrentSolutionAsync();
				var project = await VS.Solutions.GetActiveProjectAsync();
				var solutionItem = await VS.Solutions.GetActiveItemAsync();

				await project?.SaveAsync();

				var projectDescriptions = solution.GetProjectDescriptions();
				var partialClassDirectory = solutionItem.FullPath;

				var partialClassName = partialClassDirectory.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last();

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

				var inputDialog = new AddPartialClassMethodDialog(partialClassName, projectDescriptions, contractProjectDescription);

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.NewPartialClassMethodName))
				{
					var methodName = inputDialog.NewPartialClassMethodName;

					if (inputDialog.IsAsync)
					{
						methodName = methodName.TrimEnd("Async");
					}

					if (!string.IsNullOrWhiteSpace(methodName))
					{
						var outputWindowPane = await GetOutputWindowPaneAsync();

						await outputWindowPane.ActivateAsync();

						await outputWindowPane.ClearAsync();

						await outputWindowPane.WriteLineAsync("New Partial Class Method");

						await project?.SaveAsync();

						var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
						var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

						var projectDirectory = GetProjectDirectory(project);

						var @namespace = GetNamespace(project, solutionItem, partialClassName);

						if (System.IO.Directory.Exists(partialClassDirectory))
						{
							var codeExtensionProvider = project.GetCodeExtensionProvider();

							var sortedUsingStatements = GetSortedUsings(codeExtensionProvider, null, new []{System.IO.Directory.GetFiles(partialClassDirectory).OrderBy(partialClassFileName => partialClassFileName, StringComparer.InvariantCultureIgnoreCase).FirstOrDefault()});

							var contentReplacements = new Dictionary<string, string>
							{
								{"${Usings}", string.Join(Environment.NewLine, sortedUsingStatements.GetFormatted())},
								{"${Namespace}", @namespace},
								{"${ClassName}", partialClassName},
								{"${MethodName}", methodName},
							};
							
							var recipes = new[]
							{
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(partialClassDirectory, string.Format("{0}{1}.cs", methodName, (inputDialog.IsAsync ? "Async" : string.Empty))), GetContent((inputDialog.IsAsync ? nameof(RecipeOptions.ProjectPartialClass_AsyncPartialClassMethod_Template) : nameof(RecipeOptions.ProjectPartialClass_PartialClassMethod_Template)), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
							};

							await AddFromRecipesAsync(project, recipes, contentReplacements);

							if (inputDialog.AddDTOs)
							{
								contractProjectDescription = projectDescriptions.FirstOrDefault(projectDescription => string.Equals(projectDescription.Description, inputDialog.ContractProjectDescription, StringComparison.InvariantCultureIgnoreCase));
						
								var contractProject = contractProjectDescription.Project;
								var contractProjectDirectory = GetProjectDirectory(contractProject);

								await contractProject.SaveAsync();

								var dtosDirectory = System.IO.Path.Combine(contractProjectDirectory, "DataTransferObjects", partialClassName);

								var dtosNamespace = GetNamespace(contractProjectDescription.Project, dtosDirectory);

								await AddDataTransferObjectRequestResponseAsync(solution, contractProjectDescription.Project, dtosDirectory, dtosNamespace, methodName);
							}

							await outputWindowPane.WriteLineAsync("Done\n");
							await outputWindowPane.ActivateAsync();
						}
					}
				}
			}
			catch (Exception exception)
			{
				var outputWindowPane = await GetOutputWindowPaneAsync();

				await outputWindowPane.WriteLineAsync(exception.ErrorMessageFormatted());

				throw;
			}
		}
	}
}