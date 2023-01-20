#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using ISI.VisualStudio.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.SolutionExtensions_AddMissingLocalSourcePackages_MenuItemId)]
	public class SolutionExtensions_AddMissingLocalSourcePackages_Command : BaseCommand<SolutionExtensions_AddMissingLocalSourcePackages_Command>
	{
		private static SolutionExtensions_Helper _solutionExtensionsHelper = null;
		protected SolutionExtensions_Helper SolutionExtensionsHelper => _solutionExtensionsHelper ??= Package.GetServiceProvider().GetService<SolutionExtensions_Helper>();

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var dte = Package.GetDTE2();

			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			var getThreadedWaitDialogResponse = await VS.Services.GetThreadedWaitDialogAsync();

			var threadedWaitDialogFactory = getThreadedWaitDialogResponse as Microsoft.VisualStudio.Shell.Interop.IVsThreadedWaitDialogFactory;

			var threadedWaitDialog = threadedWaitDialogFactory?.CreateInstance();

			var solutionItem = await VS.Solutions.GetActiveItemAsync();

			try
			{
				threadedWaitDialog?.StartWaitDialog("Add Missing Local Source Packages", "Working on it...", "", null, "", 1, true, true);

				SolutionExtensionsHelper.AddMissingLocalSourcePackages(dte, solutionItem, (package, index, count) =>
				{
					threadedWaitDialog?.UpdateProgress("In Progress", package, "Adding Missing Local Source Packages", index, count, true, out _);
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