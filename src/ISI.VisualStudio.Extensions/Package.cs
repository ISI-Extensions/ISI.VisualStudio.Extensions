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
	//[ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
	[ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionOpening_string, PackageAutoLoadFlags.BackgroundLoad)]
	[ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
	[ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionHasMultipleProjects_string, PackageAutoLoadFlags.BackgroundLoad)]
	[ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionHasSingleProject_string, PackageAutoLoadFlags.BackgroundLoad)]
	public sealed class Package : ToolkitPackage
	{
		internal PackageServiceProvider PackageServiceProvider { get; } = new();

		internal Community.VisualStudio.Toolkit.OutputWindowPane OutputWindowPane = null;

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
	
			PackageServiceProvider.Initialize();

			OutputWindowPane ??= await Community.VisualStudio.Toolkit.VS.Windows.CreateOutputWindowPaneAsync("ISI.VisualStudio.Extensions");
	
			await InsertNewGuidCommand.InitializeAsync(this);
	
			await ClipboardExtensionsPasteAsCommand.InitializeAsync(this);

			await CakeExecuteDefaultTargetCommand.InitializeAsync(this);

			await OutputWindowPane.ActivateAsync();
			await OutputWindowPane.WriteLineAsync("ISI.VisualStudio.Extensions Loaded");
		}
	}
}