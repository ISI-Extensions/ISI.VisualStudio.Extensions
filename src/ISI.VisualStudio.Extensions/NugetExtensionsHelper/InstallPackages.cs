using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensionsHelper
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
