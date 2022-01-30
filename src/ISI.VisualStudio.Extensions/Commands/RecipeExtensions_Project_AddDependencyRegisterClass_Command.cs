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
	[Command(PackageIds.RecipeExtensions_Project_AddDependencyRegisterClass_MenuItemId)]
	public class RecipeExtensions_Project_AddDependencyRegisterClass_Command : BaseCommand<RecipeExtensions_Project_AddDependencyRegisterClass_Command>
	{
		private static RecipeExtensions_Project_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_Project_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_Project_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			if (RecipeExtensionsHelper.IsProjectRoot(solutionItem))
			{
				var project = VS.Solutions.GetActiveProjectAsync().GetAwaiter().GetResult();

				var codeExtensionProvider = project.GetCodeExtensionProvider();

				showCommand = (codeExtensionProvider.CodeExtensionProviderUuid == ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Libraries.CodeExtensionProvider.CodeExtensionProviderUuid);
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

				await outputWindowPane.WriteLineAsync("Add DependencyRegister Class");

				var solution = await VS.Solutions.GetCurrentSolutionAsync();
				var project = await VS.Solutions.GetActiveProjectAsync();
				var solutionItem = await VS.Solutions.GetActiveItemAsync();

				await project?.SaveAsync();

				var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
				var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

				var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);
				var @namespace = RecipeExtensionsHelper.GetRootNamespace(project);

				var codeExtensionProvider = project.GetCodeExtensionProvider();

				var usings = new List<string>(codeExtensionProvider.DefaultUsingStatements.Select(@using => string.Format("using {0};", @using)));

				var contentReplacements = new Dictionary<string, string>
				{
					{ "${Usings}", string.Join("\r\n", usings) },
					{ "${Namespace}", @namespace },
				};

				var recipes = new[]
				{
					new ExtensionsHelper.RecipeItem(System.IO.Path.Combine(projectDirectory, "DependencyRegister.cs"), RecipeExtensionsHelper.GetContent(nameof(RecipeExtensionsOptions.Project_AddDependencyRegisterClass), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
				};


				await RecipeExtensionsHelper.AddFromRecipesAsync(project, recipes, contentReplacements);

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
