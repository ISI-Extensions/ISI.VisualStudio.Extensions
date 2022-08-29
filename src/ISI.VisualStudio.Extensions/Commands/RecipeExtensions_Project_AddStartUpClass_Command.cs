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
	[Command(PackageIds.RecipeExtensions_Project_AddStartUpClass_MenuItemId)]
	public class RecipeExtensions_Project_AddStartUpClass_Command : BaseCommand<RecipeExtensions_Project_AddStartUpClass_Command>
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

				showCommand = !project.HasStartup_cs();
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

				await outputWindowPane.WriteLineAsync("Add StartUp Class");

				var solution = await VS.Solutions.GetCurrentSolutionAsync();
				var project = await VS.Solutions.GetActiveProjectAsync();
				var solutionItem = await VS.Solutions.GetActiveItemAsync();

				await project?.SaveAsync();

				var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
				var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

				var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);
				var @namespace = project.GetRootNamespace();

				var codeExtensionProvider = project.GetCodeExtensionProvider();

				var recipeName = string.Empty;
				switch (codeExtensionProvider)
				{
					case ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Extensions.CodeExtensionProvider isiExtensionsCodeExtensionProvider:
						recipeName = nameof(RecipeOptions.Project_ISI_Extensions_StartUpClass_Template);
						break;

					case ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Libraries.CodeExtensionProvider isiLibrariesCodeExtensionProvider:
						recipeName = nameof(RecipeOptions.Project_ISI_Libraries_StartUpClass_Template);
						break;
				}

				if (!string.IsNullOrWhiteSpace(recipeName))
				{
					var sortedUsingStatements = RecipeExtensionsHelper.GetSortedUsings(codeExtensionProvider, null, null);

					var contentReplacements = new Dictionary<string, string>
					{
						{ "${Usings}", string.Join(Environment.NewLine, sortedUsingStatements.GetFormatted()) },
						{ "${Namespace}", @namespace },
					};

					var recipes = new[]
					{
						new Extensions_Helper.RecipeItem(System.IO.Path.Combine(projectDirectory, "StartUp.cs"), RecipeExtensionsHelper.GetContent(recipeName, projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
					};

					await RecipeExtensionsHelper.AddFromRecipesAsync(project, recipes, contentReplacements);
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
