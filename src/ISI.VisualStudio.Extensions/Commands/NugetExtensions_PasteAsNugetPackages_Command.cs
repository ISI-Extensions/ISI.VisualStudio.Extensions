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
	[Command(PackageIds.NugetExtensionsPasteAsNugetPackagesMenuItemId)]
	public class NugetExtensions_PasteAsNugetPackages_Command : BaseCommand<NugetExtensions_PasteAsNugetPackages_Command>
	{
		private static NugetExtensions_Helper _nugetExtensionsHelper = null;
		protected NugetExtensions_Helper NugetExtensionsHelper => _nugetExtensionsHelper ??= Package.GetServiceProvider().GetService<NugetExtensions_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			showCommand = NugetExtensionsHelper.IsReferencesFolder(solutionItem);

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var nugetPackageKeys = NugetExtensionsHelper.GetPackageKeysFromClipboard();

			using (var selectNugetPackagesForm = new ISI.Extensions.Nuget.Forms.SelectNugetPackagesForm(nugetPackageKeys))
			{
				if (selectNugetPackagesForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					nugetPackageKeys = selectNugetPackagesForm.NugetPackages.Where(nugetPackage => nugetPackage.Selected).Select(nugetPackage => nugetPackage.NugetPackageKey).ToArray();

					if (nugetPackageKeys.Any())
					{
						var threadedWaitDialogFactory = await VS.Services.GetThreadedWaitDialogAsync() as Microsoft.VisualStudio.Shell.Interop.IVsThreadedWaitDialogFactory;

						var threadedWaitDialog = threadedWaitDialogFactory.CreateInstance();

						try
						{
							threadedWaitDialog.StartWaitDialog("Install Nuget Packages", "Working on it...", "", null, "", 1, true, true);

							NugetExtensionsHelper.InstallPackages(Package, nugetPackageKeys, (package, index, count) =>
							{
								threadedWaitDialog.UpdateProgress("In Progress", package, "Installing Nuget Packages", index, count, true, out _);
							});
						}
						finally
						{
							(threadedWaitDialog as IDisposable).Dispose();
						}
					}
				}
			}
		}
	}
}