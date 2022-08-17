using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Community.VisualStudio.Toolkit.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.CakeExtensions_ExecuteDefaultTarget_MenuItemId)]
	public class CakeExtensions_ExecuteDefaultTarget_Command : BaseCommand<CakeExtensions_ExecuteDefaultTarget_Command>
	{
		private static CakeExtensions_Helper _cakeExtensionsHelper = null;
		protected CakeExtensions_Helper CakeExtensionsHelper => _cakeExtensionsHelper ??= Package.GetServiceProvider().GetService<CakeExtensions_Helper>();

		protected override void BeforeQueryStatus(System.EventArgs eventArgs)
		{
			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			Command.Visible = CakeExtensionsHelper.IsCakeBuildScript(solutionItem);

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
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
						await CakeExtensionsHelper.ExecuteBuildTargetAsync(solutionItem);
					}
				}
			}
		}
	}
}
