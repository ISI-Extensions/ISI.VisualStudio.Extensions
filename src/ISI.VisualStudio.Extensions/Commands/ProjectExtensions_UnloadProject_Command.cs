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
	[Command(PackageIds.ProjectExtensions_ProjectUnloadProject_MenuItemId)]
	public class ProjectExtensions_UnloadProject_Command : BaseCommand<ProjectExtensions_UnloadProject_Command>
	{
		private static ProjectExtensions_Helper _projectExtensionsHelper = null;
		protected ProjectExtensions_Helper ProjectExtensionsHelper => _projectExtensionsHelper ??= Package.GetServiceProvider().GetService<ProjectExtensions_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var project = VS.Solutions.GetActiveProjectAsync().GetAwaiter().GetResult();

			if (project != null)
			{
				showCommand = project.IsLoaded;
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			var outputWindowPane = await ProjectExtensionsHelper.GetOutputWindowPaneAsync();

			await outputWindowPane.ActivateAsync();

			await outputWindowPane.ClearAsync();

			var project = await VS.Solutions.GetActiveProjectAsync();
			if (project != null)
			{
				await project.UnloadAsync();

				await VS.Documents.OpenAsync(project.FullPath);

				await outputWindowPane.WriteLineAsync($"Project '{project.Name}' has been unloaded.");
			}
		}
	}
}
