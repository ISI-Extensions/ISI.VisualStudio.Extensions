#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
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
					new Extensions_Helper.RecipeItem(fullName, GetContent(nameof(RecipeOptions.Project_ServiceRegistrarClass_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), false),
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
