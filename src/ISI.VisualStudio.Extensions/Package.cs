﻿using Community.VisualStudio.Toolkit;
using Community.VisualStudio.Toolkit.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	[Guid(PackageGuids.PackageUuidString)]
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
	[ProvideMenuResource("Menus.ctmenu", 1)]

	[ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionOpening_string, PackageAutoLoadFlags.BackgroundLoad)]
	[ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
	[ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionHasMultipleProjects_string, PackageAutoLoadFlags.BackgroundLoad)]
	[ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionHasSingleProject_string, PackageAutoLoadFlags.BackgroundLoad)]

	[ProvideOptionPage(typeof(OptionsProvider.RecipeOptionsPage), Vsix.Name, "Recipes", 0, 0, true)]
	[ProvideProfile(typeof(OptionsProvider.RecipeOptionsPage), Vsix.Name, "Recipes", 0, 0, true)]
	[ProvideOptionPage(typeof(OptionsProvider.EditorOptionsPage), Vsix.Name, "Editor", 0, 0, true)]
	[ProvideProfile(typeof(OptionsProvider.EditorOptionsPage), Vsix.Name, "Editor", 0, 0, true)]
	public sealed class Package : ToolkitPackage
	{
		public EnvDTE80.DTE2 DTE2 { get; private set; } = null!;
		public IServiceProvider ServiceProvider { get; private set; } = null!;

		public Microsoft.VisualStudio.Shell.Interop.IVsShell VsShell { get; private set; } = null!;
		public Microsoft.VisualStudio.Shell.Interop.IVsGetScciProviderInterface VsGetScciProviderInterface { get; private set; } = null!;
		public Microsoft.VisualStudio.Shell.Interop.IVsRegisterScciProvider VsRegisterScciProvider { get; private set; } = null!;

		private OutputWindowPane OutputWindowPane = null;

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await base.InitializeAsync(cancellationToken, progress);

			await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			var configurationRoot = configurationBuilder.Build();

			var services = new ServiceCollection();
			services
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configurationRoot);

			services.AddAllConfigurations(configurationRoot)

				//.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory>()
				.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.LoggerFactory>()
				//.AddLogging(builder => builder
				//		//.AddConsole()
				//	//.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information)
				//)
				.AddSingleton<Microsoft.Extensions.Logging.ILogger>(_ => new ISI.Extensions.NullLogger())

				.AddSingleton<ISI.Extensions.DateTimeStamper.IDateTimeStamper, ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper>()

				.AddSingleton<ISI.Extensions.JsonSerialization.IJsonSerializer, ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer>()
				.AddSingleton<ISI.Extensions.Serialization.ISerialization, ISI.Extensions.Serialization.Serialization>()

				.AddSingleton<SolutionExtensions_Helper>()
				.AddSingleton<ProjectExtensions_Helper>()
				.AddSingleton<Extensions_Helper>()
				.AddSingleton<CakeExtensions_Helper>()
				.AddSingleton<JenkinsExtensions_Helper>()
				.AddSingleton<NugetExtensions_Helper>()
				.AddSingleton<RecipeExtensions_AspNet_Helper>()
				.AddSingleton<RecipeExtensions_AspNetMvc_5x_Helper>()
				.AddSingleton<RecipeExtensions_AspNetMvc_6x_Helper>()
				.AddSingleton<RecipeExtensions_Project_Helper>()
				.AddSingleton<RecipeExtensions_ProjectPartialClass_Helper>()
				.AddSingleton<XmlConfigurationExtensions_Helper>()

				.AddConfigurationRegistrations(configurationRoot)
				.ProcessServiceRegistrars()
				;

			ServiceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configurationRoot);

			ServiceProvider.SetServiceLocator();

			OutputWindowPane ??= await VS.Windows.CreateOutputWindowPaneAsync(Vsix.Name);

			DTE2 = await GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
			Microsoft.Assumes.Present(DTE2);

			await this.RegisterCommandsAsync();

			await OutputWindowPane.ActivateAsync();
			await OutputWindowPane.WriteLineAsync(string.Format("{0} Loaded", Vsix.Name));

			VsShell = await GetServiceAsync(typeof(Microsoft.VisualStudio.Shell.Interop.SVsShell)) as Microsoft.VisualStudio.Shell.Interop.IVsShell;
			VsRegisterScciProvider = await GetServiceAsync(typeof(Microsoft.VisualStudio.Shell.Interop.IVsRegisterScciProvider)) as Microsoft.VisualStudio.Shell.Interop.IVsRegisterScciProvider;
			VsGetScciProviderInterface = await GetServiceAsync(typeof(Microsoft.VisualStudio.Shell.Interop.IVsRegisterScciProvider)) as Microsoft.VisualStudio.Shell.Interop.IVsGetScciProviderInterface;
		}
	}
}