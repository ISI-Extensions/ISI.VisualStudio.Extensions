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
	[Command(PackageIds.RecipeExtensions_ProjectPartialClass_SyncPartialClassUsingStatements_MenuItemId)]
	public class RecipeExtensions_ProjectPartialClass_SyncPartialClassUsingStatements_Command : BaseCommand<RecipeExtensions_ProjectPartialClass_SyncPartialClassUsingStatements_Command>
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
				var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

				await outputWindowPane.ActivateAsync();

				await outputWindowPane.ClearAsync();

				await outputWindowPane.WriteLineAsync("Sync Partial Class Using");

				var project = await VS.Solutions.GetActiveProjectAsync();
				var solutionItem = await VS.Solutions.GetActiveItemAsync();

				await project?.SaveAsync();

				var partialClassDirectory = solutionItem.FullPath;

				var codeExtensionProvider = project.GetCodeExtensionProvider();

				var fileNames = System.IO.Directory.GetFiles(partialClassDirectory, "*.cs");

				var sortedUsingStatements = RecipeExtensionsHelper.GetSortedUsings(codeExtensionProvider.DefaultUsingStatements, fileNames);

				foreach (var fileName in fileNames)
				{
					if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
					{
						var lines = new List<string>();

						var insertUsingStatementsIndex = 0;

						var inUsingSection = false;
						foreach (var line in System.IO.File.ReadAllLines(fileName))
						{
							var currentLine = line.Replace('\t', ' ').Trim().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
							if (currentLine.StartsWith("using ") && (currentLine.Length > 6) && (currentLine.IndexOf("(") < 0))
							{
								insertUsingStatementsIndex = lines.Count;
								inUsingSection = true;
							}
							else if (inUsingSection && string.IsNullOrWhiteSpace(line))
							{
								insertUsingStatementsIndex = lines.Count;
							}
							else
							{
								inUsingSection = false;
								lines.Add(line);
							}
						}

						lines.Insert(insertUsingStatementsIndex, string.Empty);
						for (int index = sortedUsingStatements.Count - 1; index >= 0; index--)
						{
							lines.Insert(insertUsingStatementsIndex, string.Format("using {0};", sortedUsingStatements[index]));
						}

						System.IO.File.WriteAllText(fileName, string.Join(Environment.NewLine, lines));
					}
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
