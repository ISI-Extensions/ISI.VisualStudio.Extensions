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

					var isAsync = methodName.EndsWith("Async", StringComparison.InvariantCulture);
					if (isAsync)
					{
						methodName = methodName.Substring(0, methodName.Length - "Async".Length);
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

							var usings = new List<string>(codeExtensionProvider.DefaultUsingStatements.Select(@using => string.Format("using {0};", @using)));
							{
								var fileName = System.IO.Directory.GetFiles(partialClassDirectory).OrderBy(partialClassFileName => partialClassFileName, StringComparer.InvariantCultureIgnoreCase).FirstOrDefault();

								if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
								{
									usings = new List<string>(System.IO.File.ReadAllLines(fileName).Where(line => line.StartsWith("using ", StringComparison.InvariantCulture)));
								}
							}

							var contentReplacements = new Dictionary<string, string>
							{
								{"${Usings}", string.Join("\r\n", usings)},
								{"${Namespace}", @namespace},
								{"${ClassName}", partialClassName},
								{"${MethodName}", methodName},
							};

							var recipes = new[]
							{
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(partialClassDirectory, string.Format("{0}{1}.cs", methodName, (isAsync ? "Async" : string.Empty))), GetContent((isAsync ? nameof(RecipeOptions.ProjectPartialClass_AsyncPartialClassMethod_Template) : nameof(RecipeOptions.ProjectPartialClass_PartialClassMethod_Template)), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
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

								await AddDataTransferObjectRequestResponseAsync(solution, contractProjectDescription.Project, dtosDirectory, dtosNamespace, methodName, isAsync);
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