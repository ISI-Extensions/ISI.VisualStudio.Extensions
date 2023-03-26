#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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

				var sortedUsingStatements = GetSortedUsings(codeExtensionProvider, usings, null);

				var contentReplacements = new Dictionary<string, string>
				{
					{ "${Usings}", string.Join(Environment.NewLine, sortedUsingStatements.GetFormatted()) },
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