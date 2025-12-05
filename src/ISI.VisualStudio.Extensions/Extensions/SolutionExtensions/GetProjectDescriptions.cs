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
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions.Extensions
{
	public static class SolutionExtensions
	{
		public class ProjectDescription
		{
			public Community.VisualStudio.Toolkit.Project Project { get; set; }
			public string Description { get; set; }
			public string RootNamespace { get; set; }
		}

		public static ProjectDescription[] GetProjectDescriptions(this Community.VisualStudio.Toolkit.Solution solution)
		{
			var projects = new List<ProjectDescription>();

			void addChildren(string path, IEnumerable<Community.VisualStudio.Toolkit.SolutionItem> solutionItems)
			{
				if (solutionItems.NullCheckedAny())
				{
					foreach (var solutionItem in solutionItems)
					{
						switch (solutionItem.Type)
						{
							case Community.VisualStudio.Toolkit.SolutionItemType.Project:
								projects.Add(new ProjectDescription()
								{
									Project = solutionItem as Community.VisualStudio.Toolkit.Project,
									Description = string.Format("{0}{1}", path, solutionItem.Name),
									RootNamespace = (solutionItem as Community.VisualStudio.Toolkit.Project)?.GetRootNamespace(),
								});
								break;

							case Community.VisualStudio.Toolkit.SolutionItemType.SolutionFolder:
								addChildren((string.IsNullOrWhiteSpace(path) ? string.Format("{0}\\", solutionItem.Name) : string.Format("{0}\\{1}\\", path, solutionItem.Name)), solutionItem.Children);
								break;
						}
					}
				}
			}

			addChildren(string.Empty, solution.Children);

			return projects.OrderBy(project => project.Description, StringComparer.InvariantCultureIgnoreCase).ToArray();
		}
	}
}
