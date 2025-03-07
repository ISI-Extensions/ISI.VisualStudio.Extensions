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
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class Extensions_Helper
	{
		public string GetNamespace(Community.VisualStudio.Toolkit.Project project, Community.VisualStudio.Toolkit.SolutionItem solutionItem, string className = null)
		{
			var directory = (solutionItem.Type == Community.VisualStudio.Toolkit.SolutionItemType.Project ? System.IO.Path.GetDirectoryName(solutionItem.FullPath) : solutionItem.FullPath);

			return GetNamespace(project, directory, className);
		}

		public string GetNamespace(Community.VisualStudio.Toolkit.Project project, string directory, string className = null)
		{
			directory = string.Format("{0}{1}", directory.TrimEnd(System.IO.Path.DirectorySeparatorChar), System.IO.Path.DirectorySeparatorChar);

			var @namespace = project.GetRootNamespace();

			var projectDirectory = System.IO.Path.GetDirectoryName(project.FullPath);

			var path = System.IO.Path.GetDirectoryName(directory).TrimStart(projectDirectory).Trim('\\', '/');

			if (!string.IsNullOrWhiteSpace(path))
			{
				var pathParts = new List<string>(path.Split(['\\', '/'], StringSplitOptions.RemoveEmptyEntries));

				if (pathParts.NullCheckedAny())
				{
					if (string.Equals(pathParts.Last(), className, StringComparison.InvariantCulture))
					{
						pathParts.RemoveAt(pathParts.Count - 1);
					}

					if (pathParts.NullCheckedAny())
					{
						@namespace = string.Format("{0}.{1}", @namespace, string.Join(".", pathParts));
					}
				}
			}

			return @namespace;
		}
	}
}
