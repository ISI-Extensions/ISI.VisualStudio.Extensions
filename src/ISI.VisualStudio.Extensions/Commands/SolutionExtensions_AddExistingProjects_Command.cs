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
	[Command(PackageIds.SolutionExtensionsAddExistingProjectsMenuItemId)]
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