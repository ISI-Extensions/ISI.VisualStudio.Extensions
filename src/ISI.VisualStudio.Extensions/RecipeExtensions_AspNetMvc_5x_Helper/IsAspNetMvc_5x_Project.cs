using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNetMvc_5x_Helper
	{
		public bool IsAspNetMvc_5x_Project(Community.VisualStudio.Toolkit.Project project)
		{
			if (project == null)
			{
				return false;
			}

			var package = "ISI.Libraries.Web.Mvc";

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
