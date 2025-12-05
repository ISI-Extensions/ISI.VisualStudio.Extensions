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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using ISI.Extensions.Extensions;
using ISI.Extensions.VisualStudio;

namespace ISI.VisualStudio.Extensions
{
	public partial class SolutionExtensions_Helper
	{
		public delegate void AddMissingLocalSourcePackagesProgress(string project, int index, int count);

		public void AddMissingLocalSourcePackages(EnvDTE80.DTE2 dte, object solutionItem, AddMissingLocalSourcePackagesProgress progress)
		{
			progress ??= (project, index, count) => { };

			var solution = (EnvDTE80.Solution2)dte.Solution;

			var solutionDetails = SolutionApi.GetSolutionDetails(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.GetSolutionDetailsRequest()
			{
				Solution = solution.FullName,
			}).SolutionDetails;

			var existingProjectNames = new HashSet<string>(solutionDetails.ProjectDetailsSet.Select(projectDetails => projectDetails.ProjectName) ,StringComparer.InvariantCultureIgnoreCase);

			var projectQueue = new Queue<ISI.Extensions.VisualStudio.ProjectDetails>();

			switch (solutionItem)
			{
				case string projectFullName:
					{
						var projectDetails = ProjectApi.GetProjectDetails(new ISI.Extensions.VisualStudio.DataTransferObjects.ProjectApi.GetProjectDetailsRequest()
						{
							Project = projectFullName,
						}).ProjectDetails;

						if (projectDetails != null)
						{
							projectQueue.Enqueue(projectDetails);
						}
					}
					break;

				case Community.VisualStudio.Toolkit.Project project:
					{
						var projectDetails = ProjectApi.GetProjectDetails(new ISI.Extensions.VisualStudio.DataTransferObjects.ProjectApi.GetProjectDetailsRequest()
						{
							Project = project.FullPath,
						}).ProjectDetails;

						if (projectDetails != null)
						{
							projectQueue.Enqueue(projectDetails);
						}
					}
					break;

				case Community.VisualStudio.Toolkit.Solution solution1:
					foreach (var projectDetails in solutionDetails.ProjectDetailsSet.OrderBy(projectDetails => projectDetails.ProjectName, StringComparer.InvariantCultureIgnoreCase))
					{
						projectQueue.Enqueue(projectDetails);
					}
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(solutionItem));
			}

			var projectCount = projectQueue.Count;
			var projectIndex = 0;
			while (projectQueue.Any())
			{
				var projectDetails = projectQueue.Dequeue();

				var projectReferences = ProjectApi.GetProjectReferences(new ISI.Extensions.VisualStudio.DataTransferObjects.ProjectApi.GetProjectReferencesRequest()
				{
					Project = projectDetails.ProjectFullName,
				}).ProjectReferences;

				if (projectReferences.NullCheckedAny())
				{
					foreach (var projectReference in projectReferences)
					{
						if (!existingProjectNames.Contains(projectReference.Name))
						{
							progress(projectReference.Name, projectIndex, projectCount);

							AddLocalSourceProjectToSolution(solution, projectReference.Path);

							projectDetails = ProjectApi.GetProjectDetails(new ISI.Extensions.VisualStudio.DataTransferObjects.ProjectApi.GetProjectDetailsRequest()
							{
								Project = projectReference.Path,
							}).ProjectDetails;

							if (projectDetails != null)
							{
								projectQueue.Enqueue(projectDetails);
								projectCount++;
							}

							existingProjectNames.Add(projectReference.Name);
						}
					}
				}

				projectIndex++;
			}
		}
	}
}