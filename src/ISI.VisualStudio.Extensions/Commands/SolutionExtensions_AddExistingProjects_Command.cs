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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.SolutionExtensions_AddExistingProjects_MenuItemId)]
	public class SolutionExtensions_AddExistingProjects_Command : BaseCommand<SolutionExtensions_AddExistingProjects_Command>
	{
		private static SolutionExtensions_Helper _solutionExtensionsHelper = null;
		protected SolutionExtensions_Helper SolutionExtensionsHelper => _solutionExtensionsHelper ??= Package.GetServiceProvider().GetService<SolutionExtensions_Helper>();

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var dte = Package.GetDTE2();
			var solution = (EnvDTE80.Solution2)dte.Solution;

			using (var openFileDialog = new FolderSelectDialog())
			{
				openFileDialog.Title = "Select project folders";
				openFileDialog.Multiselect = true;
				openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(solution.FullName);

				if (openFileDialog.ShowDialog())
				{
					var outputWindowPane = await SolutionExtensionsHelper.GetOutputWindowPaneAsync();

					await outputWindowPane.ActivateAsync();

					await outputWindowPane.ClearAsync();

					await outputWindowPane.WriteLineAsync("Add Existing Projects");

					var folderNames = openFileDialog.FileNames.ToArray();

					var projectFileNames = new List<string>();

					foreach (var folderName in folderNames)
					{
						var fileNames = System.IO.Directory.GetFiles(folderName);

						foreach (var fileName in fileNames)
						{
							var fileExtension = System.IO.Path.GetExtension(fileName);

							if (SolutionExtensionsHelper.ProjectExtensions.Contains(fileExtension))
							{
								projectFileNames.Add(fileName);
							}
						}
					}

					try
					{
						dte.UndoContext.Open("Add Existing Projects");

						var parent = dte.GetSelectedSolutionItems().NullCheckedFirstOrDefault();

						if (parent?.Object is EnvDTE80.Solution2)
						{
							foreach (var projectFileName in projectFileNames)
							{
								try
								{
									solution.AddFromFile(projectFileName, false);
								}
								catch (Exception exception)
								{
									await outputWindowPane.WriteLineAsync(string.Format("Could not add {0}: {1}\n", projectFileName, exception.Message));
								}
							}
						}
						else if (parent?.Object is EnvDTE.Project project)
						{
							if (project.Kind == SolutionExtensions_Helper.vsProjectKindSolutionItems)
							{
								if (project.Object is EnvDTE80.SolutionFolder solutionFolder)
								{
									foreach (var projectFileName in projectFileNames)
									{
										try
										{
											solutionFolder.AddFromFile(projectFileName);
										}
										catch (Exception exception)
										{
											await outputWindowPane.WriteLineAsync(string.Format("Could not add {0}: {1}\n", projectFileName, exception.Message));
										}
									}
								}
							}
						}
					}
					finally
					{
						dte.UndoContext.Close();
					}
				}
			}
		}
	}
}