using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensions_Helper
	{
		public System.Collections.Generic.IEnumerable<ISI.Extensions.Nuget.NugetPackageKey> GetNugetPackageKeysFromProject(Community.VisualStudio.Toolkit.Project project)
		{
			var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();

			foreach (var nugetPackageKey in NugetApi.ExtractProjectNugetPackageDependenciesFromCsProj(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.ExtractProjectNugetPackageDependenciesFromCsProjRequest()
			         {
								 CsProjFullName = project.FullPath,
			         }).NugetPackageKeys)
			{
				nugetPackageKeys.TryAdd(nugetPackageKey);
			}

			var packagesConfigFullName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(project.FullPath), "packages.config");
			if (System.IO.File.Exists(packagesConfigFullName))
			{
				foreach (var nugetPackageKey in NugetApi.ExtractProjectNugetPackageDependenciesFromPackagesConfig(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.ExtractProjectNugetPackageDependenciesFromPackagesConfigRequest()
				         {
					         PackagesConfigFullName = packagesConfigFullName,
				         }).NugetPackageKeys)
				{
					nugetPackageKeys.TryAdd(nugetPackageKey);
				}
			}

			return nugetPackageKeys;
		}
	}
}
