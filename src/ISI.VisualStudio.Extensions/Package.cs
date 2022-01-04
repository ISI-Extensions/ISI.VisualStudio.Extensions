using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;

namespace ISI.VisualStudio.Extensions
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[Guid(PackageGuids.PackageUuidString)]
	public sealed class Package : ToolkitPackage
	{
		internal PackageServiceProvider PackageServiceProvider { get; } = new();

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
	
			PackageServiceProvider.Initialize();

			//AddService(typeof(ISI.Extensions.Cake.CakeApi), PackageServiceProvider.GetServiceAsync);
	
			await InsertNewGuidCommand.InitializeAsync(this);
	
			await CakeExecuteDefaultTargetCommand.InitializeAsync(this);
		}
	}
}