using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNetMvc_6x_Helper
	{
		public bool IsAspNetMvc_6x_Project(Community.VisualStudio.Toolkit.Project project)
		{
			var package = "ISI.Extensions.AspNetCore";

			var referenceNames = project.References.ToNullCheckedHashSet(reference => reference.Name, NullCheckCollectionResult.Empty);

			if (referenceNames.Contains(package))
			{
				return true;
			}

			var packages = new HashSet<string>(NugetApi.ExtractProjectNugetPackageDependenciesFromCsProj(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.ExtractProjectNugetPackageDependenciesFromCsProjRequest()
			{
				CsProjFullName = project.FullPath,
			}).NugetPackageKeys.NullCheckedSelect(nugetPackageKey => nugetPackageKey.Package, NullCheckCollectionResult.Empty), StringComparer.InvariantCultureIgnoreCase);

			return packages.Contains(package);
		}
	}
}
