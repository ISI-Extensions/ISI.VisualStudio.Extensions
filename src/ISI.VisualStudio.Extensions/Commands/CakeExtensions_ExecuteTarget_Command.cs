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
	[Command(PackageIds.CakeExtensionsExecuteTargetMenuItemId)]
	public class CakeExtensions_ExecuteTarget_Command : BaseDynamicCommand<CakeExtensions_ExecuteTarget_Command, string>
	{
		private static CakeExtensions_Helper _cakeExtensionsHelper = null;
		protected CakeExtensions_Helper CakeExtensionsHelper => _cakeExtensionsHelper ??= Package.GetServiceProvider().GetService<CakeExtensions_Helper>();

		protected override IReadOnlyList<string> GetItems()
		{
			var solutionExplorer = VS.Windows.GetSolutionExplorerWindowAsync().GetAwaiter().GetResult();
			if (solutionExplorer != null)
			{
				var solutionItems = solutionExplorer.GetSelectionAsync().GetAwaiter().GetResult();

				if (solutionItems.NullCheckedCount() == 1)
				{
					var solutionItem = solutionItems.NullCheckedFirstOrDefault();

					if (solutionItem != null)
					{
						return CakeExtensionsHelper.GetTargetKeysFromBuildScript(solutionItem);
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
						await CakeExtensionsHelper.ExecuteBuildTargetAsync(solutionItem, target);
					}
				}
			}
		}
	}
}