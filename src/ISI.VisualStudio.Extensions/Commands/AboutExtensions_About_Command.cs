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
	[Command(PackageIds.AboutMenuItemId)]
	public class AboutExtensions_About_Command : BaseCommand<AboutExtensions_About_Command>
	{
		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var vsVersion = await VS.Shell.GetVsVersionAsync();

			var version = ISI.Extensions.SystemInformation.GetAssemblyVersion(this.GetType().Assembly);

			var message = string.Format("{0}, Version: {1}", Vsix.Name, version);
			message += string.Format("\nVisualStudio, Version: {0}", vsVersion);

			await VS.MessageBox.ShowAsync(Vsix.Name, message, Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_INFO, Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_OK);
		}
	}
}
