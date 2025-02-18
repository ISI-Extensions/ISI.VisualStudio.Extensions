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

				var sortedUsingStatements = RecipeExtensionsHelper.GetSortedUsings(codeExtensionProvider, null, fileNames);

				foreach (var fileName in fileNames)
				{
					if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
					{
						var lines = new List<string>();

						var insertUsingStatementsIndex = 0;

						var inUsingSection = false;
						foreach (var line in System.IO.File.ReadAllLines(fileName))
						{
							var currentLine = line.Replace('\t', ' ').Trim().Split([';'], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
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
						var usings = sortedUsingStatements.ToArray();
						for (var index = usings.Length - 1; index >= 0; index--)
						{
							lines.Insert(insertUsingStatementsIndex, string.Format("using {0};", usings[index]));
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
