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
