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
	public partial class ProjectExtensions_Helper
	{
		public async Task AddSerializableDataTransferObjectRequestResponseAsync()
		{
			try
			{
				var inputDialog = new InputDialog("Add Serializable DataTransferObject Request Response");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var methodName = inputDialog.Value.Replace(" ", string.Empty);

					var solution = await VS.Solutions.GetCurrentSolutionAsync();
					var project = await VS.Solutions.GetActiveProjectAsync();
					var solutionItem = await VS.Solutions.GetActiveItemAsync();

					var @namespace = GetNamespace(project, solutionItem);

					await AddSerializableDataTransferObjectRequestResponseAsync(solution, project, solutionItem.FullPath, @namespace, methodName);
				}
			}
			catch (Exception exception)
			{
				var outputWindowPane = await GetOutputWindowPaneAsync();

				await outputWindowPane.WriteLineAsync(exception.ErrorMessageFormatted());

				throw;
			}
		}

		public async Task AddSerializableDataTransferObjectRequestResponseAsync(Solution solution, Project project, string dtosDirectory, string @namespace, string methodName)
		{
			if (!string.IsNullOrWhiteSpace(methodName))
			{
				var outputWindowPane = await GetOutputWindowPaneAsync();

				await outputWindowPane.ActivateAsync();

				await outputWindowPane.ClearAsync();

				await outputWindowPane.WriteLineAsync("Add Serializable DataTransferObject Request Response");

				await project?.SaveAsync();

				var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
				var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

				var projectDirectory = GetProjectDirectory(project);

				if (!System.IO.Directory.Exists(dtosDirectory))
				{
					System.IO.Directory.CreateDirectory(dtosDirectory);
				}

				var codeExtensionProvider = project.GetCodeExtensionProvider();

				var usings = new List<string>();
				usings.Add("using System.Runtime.Serialization;");

				var sortedUsingStatements = GetSortedUsings(codeExtensionProvider, null, null);

				var contentReplacements = new Dictionary<string, string>
				{
					{ "${Usings}", string.Join("\r\n", sortedUsingStatements.GetFormatted()) },
					{ "${Namespace}", @namespace },
					{ "${ClassNamePrefix}", methodName },
				};

				var recipes = new[]
				{
					new Extensions_Helper.RecipeItem(System.IO.Path.Combine(dtosDirectory, string.Format("{0}Request.cs", methodName)), GetContent(nameof(RecipeOptions.Project_SerializableDataTransferObjectRequest_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
					new Extensions_Helper.RecipeItem(System.IO.Path.Combine(dtosDirectory, string.Format("{0}Response.cs", methodName)), GetContent(nameof(RecipeOptions.Project_SerializableDataTransferObjectResponse_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
				};

				await AddFromRecipesAsync(project, recipes, contentReplacements);

				await AddFromRecipesAsync(project, recipes, contentReplacements);
			}
		}
	}
}