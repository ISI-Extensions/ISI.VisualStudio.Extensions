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

using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public static partial class ProjectExtensions
	{
		public static bool UsesISIExtensionsAspNetCore(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Extensions.AspNetCore");

		public static bool UsesISIExtensionsRepositorySqlServer(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Extensions.Repository.SqlServer");

		public static bool UsesISILibrariesWebMvc(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Libraries.Web.Mvc");

		public static bool UsesISILibrariesJQueryWebMvc(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Libraries.JQuery.Web.Mvc");

		public static bool UsesISICmsWebMvc(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Cms.Web.Mvc");

		public static bool UsesISILibrariesBootstrapWebMvc(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Libraries.Bootstrap.Web.Mvc");

		public static bool UsesISILibrariesRepositorySqlServer(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Libraries.Repository.SqlServer");
		
		public static bool UsesNugetPackage(this Community.VisualStudio.Toolkit.Project project, string packageName)
		{
			if (project != null)
			{
				var referenceNames = project.References.ToNullCheckedHashSet(reference => reference.Name, NullCheckCollectionResult.Empty);

				if (referenceNames.Contains(packageName))
				{
					return true;
				}

				var content = System.IO.File.ReadAllText(project.FullPath);

				if (content.IndexOf(string.Format("\"{0}", packageName)) >= 0)
				{
					return true;
				}

				if (content.IndexOf(string.Format("\\{0}", packageName)) >= 0)
				{
					return true;
				}
			}

			return false;
		}
	}
}
