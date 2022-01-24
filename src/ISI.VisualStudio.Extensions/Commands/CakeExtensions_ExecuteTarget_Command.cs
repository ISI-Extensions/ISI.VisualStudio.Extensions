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
	[Command(PackageIds.CakeExtensionsExecuteDefaultTargetMenuItemId)]
	internal sealed class CakeExtensions_ExecuteTarget_Command : BaseDynamicCommand<CakeExtensions_ExecuteTarget_Command, string>
	{
		private OutputWindowPane _outputWindowPane = null;

		private OleMenuCommand[] _executeTargetSubMenus = null;

		private ISI.Extensions.Cake.CakeApi CakeApi { get; set; }

		protected override async Task InitializeCompletedAsync()
		{
			CakeApi = (Package as Package)?.PackageServiceProvider.ServiceProvider.GetService<ISI.Extensions.Cake.CakeApi>();

			var serviceAsync = (System.ComponentModel.Design.IMenuCommandService)await Package.GetServiceAsync(typeof(System.ComponentModel.Design.IMenuCommandService));

			if (serviceAsync != null)
			{
				_executeTargetSubMenus ??= new OleMenuCommand[20];

				for (var index = 1; index <= _executeTargetSubMenus.Length; index++)
				{
					var menuCommandId = new System.ComponentModel.Design.CommandID(PackageGuids.PackageUuid, PackageIds.CakeExtensionsExecuteTargetSubMenuId + index);
					var menuCommand = new OleMenuCommand((object sender, EventArgs eventArgs) => Package.JoinableTaskFactory.RunAsync((async () =>
					{
						try
						{
							await this.ExecuteTargetAsync(sender, (OleMenuCmdEventArgs)eventArgs);
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

		protected override System.Collections.Generic.IReadOnlyList<string> GetItems()
		{
			throw new NotImplementedException();
		}

		protected override void BeforeQueryStatus(OleMenuCommand menuItem, EventArgs e, string item)
		{
			throw new NotImplementedException();
		}

		protected override void BeforeQueryStatus(System.EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionExplorer = Community.VisualStudio.Toolkit.VS.Windows.GetSolutionExplorerWindowAsync().GetAwaiter().GetResult();
			if (solutionExplorer != null)
			{
				var solutionItems = solutionExplorer.GetSelectionAsync().GetAwaiter().GetResult();

				if (solutionItems.NullCheckedCount() == 1)
				{
					var solutionItem = solutionItems.NullCheckedFirstOrDefault();

					if (solutionItem != null)
					{
						var activeTargetKeys = CakeApi.GetTargetKeysFromBuildScript(new ISI.Extensions.Cake.DataTransferObjects.CakeApi.GetTargetKeysFromBuildScriptRequest()
						{
							BuildScriptFullName = solutionItem.FullPath,
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
				var solutionItems = await solutionExplorer.GetSelectionAsync();

				if (solutionItems.NullCheckedCount() == 1)
				{
					var solutionItem = solutionItems.NullCheckedFirstOrDefault();

					if (solutionItem != null)
					{
						_outputWindowPane ??= await Community.VisualStudio.Toolkit.VS.Windows.CreateOutputWindowPaneAsync("ISI.VisualStudio.Extensions.Cake");

						await _outputWindowPane.ActivateAsync();

						await System.Threading.Tasks.Task.Run(() =>
						{
							CakeApi.ExecuteBuildTarget(new ISI.Extensions.Cake.DataTransferObjects.CakeApi.ExecuteBuildTargetRequest()
							{
								BuildScriptFullName = solutionItem.FullPath,
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