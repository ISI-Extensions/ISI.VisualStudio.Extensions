#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using System.Collections.Generic;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.XmlConfigurationExtensions_Execute_MenuItemId)]
	public class XmlConfigurationExtensions_ExecuteTransform_Command : BaseDynamicCommand<XmlConfigurationExtensions_ExecuteTransform_Command, string>
	{
		private static XmlConfigurationExtensions_Helper _xmlConfigurationExtensionsHelper = null;
		protected XmlConfigurationExtensions_Helper XmlConfigurationExtensionsHelper => _xmlConfigurationExtensionsHelper ??= Package.GetServiceProvider().GetService<XmlConfigurationExtensions_Helper>();

		protected override IReadOnlyList<string> GetItems()
		{
			var solutionExplorer = VS.Windows.GetSolutionExplorerWindowAsync().GetAwaiter().GetResult();
			if (solutionExplorer != null)
			{
				var solutionItems = solutionExplorer.GetSelectionAsync().GetAwaiter().GetResult();

				if (solutionItems.NullCheckedCount() == 1)
				{
					var solutionItem = solutionItems.NullCheckedFirstOrDefault();

					if (solutionItem?.Type == SolutionItemType.PhysicalFile)
					{
						if (string.Equals(System.IO.Path.GetExtension(solutionItem.FullPath), ".config", StringComparison.InvariantCultureIgnoreCase))
						{
							if (solutionItem.Children.NullCheckedAny())
							{
								var baseFileName = System.IO.Path.GetFileNameWithoutExtension(solutionItem.FullPath);

								return solutionItem.Children.ToNullCheckedHashSet(item => System.IO.Path.GetFileNameWithoutExtension(item.FullPath).Substring(baseFileName.Length + 1)).ToArray();
							}
						}
					}
				}
			}

			return null;
		}

		protected override void BeforeQueryStatus(OleMenuCommand menuItem, EventArgs eventArgs, string item)
		{
			menuItem.Text = item;
			menuItem.Visible = true;
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs, string target)
		{
			var solutionExplorer = await VS.Windows.GetSolutionExplorerWindowAsync();
			if (solutionExplorer != null)
			{
				var solutionItems = await solutionExplorer.GetSelectionAsync();

				if (solutionItems.NullCheckedCount() == 1)
				{
					var solutionItem = solutionItems.NullCheckedFirstOrDefault();

					if (solutionItem != null)
					{
						//await CakeExtensionsHelper.ExecuteBuildTargetAsync(solutionItem, target);
					}
				}
			}
		}
	}
}
