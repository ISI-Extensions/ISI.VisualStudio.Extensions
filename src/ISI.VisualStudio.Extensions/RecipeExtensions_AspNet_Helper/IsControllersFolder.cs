﻿#region Copyright & License
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
 
using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNet_Helper
	{
		public virtual bool IsControllersFolder(Community.VisualStudio.Toolkit.Project project, Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.PhysicalFolder)
			{
				var directory = solutionItem.FullPath.TrimEnd('\\','/');

				if (string.Equals(directory.Split(['\\', '/'], StringSplitOptions.RemoveEmptyEntries).LastOrDefault(), ControllersFolderName, StringComparison.InvariantCultureIgnoreCase))
				{
					var projectRootDirectory = System.IO.Path.GetDirectoryName(project.FullPath);
					var rootDirectory = directory.TrimEnd(ControllersFolderName, StringComparison.InvariantCultureIgnoreCase);

					if (ISI.Extensions.IO.Path.IsPathEqual(projectRootDirectory, rootDirectory))
					{
						return true;
					}

					projectRootDirectory = string.Format("{0}\\", System.IO.Path.Combine(projectRootDirectory, AreasFolderName));

					return ISI.Extensions.IO.Path.IsPathEqual(projectRootDirectory, ISI.Extensions.IO.Path.GetCommonPath([rootDirectory, projectRootDirectory]));
				}
			}

			return false;
		}
	}
}
