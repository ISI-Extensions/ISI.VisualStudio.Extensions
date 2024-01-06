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
	[Command(PackageIds.NugetExtensions_CopyReferencesAsNugetPackages_MenuItemId)]
	public class NugetExtensions_CopyReferencesAsNugetPackages_Command : BaseCommand<NugetExtensions_CopyReferencesAsNugetPackages_Command>
	{
		private static NugetExtensions_Helper _nugetExtensionsHelper = null;
		protected NugetExtensions_Helper NugetExtensionsHelper => _nugetExtensionsHelper ??= Package.GetServiceProvider().GetService<NugetExtensions_Helper>();
		
		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var project = VS.Solutions.GetActiveProjectAsync().GetAwaiter();

			var solutionItems =  VS.Solutions.GetActiveItemsAsync().GetAwaiter().GetResult();

			if (solutionItems.NullCheckedAny())
			{
				var packageNames = solutionItems
					.ToNullCheckedArray(solutionItem => solutionItem.Text, NullCheckCollectionResult.Empty)
					.ToHashSet(StringComparer.InvariantCultureIgnoreCase);

				var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary(NugetExtensionsHelper.GetNugetPackageKeysFromProject(project.GetResult()).Where(nugetPackageKey => packageNames.Contains(nugetPackageKey.Package)));

				showCommand = nugetPackageKeys.NullCheckedAny();
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

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