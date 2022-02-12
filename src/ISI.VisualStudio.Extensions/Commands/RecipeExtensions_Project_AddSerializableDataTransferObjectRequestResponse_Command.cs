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
	[Command(PackageIds.RecipeExtensions_Project_AddSerializableDataTransferObjectRequestResponse_MenuItemId)]
	public class RecipeExtensions_Project_AddSerializableDataTransferObjectRequestResponse_Command : BaseCommand<RecipeExtensions_Project_AddSerializableDataTransferObjectRequestResponse_Command>
	{
		private static RecipeExtensions_ProjectPartialClass_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_ProjectPartialClass_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_ProjectPartialClass_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			if (RecipeExtensionsHelper.IsProjectFolder(solutionItem))
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
				var inputDialog = new InputDialog("Add Serializable DataTransferObject Request Response");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var classNamePrefix = inputDialog.Value.Replace(" ", string.Empty);

					if (!string.IsNullOrWhiteSpace(classNamePrefix))
					{
						var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

						await outputWindowPane.ActivateAsync();

						await outputWindowPane.ClearAsync();

						await outputWindowPane.WriteLineAsync("Add Serializable DataTransferObject Request Response");

						var solution = await VS.Solutions.GetCurrentSolutionAsync();
						var project = await VS.Solutions.GetActiveProjectAsync();
						var solutionItem = await VS.Solutions.GetActiveItemAsync();

						await project?.SaveAsync();

						var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
						var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

						var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);
						var @namespace = RecipeExtensionsHelper.GetNamespace(project, solutionItem);

						var directory = solutionItem.FullPath;

						var codeExtensionProvider = project.GetCodeExtensionProvider();

						var usings = new List<string>(codeExtensionProvider.DefaultUsingStatements.Select(@using => string.Format("using {0};", @using)));
						usings.Add("using System.Runtime.Serialization;");

						var contentReplacements = new Dictionary<string, string>
							{
								{"${Usings}", string.Join("\r\n", usings)},
								{"${Namespace}", @namespace},
								{"${ClassNamePrefix}", classNamePrefix},
							};

						var recipes = new[]
						{
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, string.Format("{0}Request.cs", classNamePrefix)), RecipeExtensionsHelper.GetContent(nameof(RecipeExtensionsOptions.Project_SerializableDataTransferObjectRequest_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, string.Format("{0}Response.cs", classNamePrefix)), RecipeExtensionsHelper.GetContent(nameof(RecipeExtensionsOptions.Project_SerializableDataTransferObjectResponse_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
							};

						await RecipeExtensionsHelper.AddFromRecipesAsync(project, recipes, contentReplacements);

						await outputWindowPane.WriteLineAsync("Done\n");
						await outputWindowPane.ActivateAsync();
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
