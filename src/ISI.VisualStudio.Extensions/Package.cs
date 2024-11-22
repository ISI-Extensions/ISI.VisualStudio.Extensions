#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using Community.VisualStudio.Toolkit;
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

	[ProvideOptionPage(typeof(OptionsProvider.PackageOptionsPage), Vsix.Name, "Package", 0, 0, true)]
	[ProvideProfile(typeof(OptionsProvider.PackageOptionsPage), Vsix.Name, "Package", 0, 0, false)]
	[ProvideOptionPage(typeof(OptionsProvider.RecipeOptionsPage), Vsix.Name, "Recipes", 0, 0, true)]
	[ProvideProfile(typeof(OptionsProvider.RecipeOptionsPage), Vsix.Name, "Recipes", 0, 0, true)]
	[ProvideOptionPage(typeof(OptionsProvider.EditorOptionsPage), Vsix.Name, "Editor", 0, 0, true)]
	[ProvideProfile(typeof(OptionsProvider.EditorOptionsPage), Vsix.Name, "Editor", 0, 0, true)]
	[ProvideOptionPage(typeof(OptionsProvider.CakeOptionsPage), Vsix.Name, "Cake", 0, 0, true)]
	[ProvideProfile(typeof(OptionsProvider.CakeOptionsPage), Vsix.Name, "Cake", 0, 0, true)]
	public sealed class Package : ToolkitPackage
	{
		public EnvDTE80.DTE2 DTE2 { get; private set; } = null!;
		public IServiceProvider ServiceProvider { get; private set; } = null!;

		public Microsoft.VisualStudio.Shell.Interop.IVsShell VsShell { get; private set; } = null!;
		public Microsoft.VisualStudio.Shell.Interop.IVsGetScciProviderInterface VsGetScciProviderInterface { get; private set; } = null!;
		public Microsoft.VisualStudio.Shell.Interop.IVsRegisterScciProvider VsRegisterScciProvider { get; private set; } = null!;

		public LaunchSettings_Helper LaunchSettingsHelper { get; private set; } = null!;

		private OutputWindowPane OutputWindowPane = null;

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			try
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

					.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory>()
					//.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.LoggerFactory>()
					//.AddLogging(builder => builder
					//		//.AddConsole()
					//	//.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information)
					//)
					//.AddSingleton<Microsoft.Extensions.Logging.ILogger>(_ => new ISI.Extensions.NullLogger())
					//.AddSingleton<Microsoft.Extensions.Logging.ILogger, ISI.Extensions.NullLogger>()
					.UseNullLogger()

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
					.ProcessServiceRegistrars(configurationRoot)
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

				LaunchSettingsHelper = ServiceProvider.GetService<LaunchSettings_Helper>();

				//var repository = await GetServiceAsync(typeof(Microsoft.VisualStudio.ExtensionManager.SVsExtensionRepository)) as IVsExtensionRepository;
				//var manager = await GetServiceAsync(typeof(SVsExtensionManager)) as IVsExtensionManager;


				if (PackageOptions.Instance.AutoUpdateRecipes)
				{
					var priorExtensionVersion = PackageOptions.Instance.ExtensionVersion ?? string.Empty;
					var currentExtensionVersion = ISI.Extensions.SystemInformation.GetAssemblyVersion(typeof(OptionsProvider).Assembly);

					if (!string.Equals(priorExtensionVersion, currentExtensionVersion, StringComparison.InvariantCultureIgnoreCase))
					{
						var recipeExtensionsOptions = await RecipeOptions.GetLiveInstanceAsync();

						var recipeOptions = new RecipeOptions();

						foreach (var propertyInfo in typeof(RecipeOptions).GetProperties())
						{
							if (propertyInfo.PropertyType == typeof(string))
							{
								propertyInfo.SetValue(recipeExtensionsOptions, propertyInfo.GetValue(recipeOptions));
							}
						}

						await recipeExtensionsOptions.SaveAsync();

						var packagesOptions = await PackageOptions.GetLiveInstanceAsync();

						packagesOptions.ExtensionVersion = currentExtensionVersion;

						await packagesOptions.SaveAsync();
					}
				}

				VS.Events.SolutionEvents.OnAfterOpenProject += LaunchSettingsHelper.CheckProjectPortReservations;

				var solution = await VS.Solutions.GetCurrentSolutionAsync();

				LaunchSettingsHelper.CheckProjectPortReservations(solution);
			}
			catch (Exception exception)
			{
				var logFullName = ISI.Extensions.IO.Path.GetFileNameDeMasked(@"{ApplicationData}\ISI.VisualStudio.Extensions\log{YYYYMMDD.HHmmssfff}.txt");
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(logFullName));

				System.IO.File.WriteAllText(logFullName, exception.ErrorMessageFormatted());

				throw;
			}
		}
	}
}