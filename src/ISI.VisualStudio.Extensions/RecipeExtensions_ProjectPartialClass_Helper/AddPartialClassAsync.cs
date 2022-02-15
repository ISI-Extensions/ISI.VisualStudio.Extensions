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
		public async Task AddPartialClassAsync()
		{
			try
			{
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

				var inputDialog = new AddPartialClassDialog(projectDescriptions, contractProjectDescription);

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.NewPartialClassName))
				{
					var partialClassName = inputDialog.NewPartialClassName;

					if (!string.IsNullOrWhiteSpace(partialClassName))
					{
						contractProjectDescription = projectDescriptions.FirstOrDefault(projectDescription => string.Equals(projectDescription.Description, inputDialog.ContractProjectDescription, StringComparison.InvariantCultureIgnoreCase));

						await contractProjectDescription.Project?.SaveAsync();

						await AddPartialClassAsync(solution, project, solutionItem, partialClassName, contractProjectDescription.Project, inputDialog.AddInterface, inputDialog.AddIocRegistry);
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

		public async System.Threading.Tasks.Task AddPartialClassAsync(Solution solution, Project project, SolutionItem solutionItem, string partialClassName, Project contractProject, bool addInterface, bool addIocRegistry)
		{
			try
			{
				var outputWindowPane = await GetOutputWindowPaneAsync();

				await outputWindowPane.ActivateAsync();

				await outputWindowPane.ClearAsync();

				await outputWindowPane.WriteLineAsync("New Partial Class");

				var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
				var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

				var projectDirectory = GetProjectDirectory(project);
				var @namespace = GetNamespace(project, solutionItem);

				var contractProjectDirectory = GetProjectDirectory(contractProject);

				var directory = (solutionItem.Type == SolutionItemType.Project ? projectDirectory : solutionItem.FullPath);
				var partialClassDirectory = System.IO.Path.Combine(directory, partialClassName);

				if (!System.IO.Directory.Exists(partialClassDirectory))
				{
					System.IO.Directory.CreateDirectory(partialClassDirectory);

					var codeExtensionProvider = project.GetCodeExtensionProvider();

					{
						var usings = new List<string>(codeExtensionProvider.DefaultUsingStatements.Select(@using => string.Format("using {0};", @using)));
						var classInjectors = codeExtensionProvider.DefaultClassInjectors.ToList();

						if (partialClassName.EndsWith("Api", StringComparison.InvariantCulture))
						{
							try
							{
								usings.Add(string.Format("using DTOs = {0}.DataTransferObjects.{1};", @namespace.TrimEnd(".Api"), partialClassName));
							}
							catch
							{
							}

							try
							{
								usings.Add(string.Format("using RepositoryDTOs = {0}.DataTransferObjects.{1}Repository;", @namespace.TrimEnd(".Repository"), partialClassName.TrimEnd("Api")));
							}
							catch
							{
							}

							try
							{
								classInjectors.Add(new ISI.Extensions.VisualStudio.CodeGenerationClassInjector()
								{
									Type = string.Format("{0}.I{1}Repository", @namespace.TrimEnd(".Repository"), partialClassName.TrimEnd("Api")),
									Name = string.Format("{0}Repository", partialClassName.TrimEnd("Api")),
								});
							}
							catch
							{
							}
						}

						if (partialClassName.EndsWith("Repository", StringComparison.InvariantCulture))
						{
							try
							{
								usings.Add(string.Format("using DTOs = {0}.DataTransferObjects.{1};", @namespace.TrimEnd(".Repository"), partialClassName));
							}
							catch
							{
							}
						}

						var contentReplacements = new Dictionary<string, string>
						{
							{ "${Usings}", string.Join("\r\n", usings) },
							{ "${Namespace}", @namespace },
							{ "${ClassName}", partialClassName },
							{ "${ClassInjectorProperties}", string.Join(string.Empty, classInjectors.Select(injector => string.Format("\t\tprotected {0} {1} {{ get; }}\r\n", injector.Type, injector.Name))) },
							{ "${ClassInjectors}", string.Join(",", classInjectors.Select(injector => string.Format("\r\n\t\t\t{0} {1}", injector.Type, ISI.Extensions.StringFormat.CamelCase(injector.Name)))) },
							{ "${ClassInjectorAssignments}", string.Join("\r\n", classInjectors.Select(injector => string.Format("\t\t\t{0} = {1};", injector.Name, ISI.Extensions.StringFormat.CamelCase(injector.Name)))) },
						};

						var recipes = new[]
						{
							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(partialClassDirectory, string.Format("__{0}.cs", partialClassName)), GetContent(nameof(RecipeOptions.ProjectPartialClass_PartialClass_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
						};

						await AddFromRecipesAsync(project, recipes, contentReplacements);
					}

					if (addInterface)
					{
						var usings = new List<string>(codeExtensionProvider.DefaultUsingStatements.Select(@using => string.Format("using {0};", @using)));

						if (partialClassName.EndsWith("Api", StringComparison.InvariantCulture))
						{
							try
							{
								usings.Add(string.Format("using DTOs = {0}.DataTransferObjects.{1};", @namespace.TrimEnd(".Api"), partialClassName));
							}
							catch
							{
							}
						}

						if (partialClassName.EndsWith("Repository", StringComparison.InvariantCulture))
						{
							try
							{
								usings.Add(string.Format("using DTOs = {0}.DataTransferObjects.{1};", @namespace.TrimEnd(".Repository"), partialClassName));
							}
							catch
							{
							}
						}

						var interfaceName = string.Format("I{0}", partialClassName);

						var contentReplacements = new Dictionary<string, string>
						{
							{ "${Usings}", string.Join("\r\n", usings) },
							{ "${Namespace}", @namespace },
							{ "${InterfaceName}", interfaceName },
						};

						var recipes = new[]
						{
							new Extensions_Helper.RecipeItem(System.IO.Path.Combine(contractProjectDirectory, string.Format("I{0}.cs", partialClassName)), GetContent(nameof(RecipeOptions.ProjectPartialClass_PartialClassInterface_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
						};

						await AddFromRecipesAsync(contractProject, recipes, contentReplacements);
					}

					if (addIocRegistry)
					{
						var serviceRegistrations = new List<(string InterfaceName, string ClassName)>();
						serviceRegistrations.Add((InterfaceName: string.Format("I{0}", partialClassName), ClassName: partialClassName));

						if (codeExtensionProvider.CodeExtensionProviderUuid == ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Extensions.CodeExtensionProvider.CodeExtensionProviderUuid)
						{
							await AddServiceRegistrarClassAsync(solution, contractProject, serviceRegistrations);
						}
						else if (codeExtensionProvider.CodeExtensionProviderUuid == ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Libraries.CodeExtensionProvider.CodeExtensionProviderUuid)
						{
							await AddDependencyRegisterClassAsync(solution, contractProject, serviceRegistrations);
						}
					}


					await outputWindowPane.WriteLineAsync("Done\n");
					await outputWindowPane.ActivateAsync();
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
