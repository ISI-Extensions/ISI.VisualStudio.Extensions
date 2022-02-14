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
	[Command(PackageIds.RecipeExtensions_Project_AddT4LocalContent_MenuItemId)]
	public class RecipeExtensions_Project_AddT4LocalContent_Command : BaseCommand<RecipeExtensions_Project_AddT4LocalContent_Command>
	{
		private static RecipeExtensions_Project_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_Project_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_Project_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			if (RecipeExtensionsHelper.IsProjectRoot(solutionItem))
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

				directory = System.IO.Path.Combine(directory, "T4LocalContent");

				var @namespace = RecipeExtensionsHelper.GetNamespace(project, solutionItem);

				var referenceNames = project.References.ToNullCheckedHashSet(reference => reference.Name, NullCheckCollectionResult.Empty);

				var isWebRoot = referenceNames.Contains("ISI.Libraries.Web.Mvc") ||  referenceNames.Contains("ISI.Extensions.AspNetCore");
				var buildT4Links = referenceNames.Contains("ISI.Libraries.Web.Mvc");

				var addT4LocalContentDialog = new AddT4LocalContentDialog(isWebRoot, true, buildT4Links, false, false);

				var addEnumTextTemplateDialogResult = await addT4LocalContentDialog.ShowDialogAsync();

				if (addEnumTextTemplateDialogResult.GetValueOrDefault())
				{
					var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

					await outputWindowPane.ActivateAsync();

					await outputWindowPane.ClearAsync();

					await outputWindowPane.WriteLineAsync("Add T4LocalContent");

					System.IO.Directory.CreateDirectory(directory);

					var codeExtensionProvider = project.GetCodeExtensionProvider();

					var rootGenerator = string.Empty;
					if (codeExtensionProvider.CodeExtensionProviderUuid == ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Extensions.CodeExtensionProvider.CodeExtensionProviderUuid)
					{
						rootGenerator = "ISI.ExtensionsFileProviders.EmbeddedVolumesFileProvider.GetPathPrefix";
					}
else					if (codeExtensionProvider.CodeExtensionProviderUuid == ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Libraries.CodeExtensionProvider.CodeExtensionProviderUuid)
					{
						rootGenerator = "ISI.Libraries.Configuration.GetUrlRoot";
					}


					var contentReplacements = new Dictionary<string, string>
					{
						{ "${Namespace}", @namespace },
						{ "${ExtensionsRoot}", codeExtensionProvider.Namespace },
						{ "${IsWebRoot}", addT4LocalContentDialog.IsWebRoot.TrueFalse(false, BooleanExtensions.TextCase.Lower) },
						{ "${BuildT4Files}", addT4LocalContentDialog.BuildT4Files.TrueFalse(false, BooleanExtensions.TextCase.Lower) },
						{ "${BuildT4Links}", addT4LocalContentDialog.BuildT4Links.TrueFalse(false, BooleanExtensions.TextCase.Lower) },
						{ "${BuildT4Embedded}", addT4LocalContentDialog.BuildT4Embedded.TrueFalse(false, BooleanExtensions.TextCase.Lower) },
						{ "${BuildT4Resources}", addT4LocalContentDialog.BuildT4Resources.TrueFalse(false, BooleanExtensions.TextCase.Lower) },
						{ "${RootGenerator}", rootGenerator },
					};

					var recipes = new Extensions_Helper.RecipeItem[]
					{
						new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, "T4LocalContent.settings.t4"), RecipeExtensionsHelper.GetContent(nameof(Options.Project_T4LocalContent_Settings_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
						new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, "T4LocalContent.Generator.t4"), RecipeExtensionsHelper.GetContent(nameof(Options.Project_T4LocalContent_Generator_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
						new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, "T4LocalContent.tt"), RecipeExtensionsHelper.GetContent(nameof(Options.Project_T4LocalContent_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
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
