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
	[Command(PackageIds.RecipeExtensions_ProjectPartialClass_AddPartialClassPrivateMethod_MenuItemId)]
	public class RecipeExtensions_ProjectPartialClass_AddPartialClassPrivateMethod_Command : BaseCommand<RecipeExtensions_ProjectPartialClass_AddPartialClassPrivateMethod_Command>
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
				var inputDialog = new InputDialog("New Partial Class Private Method");

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

						await outputWindowPane.WriteLineAsync("New Partial Class Private Method");

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

							var controllerFileName = System.IO.Directory.GetFiles(partialClassDirectory).OrderBy(partialClassFileName => partialClassFileName, StringComparer.InvariantCultureIgnoreCase).FirstOrDefault();
							var sortedUsingStatements = RecipeExtensionsHelper.GetSortedUsings(codeExtensionProvider, null, new []{controllerFileName});

							var contentReplacements = new Dictionary<string, string>
							{
								{ "${Usings}", sortedUsingStatements.GetFormatted() },
								{"${Namespace}", @namespace},
								{"${ClassName}", partialClassName},
								{"${MethodName}", methodName},
							};

							var recipes = new[]
							{
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(partialClassDirectory, string.Format("_{0}{1}.cs", methodName, (isAsync ? "Async" : string.Empty))), RecipeExtensionsHelper.GetContent((isAsync ? nameof(RecipeOptions.ProjectPartialClass_AsyncPartialClassPrivateMethod_Template) : nameof(RecipeOptions.ProjectPartialClass_PartialClassPrivateMethod_Template)), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
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
