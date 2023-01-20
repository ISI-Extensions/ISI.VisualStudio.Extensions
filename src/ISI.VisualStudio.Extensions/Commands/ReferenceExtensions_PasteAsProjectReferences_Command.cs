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
using System.Windows;
using System.Threading.Tasks;
using ISI.Extensions.VisualStudio;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.ReferenceExtensions_PasteAsProjectReferences_MenuItemId)]
	public class ReferenceExtensions_PasteAsProjectReferences_Command : BaseCommand<ReferenceExtensions_PasteAsProjectReferences_Command>
	{
		private static ProjectExtensions_Helper _projectExtensionsHelper = null;
		protected ProjectExtensions_Helper ProjectExtensionsHelper => _projectExtensionsHelper ??= Package.GetServiceProvider().GetService<ProjectExtensions_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var projectReferences = ProjectExtensionsHelper.GetProjectReferencesFromClipboard();

			if (projectReferences.NullCheckedAny())
			{
				var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

				showCommand = ProjectExtensionsHelper.IsReferencesFolder(solutionItem);
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			var projectReferences = ProjectExtensionsHelper.GetProjectReferencesFromClipboard();

			if (projectReferences.Any())
			{
				var solutionProjects = new System.Collections.Generic.Dictionary<string, EnvDTE.Project>(StringComparer.InvariantCultureIgnoreCase);

				void addSolutionProjects(System.Collections.Generic.IEnumerable<EnvDTE.Project> projects)
				{
					if (projects != null)
					{
						foreach (var project in projects)
						{
							if (project.Object is VSLangProj.VSProject)
							{
								solutionProjects.Add(project.Name, project);
							}
							else if (project is EnvDTE.Project solutionProject)
							{
								addSolutionProjects(solutionProject.ProjectItems.Cast<EnvDTE.ProjectItem>().Select(x => x.SubProject).OfType<EnvDTE.Project>());
							}
						}
					}
				}

				addSolutionProjects(Package.GetDTE2().Solution.Projects.Cast<EnvDTE.Project>());

				var projects = projectReferences.Select(projectReference =>
					{
						if (solutionProjects.TryGetValue(projectReference.Name, out var project))
						{
							return project;
						}

						return null;
					}).Where(project => project != null)
					.ToArray();

				if (projects.Any())
				{
					var selectedProject = ((Package.GetDTE2().ActiveSolutionProjects as Array).GetValue(0) as EnvDTE.Project)?.Object as VSLangProj.VSProject;

					if (selectedProject != null)
					{
						foreach (var project in projects)
						{
							selectedProject.References.AddProject(project);
						}
					}
				}
			}
		}
	}
}