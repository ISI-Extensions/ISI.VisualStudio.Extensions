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
	[Command(PackageIds.RecipeExtensions_ProjectPartialClass_AddPartialClassMethod_MenuItemId)]
	public class RecipeExtensions_ProjectPartialClass_AddPartialClassMethod_Command : BaseCommand<RecipeExtensions_ProjectPartialClass_AddPartialClassMethod_Command>
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
				var inputDialog = new InputDialog("New Partial Class Method");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var methodName = inputDialog.Value.Replace(" ", string.Empty);

					var isAsync = methodName.EndsWith("Async", StringComparison.InvariantCulture);
					if (isAsync)
					{
						methodName = methodName.Substring(0, methodName.Length - "Async".Length);
					}

					if (!string.IsNullOrWhiteSpace(methodName))
					{
						var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

						await outputWindowPane.ActivateAsync();

						await outputWindowPane.ClearAsync();

						await outputWindowPane.WriteLineAsync("New Partial Class Method");

						var solution = await VS.Solutions.GetCurrentSolutionAsync();
						var project = await VS.Solutions.GetActiveProjectAsync();
						var solutionItem = await VS.Solutions.GetActiveItemAsync();

						await project?.SaveAsync();

						var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
						var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

						var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);
						var partialClassDirectory = solutionItem.FullPath;

						var partialClassName = partialClassDirectory.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last();

						var @namespace = RecipeExtensionsHelper.GetNamespace(project, solutionItem, partialClassName);

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
								new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(partialClassDirectory, string.Format("{0}{1}.cs", methodName, (isAsync ? "Async" : string.Empty))), RecipeExtensionsHelper.GetContent((isAsync ? nameof(RecipeExtensionsOptions.ProjectPartialClass_AddAsyncPartialClassMethod) : nameof(RecipeExtensionsOptions.ProjectPartialClass_AddPartialClassMethod)), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
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
