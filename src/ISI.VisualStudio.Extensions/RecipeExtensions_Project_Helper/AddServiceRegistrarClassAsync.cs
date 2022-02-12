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
	public partial class RecipeExtensions_Project_Helper
	{
		public async System.Threading.Tasks.Task AddServiceRegistrarClassAsync()
		{
			var outputWindowPane = await GetOutputWindowPaneAsync();

			await outputWindowPane.ActivateAsync();

			await outputWindowPane.ClearAsync();

			await outputWindowPane.WriteLineAsync("Add ServiceRegistrar Class");

			var solution = await VS.Solutions.GetCurrentSolutionAsync();
			var project = await VS.Solutions.GetActiveProjectAsync();

			await project?.SaveAsync();

			await AddServiceRegistrarClassAsync(solution, project);
		}

		public async System.Threading.Tasks.Task AddServiceRegistrarClassAsync(Solution solution, Project project, IEnumerable<(string InterfaceName, string ClassName)> serviceRegistrations = null)
		{
			try
			{
				var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
				var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

				var projectDirectory = GetProjectDirectory(project);
				var @namespace = project.GetRootNamespace();

				var codeExtensionProvider = project.GetCodeExtensionProvider();

				var usings = new List<string>(codeExtensionProvider.DefaultUsingStatements.Select(@using => string.Format("using {0};", @using)));

				var contentReplacements = new Dictionary<string, string>
				{
					{ "${Usings}", string.Join("\r\n", usings) },
					{ "${Namespace}", @namespace },
				};

				var recipes = new[]
				{
					new Extensions_Helper.RecipeItem(System.IO.Path.Combine(projectDirectory, "ServiceRegistrar.cs"), GetContent(nameof(RecipeExtensionsOptions.Project_ServiceRegistrarClass_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true,
						(projectItems, fullName, content, replacementValues) =>
						{
							if (serviceRegistrations.NullCheckedAny())
							{
								var replacementValue = string.Join("\t\t\t\t", serviceRegistrations.Select(serviceRegistration => string.Format("services.AddSingleton<{0}, {1}>();\r\n", serviceRegistration.InterfaceName, serviceRegistration.ClassName)));

								ReplaceFileContent(fullName, new Dictionary<string, string>
								{
									{ "//${ServiceRegistrations}", string.Format("{0}\t\t\t\t//${{ServiceRegistrations}}", replacementValue) },
								});
							}
						}),
				};

				await AddFromRecipesAsync(project, recipes, contentReplacements);
			}
			catch (Exception exception)
			{
				var outputWindowPane = await GetOutputWindowPaneAsync();

				await outputWindowPane.WriteLineAsync(exception.ErrorMessageFormatted());

				throw;
			}
		}
	}
}
