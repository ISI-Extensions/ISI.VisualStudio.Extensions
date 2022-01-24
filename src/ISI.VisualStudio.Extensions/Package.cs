using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

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
	[ProvideOptionPage(typeof(OptionsProvider.RecipeExtensionsOptionsPage), "ISI.VisualStudio.Extensions", "RecipeExtensions_Options", 0, 0, true)]
	[ProvideProfile(typeof(OptionsProvider.RecipeExtensionsOptionsPage), "ISI.VisualStudio.Extensions", "RecipeExtensions_Options", 0, 0, true)]
	public sealed class Package : ToolkitPackage
	{
		internal PackageServiceProvider PackageServiceProvider { get; } = new();

		internal Community.VisualStudio.Toolkit.OutputWindowPane OutputWindowPane = null;

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

			OutputWindowPane ??= await Community.VisualStudio.Toolkit.VS.Windows.CreateOutputWindowPaneAsync(Vsix.Name);

			PackageServiceProvider.Initialize();

			await AboutExtensions_About_Command.InitializeAsync(this);

			await XmlConfigurationExtensions_AddTransform_Command.InitializeAsync(this);
			await XmlConfigurationExtensions_ExecuteTransform_Command.InitializeAsync(this);

			await RecipeExtensions_AspNetMvc_5x_AddArea_Command.InitializeAsync(this);

			await GuidExtensions_InsertNewGuid_Command.InitializeAsync(this);

			await ClipboardExtensions_PasteAs_Command.InitializeAsync(this);

			await CakeExtensions_ExecuteTarget_Command.InitializeAsync(this);

			await OutputWindowPane.ActivateAsync();
			await OutputWindowPane.WriteLineAsync(string.Format("{0} Loaded", Vsix.Name));
		}
	}
}