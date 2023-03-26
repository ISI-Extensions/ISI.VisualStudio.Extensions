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
 
using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensions_Helper
	{
		public delegate void InstallPackagesProgress(string package, int index, int count);

		public void InstallPackages(Microsoft.VisualStudio.Shell.AsyncPackage package, ISI.Extensions.Nuget.NugetPackageKey[] nugetPackageKeys, InstallPackagesProgress progress)
		{
			var componentModel = Package.GetGlobalService(typeof(Microsoft.VisualStudio.ComponentModelHost.SComponentModel)) as Microsoft.VisualStudio.ComponentModelHost.IComponentModel;
			var installer = componentModel.GetService<NuGet.VisualStudio.IVsPackageInstaller>();

			var selectedProject = package.GetDTE2().GetSelectedProject();

			var nugetPackageKeyCount = nugetPackageKeys.Length;
			for (var nugetPackageKeyIndex = 1; nugetPackageKeyIndex <= nugetPackageKeyCount; nugetPackageKeyIndex++)
			{
				var nugetPackageKey = nugetPackageKeys[nugetPackageKeyIndex - 1];

				progress?.Invoke(nugetPackageKey.Package, nugetPackageKeyIndex, nugetPackageKeyCount);

				try
				{
					if (string.IsNullOrEmpty(nugetPackageKey.Version))
					{
						installer.InstallPackage(null, selectedProject, nugetPackageKey.Package, (Version)null, false);
					}
					else
					{
						installer.InstallPackage(null, selectedProject, nugetPackageKey.Package, nugetPackageKey.Version, false);
					}
				}
				catch (Exception exception)
				{
					GetOutputWindowPaneAsync().GetAwaiter().GetResult().WriteLine(exception.ErrorMessageFormatted());

					throw;
				}
			}
		}
	}
}
