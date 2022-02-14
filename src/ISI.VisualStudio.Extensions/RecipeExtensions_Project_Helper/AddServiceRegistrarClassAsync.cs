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

				var sortedUsingStatements = GetSortedUsings(codeExtensionProvider);

				var contentReplacements = new Dictionary<string, string>
				{
					{ "${Usings}", sortedUsingStatements.GetFormatted() },
					{ "${Namespace}", @namespace },
				};

				var fullName = System.IO.Path.Combine(projectDirectory, "ServiceRegistrar.cs");

				var recipes = new[]
				{
					new Extensions_Helper.RecipeItem(fullName, GetContent(nameof(Options.Project_ServiceRegistrarClass_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), false),
				};

				await AddFromRecipesAsync(project, recipes, contentReplacements);

				var content = System.IO.File.ReadAllText(fullName);

				var regex = new System.Text.RegularExpressions.Regex(@"(?s:(?<start>(?:.*)(?:void)(?:\s+)(?:ServiceRegister\()(?:.*)(?:\{))(?<end>(?:.*)))");

				var match = regex.Match(content);

				if (match.Success)
				{
					var replacementValue = string.Join(string.Empty, serviceRegistrations.Select(serviceRegistration => string.Format("{2}\t\t\tservices.AddSingleton<{0}, {1}>();", serviceRegistration.InterfaceName, serviceRegistration.ClassName, Environment.NewLine)));

					content = string.Format("{0}{1}{2}", match.Groups["start"], replacementValue, match.Groups["end"]);
				}

				System.IO.File.WriteAllText(fullName, content);
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
