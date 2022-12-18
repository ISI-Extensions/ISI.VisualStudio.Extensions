﻿#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class SolutionExtensions_Helper
	{
		public delegate void UseLocalSourcePackagesProgress(string project, int index, int count);

		public void UseLocalSourcePackages(EnvDTE80.DTE2 dte, Community.VisualStudio.Toolkit.SolutionItem solutionItem, UseLocalSourcePackagesProgress progress)
		{
			var solution = (EnvDTE80.Solution2)dte.Solution;

			SolutionApi.UseLocalSourcePackages(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.UseLocalSourcePackagesRequest()
			{
				SolutionItem = solutionItem.FullPath,
				AddProject = projectFullName =>
				{
					AddLocalSourceProjectToSolution(solution, projectFullName);
					AddMissingLocalSourcePackages(dte, projectFullName, null);
				},
				Progress = (projectName, index, count) => progress(projectName, index, count),
			});
		}
	}
}