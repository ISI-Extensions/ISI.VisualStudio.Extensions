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
		private static readonly System.Text.RegularExpressions.Regex regexLocalEntity = new("(?:(?:using)(?:\\s+)(?:LOCALENTITIES)(?:\\s+)(?:=)(?:\\s+)(?<LocalEntity>[\\w|\\.]+)(?:\\s*)(?:;))");

		public async Task AddSerializableRecordAsync()
		{
			try
			{
				var inputDialog = new AddSerializableRecordDialog();

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.NewSerializableRecordName))
				{
					var className = inputDialog.NewSerializableRecordName.Replace(" ", string.Empty);

					var solution = await VS.Solutions.GetCurrentSolutionAsync();
					var solutionItem = await VS.Solutions.GetActiveItemAsync();
					var project = await VS.Solutions.GetActiveProjectAsync();

					var @namespace = GetNamespace(project, solutionItem);

					await AddSerializableRecordAsync(solution, project, solutionItem.FullPath, @namespace, className, inputDialog.AddInterface);
				}
			}
			catch (Exception exception)
			{
				var outputWindowPane = await GetOutputWindowPaneAsync();

				await outputWindowPane.WriteLineAsync(exception.ErrorMessageFormatted());

				throw;
			}
		}

		public async Task AddSerializableRecordAsync(Solution solution, Project project, string directory, string @namespace, string className, bool addInterface)
		{
			if (!string.IsNullOrWhiteSpace(className))
			{
				var outputWindowPane = await GetOutputWindowPaneAsync();

				await outputWindowPane.ActivateAsync();

				await outputWindowPane.ClearAsync();

				await outputWindowPane.WriteLineAsync("Add Serializable Record");

				await project?.SaveAsync();

				var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
				var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

				var projectDirectory = GetProjectDirectory(project);

				if (!System.IO.Directory.Exists(directory))
				{
					System.IO.Directory.CreateDirectory(directory);
				}

				var codeExtensionProvider = project.GetCodeExtensionProvider();

				var localEntities = "XXXXXXXXXXXXXXXXXXX";

				foreach (var fullName in System.IO.Directory.GetFiles(directory, "*.cs", System.IO.SearchOption.TopDirectoryOnly))
				{
					var lines = System.IO.File.ReadAllLines(fullName);

					foreach (var line in lines)
					{
						var match = regexLocalEntity.Match(line);

						if (match.Success)
						{
							localEntities = match.Groups["LocalEntity"].Value;
						}
					}
				}

				var usings = new List<string>();
				usings.Add("using System.Runtime.Serialization;");
				usings.Add($"using LOCALENTITIES = {localEntities};");

				var classInterfaceName = $"I{className}";
				var entityClassName = className;

				var versionKey = (new System.Text.RegularExpressions.Regex("(?:V(?:(?:\\d+)|(?:\\*)))$")).Match(className)?.Value;
				if (!string.IsNullOrWhiteSpace(versionKey))
				{
					if (versionKey.EndsWith("*"))
					{
						var version = System.IO.Directory.GetFiles(directory, $"{entityClassName}.cs", System.IO.SearchOption.TopDirectoryOnly)
							.Select(fileName => System.IO.Path.GetFileNameWithoutExtension(fileName).TrimStart(entityClassName.TrimEnd('*')).ToInt())
							.NullCheckedMax() + 1;

						classInterfaceName = $"I{className.TrimEnd("V*")}";
						entityClassName = $"{className.TrimEnd("V*")}";
						className = $"{className.TrimEnd('*')}{version}";
					}
					else
					{
						classInterfaceName = $"I{className.TrimEnd(versionKey)}";
						entityClassName = $"{className.TrimEnd(versionKey)}";
					}
				}

				var sortedUsingStatements = GetSortedUsings(codeExtensionProvider, usings, null);

				var contentReplacements = new Dictionary<string, string>
				{
					{ "${Usings}", string.Join(Environment.NewLine, sortedUsingStatements.GetFormatted()) },
					{ "${Namespace}", @namespace },
					{ "${ContractUuid}", Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens) },
					{ "${ClassName}", className },
					{ "${ClassInterfaceName}", classInterfaceName },
					{ "${EntityClassName}", entityClassName },
				};

				if (codeExtensionProvider.CodeExtensionProviderUuid == ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Extensions.CodeExtensionProvider.CodeExtensionProviderUuid)
				{
					contentReplacements.Add("${SerialNamespace}", "ISI.Extensions.Serialization");
				}
				else if (codeExtensionProvider.CodeExtensionProviderUuid == ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Libraries.CodeExtensionProvider.CodeExtensionProviderUuid)
				{
					contentReplacements.Add("${SerialNamespace}", "ISI.Libraries.Serializers");
				}

				var recipes = new List<Extensions_Helper.RecipeItem>();

				if (addInterface)
				{
					recipes.Add(new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, string.Format("{0}.cs", classInterfaceName)), GetContent(nameof(RecipeOptions.Project_SerializableRecordInterface_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), false));
				}

				recipes.Add(new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, string.Format("{0}.cs", className)), GetContent(nameof(RecipeOptions.Project_SerializableRecord_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true));

				await AddFromRecipesAsync(project, recipes, contentReplacements);
			}
		}
	}
}