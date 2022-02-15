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
	[Command(PackageIds.RecipeExtensions_Project_AddEnumTextTemplate_MenuItemId)]
	public class RecipeExtensions_Project_AddEnumTextTemplate_Command : BaseCommand<RecipeExtensions_Project_AddEnumTextTemplate_Command>
	{
		private static RecipeExtensions_Project_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_Project_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_Project_Helper>();

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
				var solutionItem = await VS.Solutions.GetActiveItemAsync();
				var solution = await VS.Solutions.GetCurrentSolutionAsync();
				var project = await VS.Solutions.GetActiveProjectAsync();

				var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
				var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

				var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);

				var directory = solutionItem.FullPath;
				while (!System.IO.Directory.Exists(directory))
				{
					directory = System.IO.Path.GetDirectoryName(directory);
				}

				var @namespace = RecipeExtensionsHelper.GetNamespace(project, solutionItem);

				var addEnumTextTemplateDialog = new AddEnumTextTemplateDialog(string.Empty, @namespace, string.Empty, string.Empty, string.Empty, string.Empty, "|");

				var addEnumTextTemplateDialogResult = await addEnumTextTemplateDialog.ShowDialogAsync();

				if (addEnumTextTemplateDialogResult.GetValueOrDefault())
				{
					var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

					await outputWindowPane.ActivateAsync();

					await outputWindowPane.ClearAsync();

					await outputWindowPane.WriteLineAsync("New Enum Text Template");

					var contentReplacements = new Dictionary<string, string>
					{
						{ "${Namespace}", addEnumTextTemplateDialog.Namespace },
						{ "${ConnectionString}", addEnumTextTemplateDialog.ConnectionString },
						{ "${EnumTableName}", addEnumTextTemplateDialog.EnumTableName },
						{ "${EnumIdColumnName}", addEnumTextTemplateDialog.EnumIdColumnName },
						{ "${EnumUuidColumnName}", addEnumTextTemplateDialog.EnumUuidColumnName },
						{ "${AliasesDelimiter}", addEnumTextTemplateDialog.AliasesDelimiter },
					};

					var recipes = new Extensions_Helper.RecipeItem[]
					{
						new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, string.Format("{0}.tt", addEnumTextTemplateDialog.EnumName)), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.Project_EnumText_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
					};

					await RecipeExtensionsHelper.AddFromRecipesAsync(project, recipes, contentReplacements);

					await outputWindowPane.WriteLineAsync("Done\n");
					await outputWindowPane.ActivateAsync();
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
