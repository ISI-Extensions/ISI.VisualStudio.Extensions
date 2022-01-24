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
	[Community.VisualStudio.Toolkit.Command(PackageIds.XmlConfigurationExtensionsExecuteMenuItemId)]
	internal sealed class XmlConfigurationExtensions_ExecuteTransform_Command : Community.VisualStudio.Toolkit.BaseCommand<XmlConfigurationExtensions_ExecuteTransform_Command>
	{
		private Community.VisualStudio.Toolkit.OutputWindowPane _outputWindowPane = null;

		private Microsoft.VisualStudio.Shell.OleMenuCommand[] _configurationSubMenus = null;

		private ISI.Extensions.VisualStudio.XmlTransformApi XmlTransformApi { get; set; }

		protected override async System.Threading.Tasks.Task InitializeCompletedAsync()
		{
			_outputWindowPane ??= await Community.VisualStudio.Toolkit.VS.Windows.CreateOutputWindowPaneAsync("XmlTransformConfig");

			XmlTransformApi = (Package as Package)?.PackageServiceProvider.ServiceProvider.GetService<ISI.Extensions.VisualStudio.XmlTransformApi>();

			var serviceAsync = (System.ComponentModel.Design.IMenuCommandService)await Package.GetServiceAsync(typeof(System.ComponentModel.Design.IMenuCommandService));

			if (serviceAsync != null)
			{
				_configurationSubMenus ??= new Microsoft.VisualStudio.Shell.OleMenuCommand[20];

				for (var index = 1; index <= _configurationSubMenus.Length; index++)
				{
					var menuCommandId = new System.ComponentModel.Design.CommandID(PackageGuids.PackageUuid, PackageIds.XmlConfigurationExtensionsExecuteMenuItemId + index);
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
					_configurationSubMenus[index - 1] = menuCommand;
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

								var configurations = solutionItem.Children.ToNullCheckedHashSet(item => System.IO.Path.GetFileNameWithoutExtension(item.FullPath).Substring(baseFileName.Length + 1)).ToArray();

								for (var index = 1; index <= _configurationSubMenus.Length; index++)
								{
									var menuCommand = _configurationSubMenus[index - 1];

									if (index <= configurations.Length)
									{
										menuCommand.Text = configurations[index - 1];
										menuCommand.Visible = true;
									}
									else
									{
										menuCommand.Visible = false;
									}
								}

								showCommand = configurations.NullCheckedAny();
							}
						}
					}
				}
			}

			if (!showCommand)
			{
				for (var index = 1; index <= _configurationSubMenus.Length; index++)
				{
					_configurationSubMenus[index - 1].Visible = false;
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
						await _outputWindowPane.ActivateAsync();

						await System.Threading.Tasks.Task.Run(() =>
						{
							//XmlTransformApi
							//CakeApi.ExecuteBuildTarget(new ISI.Extensions.Cake.DataTransferObjects.CakeApi.ExecuteBuildTargetRequest()
							//{
							//	BuildScriptFullName = solutionItem.FullPath,
							//	Target = target,
							//	AddToLog = description => _outputWindowPane.WriteLine(description.TrimEnd('\r', '\n')),
							//});
						});
					}
				}
			}
		}
	}
}