using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.NugetExtensionsCopyReferencesAsNugetPackagesMenuItemId)]
	public class NugetExtensions_CopyReferencesAsNugetPackages_Command : BaseCommand<NugetExtensions_CopyReferencesAsNugetPackages_Command>
	{
		private static NugetExtensions_Helper _nugetExtensionsHelper = null;
		protected NugetExtensions_Helper NugetExtensionsHelper => _nugetExtensionsHelper ??= Package.GetServiceProvider().GetService<NugetExtensions_Helper>();

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var project = await VS.Solutions.GetActiveProjectAsync();

			await project?.SaveAsync();

			var solutionItems = await VS.Solutions.GetActiveItemsAsync();

			var packageNames = solutionItems
				.ToNullCheckedArray(solutionItem => solutionItem.Text, NullCheckCollectionResult.Empty)
				.ToHashSet(StringComparer.InvariantCultureIgnoreCase);

			var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary(NugetExtensionsHelper.GetNugetPackageKeysFromProject(project).Where(nugetPackageKey => packageNames.Contains(nugetPackageKey.Package)));

			if (nugetPackageKeys.Any())
			{
				System.Windows.Forms.Clipboard.SetText(string.Join(Environment.NewLine, nugetPackageKeys.Select(nugetPackageKey => nugetPackageKey.GetClipboardToken())));
			}
		}
	}
}