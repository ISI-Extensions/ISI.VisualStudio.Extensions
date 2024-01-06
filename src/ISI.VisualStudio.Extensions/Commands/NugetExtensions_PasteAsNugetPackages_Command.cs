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
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.NugetExtensions_PasteAsNugetPackages_MenuItemId)]
	public class NugetExtensions_PasteAsNugetPackages_Command : BaseCommand<NugetExtensions_PasteAsNugetPackages_Command>
	{
		private static ProjectExtensions_Helper _projectExtensionsHelper = null;
		protected ProjectExtensions_Helper ProjectExtensionsHelper => _projectExtensionsHelper ??= Package.GetServiceProvider().GetService<ProjectExtensions_Helper>();

		private static NugetExtensions_Helper _nugetExtensionsHelper = null;
		protected NugetExtensions_Helper NugetExtensionsHelper => _nugetExtensionsHelper ??= Package.GetServiceProvider().GetService<NugetExtensions_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var nugetPackageKeys = NugetExtensionsHelper.GetPackageKeysFromClipboard();

			if (nugetPackageKeys.NullCheckedAny())
			{
				var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

				showCommand = ProjectExtensionsHelper.IsReferencesFolder(solutionItem);
			}

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
						await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

						var getThreadedWaitDialogResponse = await VS.Services.GetThreadedWaitDialogAsync();

						var threadedWaitDialogFactory = getThreadedWaitDialogResponse as Microsoft.VisualStudio.Shell.Interop.IVsThreadedWaitDialogFactory;

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
							threadedWaitDialog?.EndWaitDialog(out _);
							(threadedWaitDialog as IDisposable)?.Dispose();

							(threadedWaitDialogFactory as IDisposable)?.Dispose();
						}
					}
				}
			}
		}
	}
}