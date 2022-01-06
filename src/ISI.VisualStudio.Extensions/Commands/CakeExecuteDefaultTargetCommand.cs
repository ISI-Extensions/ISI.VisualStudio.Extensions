using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.VisualStudio.Extensions
{
	[Community.VisualStudio.Toolkit.Command(PackageIds.CakeExtensionsExecuteDefaultTargetMenuItemId)]
	internal sealed class CakeExecuteDefaultTargetCommand : Community.VisualStudio.Toolkit.BaseCommand<CakeExecuteDefaultTargetCommand>
	{
		private Community.VisualStudio.Toolkit.OutputWindowPane _outputWindowPane = null;

		private Microsoft.VisualStudio.Shell.OleMenuCommand[] _executeTargetSubMenus = null;

		private ISI.Extensions.Cake.CakeApi CakeApi { get; set; }

		protected override async System.Threading.Tasks.Task InitializeCompletedAsync()
		{
			CakeApi = (Package as Package)?.PackageServiceProvider.ServiceProvider.GetService<ISI.Extensions.Cake.CakeApi>();

			var serviceAsync = (System.ComponentModel.Design.IMenuCommandService)await Package.GetServiceAsync(typeof(System.ComponentModel.Design.IMenuCommandService));

			if (serviceAsync != null)
			{
				_executeTargetSubMenus ??= new Microsoft.VisualStudio.Shell.OleMenuCommand[20];

				for (var index = 1; index <= _executeTargetSubMenus.Length; index++)
				{
					var menuCommandId = new System.ComponentModel.Design.CommandID(PackageGuids.PackageUuid, PackageIds.CakeExtensionsExecuteTargetSubMenuId + index);
					var menuCommand = new Microsoft.VisualStudio.Shell.OleMenuCommand((object sender, EventArgs eventArgs) => Package.JoinableTaskFactory.RunAsync((async () =>
					{
						try
						{
							await this.ExecuteTargetAsync(sender, (Microsoft.VisualStudio.Shell.OleMenuCmdEventArgs)eventArgs);
						}
						catch (System.Exception ex)
						{
							await ex.LogAsync();
						}
					})).FireAndForget(), menuCommandId);

					serviceAsync.AddCommand(menuCommand);
					_executeTargetSubMenus[index - 1] = menuCommand;
				}
			}

			await base.InitializeCompletedAsync();
		}

		protected override void BeforeQueryStatus(System.EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionExplorer = Community.VisualStudio.Toolkit.VS.Windows.GetSolutionExplorerWindowAsync().GetAwaiter().GetResult();
			if (solutionExplorer != null)
			{
				var selectedItems = solutionExplorer.GetSelectionAsync().GetAwaiter().GetResult();

				if (selectedItems.NullCheckedCount() == 1)
				{
					var selectedItem = selectedItems.NullCheckedFirstOrDefault();

					if (selectedItem != null)
					{
						var activeTargetKeys = CakeApi.GetTargetKeysFromBuildScript(new ISI.Extensions.Cake.DataTransferObjects.CakeApi.GetTargetKeysFromBuildScriptRequest()
						{
							BuildScriptFullName = selectedItem.FullPath,
						}).Targets ?? Array.Empty<string>();

						for (var index = 1; index <= _executeTargetSubMenus.Length; index++)
						{
							var menuCommand = _executeTargetSubMenus[index - 1];

							if (index <= activeTargetKeys.Length)
							{
								menuCommand.Text = activeTargetKeys[index - 1];
								menuCommand.Visible = true;
							}
							else
							{
								menuCommand.Visible = false;
							}
						}

						showCommand = activeTargetKeys.NullCheckedAny();
					}
				}
				else
				{
					for (var index = 1; index <= _executeTargetSubMenus.Length; index++)
					{
						_executeTargetSubMenus[index - 1].Visible = false;
					}
				}
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async System.Threading.Tasks.Task ExecuteAsync(Microsoft.VisualStudio.Shell.OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			await CakeTargetAsync(oleMenuCmdEventArgs);
		}

		private async System.Threading.Tasks.Task ExecuteTargetAsync(object sender, Microsoft.VisualStudio.Shell.OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			if (sender is OleMenuCommand menuCommand)
			{
				await CakeTargetAsync(oleMenuCmdEventArgs, menuCommand.Text);
			}
		}

		private async System.Threading.Tasks.Task CakeTargetAsync(Microsoft.VisualStudio.Shell.OleMenuCmdEventArgs oleMenuCmdEventArgs, string target = "Default")
		{
			var solutionExplorer = await Community.VisualStudio.Toolkit.VS.Windows.GetSolutionExplorerWindowAsync();
			if (solutionExplorer != null)
			{
				var selectedItems = await solutionExplorer.GetSelectionAsync();

				if (selectedItems.NullCheckedCount() == 1)
				{
					var selectedItem = selectedItems.NullCheckedFirstOrDefault();

					if (selectedItem != null)
					{
						_outputWindowPane ??= await Community.VisualStudio.Toolkit.VS.Windows.CreateOutputWindowPaneAsync("Cake");

						await _outputWindowPane.ActivateAsync();

						await System.Threading.Tasks.Task.Run(() =>
						{
							CakeApi.ExecuteBuildTarget(new ISI.Extensions.Cake.DataTransferObjects.CakeApi.ExecuteBuildTargetRequest()
							{
								BuildScriptFullName = selectedItem.FullPath,
								Target = target,
								AddToLog = description => _outputWindowPane.WriteLine(description.TrimEnd('\r', '\n')),
							});
						});
					}
				}
			}
		}
	}
}