﻿using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.JenkinsExtensions_PushJenkinsConfigToJenkins_MenuItemId)]
	public class JenkinsExtensions_PushJenkinsConfigToJenkins_Command : BaseCommand<JenkinsExtensions_PushJenkinsConfigToJenkins_Command>
	{
		private static JenkinsExtensions_Helper _jenkinsExtensionsHelper = null;
		protected JenkinsExtensions_Helper JenkinsExtensionsHelper => _jenkinsExtensionsHelper ??= Package.GetServiceProvider().GetService<JenkinsExtensions_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionItems = VS.Solutions.GetActiveItemsAsync().GetAwaiter().GetResult();

			Command.Visible = solutionItems.NullCheckedAll(solutionItem => JenkinsExtensionsHelper.IsJenkinsConfigFile(solutionItem));

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var solutionItems = await VS.Solutions.GetActiveItemsAsync();

			using (var form = new ISI.Extensions.Jenkins.Forms.PushJenkinsConfigToJenkinsForm(solutionItems.Select(solutionItem => solutionItem.FullPath)))
			{
				form.ShowDialog();
			}
		}
	}
}