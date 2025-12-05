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
 
using System.ComponentModel.Design;
using ISI.Extensions.Extensions;
using ISI.VisualStudio.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public static partial class ProjectExtensions
	{
		public static void RunT4LocalContents(this EnvDTE.Project project)
		{
			foreach (EnvDTE.ProjectItem projectItem in project.ProjectItems)
			{
				RunT4LocalContents(projectItem);
			}
		}

		public static void RunT4LocalContents(EnvDTE.ProjectItem projectItem)
		{
			if (projectItem.Object is EnvDTE.Project subProject)
			{
				RunT4LocalContents(subProject);
			}

			if (projectItem.ProjectItems != null)
			{
				foreach (EnvDTE.ProjectItem subProjectItem in projectItem.ProjectItems)
				{
					RunT4LocalContents(subProjectItem);
				}
			}

			if (string.Equals(System.IO.Path.GetFileName(projectItem.Name), "T4LocalContent.tt", System.StringComparison.InvariantCultureIgnoreCase))
			{
				if (projectItem.Object is VSLangProj.VSProjectItem t4TransformsProjectItem)
				{
					t4TransformsProjectItem.RunCustomTool();
				}
			}
		}
	}
}
