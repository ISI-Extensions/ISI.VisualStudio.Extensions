#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
	public partial class ProjectExtensions_Helper
	{
		public async Task AddSerializableObjectAsync()
		{
			try
			{
				var inputDialog = new InputDialog("Add Serializable Object");

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
				{
					var className = inputDialog.Value.Replace(" ", string.Empty);

					var solution = await VS.Solutions.GetCurrentSolutionAsync();
					var project = await VS.Solutions.GetActiveProjectAsync();
					var solutionItem = await VS.Solutions.GetActiveItemAsync();

					var @namespace = GetNamespace(project, solutionItem);

					await AddSerializableObjectAsync(solution, project, solutionItem.FullPath, @namespace, className);
				}
			}
			catch (Exception exception)
			{
				var outputWindowPane = await GetOutputWindowPaneAsync();

				await outputWindowPane.WriteLineAsync(exception.ErrorMessageFormatted());

				throw;
			}
		}

		public async Task AddSerializableObjectAsync(Solution solution, Project project, string directory, string @namespace, string className)
		{
			if (!string.IsNullOrWhiteSpace(className))
			{
				var outputWindowPane = await GetOutputWindowPaneAsync();

				await outputWindowPane.ActivateAsync();

				await outputWindowPane.ClearAsync();

				await outputWindowPane.WriteLineAsync("Add Serializable Object");

				await project?.SaveAsync();

				var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
				var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

				var projectDirectory = GetProjectDirectory(project);

				if (!System.IO.Directory.Exists(directory))
				{
					System.IO.Directory.CreateDirectory(directory);
				}

				var codeExtensionProvider = project.GetCodeExtensionProvider();

				var usings = new List<string>();
				usings.Add("using System.Runtime.Serialization;");
				usings.Add("using LOCALENTITIES = XXXXXXX;");

				var sortedUsingStatements = GetSortedUsings(codeExtensionProvider, usings, null);

				var contentReplacements = new Dictionary<string, string>
				{
					{ "${Usings}", string.Join(Environment.NewLine, sortedUsingStatements.GetFormatted()) },
					{ "${Namespace}", @namespace },
					{ "${ContractUuid}", Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens) },
					{ "${ClassName}", className },
					{ "${ClassInterfaceName}", className },
					{ "${EntityClassName}", className },
				};

				if (codeExtensionProvider.CodeExtensionProviderUuid == ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Extensions.CodeExtensionProvider.CodeExtensionProviderUuid)
				{
					contentReplacements.Add("${SerialNamespace}", "ISI.Extensions.Serialization");
				}
				else if (codeExtensionProvider.CodeExtensionProviderUuid == ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Libraries.CodeExtensionProvider.CodeExtensionProviderUuid)
				{
					contentReplacements.Add("${SerialNamespace}", "ISI.Libraries.Serializers");
				}

				var recipes = new[]
				{
					new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, string.Format("{0}.cs", className)), GetContent(nameof(RecipeOptions.Project_SerializableRecord_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
				};

				await AddFromRecipesAsync(project, recipes, contentReplacements);
			}
		}
	}
}