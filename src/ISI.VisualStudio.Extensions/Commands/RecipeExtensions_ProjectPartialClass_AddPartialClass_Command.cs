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
	[Command(PackageIds.RecipeExtensions_ProjectPartialClass_AddPartialClass_MenuItemId)]
	public class RecipeExtensions_ProjectPartialClass_AddPartialClass_Command : BaseCommand<RecipeExtensions_ProjectPartialClass_AddPartialClass_Command>
	{
		private static RecipeExtensions_ProjectPartialClass_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_ProjectPartialClass_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_ProjectPartialClass_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			if (RecipeExtensionsHelper.IsProjectRoot(solutionItem) || RecipeExtensionsHelper.IsProjectFolder(solutionItem))
			{
				showCommand = true;
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			try
			{
				var inputDialog = new InputDialog("New Partial Class");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var partialClassName = inputDialog.Value.Replace(" ", string.Empty);

					if (!string.IsNullOrWhiteSpace(partialClassName))
					{
						var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

						await outputWindowPane.ActivateAsync();

						await outputWindowPane.ClearAsync();

						await outputWindowPane.WriteLineAsync("New Partial Class");

						var solution = await VS.Solutions.GetCurrentSolutionAsync();
						var project = await VS.Solutions.GetActiveProjectAsync();
						var solutionItem = await VS.Solutions.GetActiveItemAsync();

						await project?.SaveAsync();

						var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
						var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

						var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);
						var @namespace = RecipeExtensionsHelper.GetNamespace(project, solutionItem);

						var directory = (solutionItem.Type == SolutionItemType.Project ? projectDirectory : solutionItem.FullPath);
						var partialClassDirectory = System.IO.Path.Combine(directory, partialClassName);

						if (!System.IO.Directory.Exists(partialClassDirectory))
						{
							System.IO.Directory.CreateDirectory(partialClassDirectory);

							var codeExtensionProvider = project.GetCodeExtensionProvider();

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
										Type = string.Format("{0}.I{1}Repository;", @namespace.TrimEnd(".Repository"), partialClassName.TrimEnd("Api")),
										Name = string.Format("{0}Repository;", partialClassName.TrimEnd("Api")),
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
								{"${Usings}", string.Join("\r\n", usings)},
								{"${Namespace}", @namespace},
								{"${ClassName}", partialClassName},
								{"${ClassInjectorProperties}", string.Join(string.Empty, classInjectors.Select(injector => string.Format("\t\tprotected {0} {1} {{ get; }}\r\n", injector.Type, injector.Name)))},
								{"${ClassInjectors}", string.Join(",", classInjectors.Select(injector => string.Format("\r\n\t\t\t{0} {1}", injector.Type, ISI.Extensions.StringFormat.CamelCase(injector.Name))))},
								{"${ClassInjectorAssignments}", string.Join("\r\n", classInjectors.Select(injector => string.Format("\t\t\t{0} = {1};", injector.Name, ISI.Extensions.StringFormat.CamelCase(injector.Name))))},
							};

							var recipes = new []
							{
								new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(partialClassDirectory, string.Format("__{0}.cs", partialClassName)), RecipeExtensionsHelper.GetContent(nameof(RecipeExtensionsOptions.ProjectPartialClass_AddPartialClass), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
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
