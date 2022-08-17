using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.SolutionExtensions_ShowSccInformation_MenuItemId)]
	public class SolutionExtensions_ShowSccInformation_Command : BaseCommand<SolutionExtensions_ShowSccInformation_Command>
	{
		private static SolutionExtensions_Helper _solutionExtensionsHelper = null;
		protected SolutionExtensions_Helper SolutionExtensionsHelper => _solutionExtensionsHelper ??= Package.GetServiceProvider().GetService<SolutionExtensions_Helper>();

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			var outputWindowPane = await SolutionExtensionsHelper.GetOutputWindowPaneAsync();

			await outputWindowPane.ActivateAsync();

			await outputWindowPane.ClearAsync();

			var solution = await VS.Solutions.GetCurrentSolutionAsync();

			var sourceControlProvider = SolutionExtensionsHelper.GetSourceControlProvider(solution);

			await outputWindowPane.WriteLineAsync(string.Format("SourceControlProviderType {0}", sourceControlProvider?.GetType()?.FullName));

			var sourceControlProviderUuid = Guid.Empty;

			if ((Package as Package).VsGetScciProviderInterface != null)
			{
				(Package as Package).VsGetScciProviderInterface.GetSourceControlProviderID(out sourceControlProviderUuid);

				await outputWindowPane.WriteLineAsync(string.Format("SourceControlProviderUuid {0}", sourceControlProviderUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens)));
			}
			else
			{
				await outputWindowPane.WriteLineAsync("SourceControlProviderUuid is null");
			}

			if ((sourceControlProvider != null) && (sourceControlProviderUuid != sourceControlProvider.SourceControlProviderUuid))
			{
				var sourceControlProviderPackageUuid = Guid.Empty;

				foreach (var controlProviderPackageUuid in sourceControlProvider.SourceControlProviderPackageUuids)
				{
					if (sourceControlProviderPackageUuid == Guid.Empty)
					{
						var guidPackage = controlProviderPackageUuid;
						var hr = (Package as Package).VsShell.IsPackageInstalled(ref guidPackage, out var installed);
						System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);

						if (installed == 1)
						{
							sourceControlProviderPackageUuid = controlProviderPackageUuid;
						}
					}
				}

				if (sourceControlProviderPackageUuid != Guid.Empty)
				{
					var hr = (Package as Package).VsRegisterScciProvider.RegisterSourceControlProvider(sourceControlProviderPackageUuid);
					System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);

					await outputWindowPane.WriteLineAsync(string.Format("SourceControlProviderType Registered {0}", sourceControlProvider?.GetType()?.FullName));
				}
			}
		}
	}
}