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
	[Command(PackageIds.XmlConfigurationExtensionsAddTransformMenuItemId)]
	public class XmlConfigurationExtensions_AddTransform_Command : BaseCommand<XmlConfigurationExtensions_AddTransform_Command>
	{
		private static XmlConfigurationExtensionsHelper _xmlConfigurationExtensionsHelper = null;
		protected XmlConfigurationExtensionsHelper XmlConfigurationExtensionsHelper => _xmlConfigurationExtensionsHelper ??= Package.GetServiceProvider().GetService<XmlConfigurationExtensionsHelper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			Command.Visible = XmlConfigurationExtensionsHelper.IsXmlConfiguration(solutionItem);

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var project = await VS.Solutions.GetActiveProjectAsync();

			var parentPhysicalFile = await VS.Solutions.GetActiveItemAsync() as PhysicalFile;

			var inputDialog = new InputDialog("New Configuration");

			var inputDialogResult = await inputDialog.ShowDialogAsync();

			if (inputDialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(inputDialog.Value))
			{
				var configurationKey = inputDialog.Value;

				var configDirectory = System.IO.Path.GetDirectoryName(parentPhysicalFile.FullPath);

				var fullName = System.IO.Path.Combine(configDirectory, string.Format("{0}.{1}.config", System.IO.Path.GetFileNameWithoutExtension(parentPhysicalFile.Name), configurationKey));

				using (var stream = System.IO.File.CreateText(fullName))
				{
					await stream.WriteLineAsync("<?xml version=\"1.0\"?>");
					await stream.WriteLineAsync("<configuration xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\">");
					await stream.WriteLineAsync("</configuration>");
				}

				await project.AddExistingFilesAsync(fullName);

				//await VS.Documents.OpenViaProjectAsync(fullName);

				//var addedPhysicalFiles = await project.AddExistingFilesAsync(fullName);

				////await project.SaveAsync();

				//foreach (var addedPhysicalFile in addedPhysicalFiles)
				//{
				//	//await parentPhysicalFile.AddNestedFileAsync(addedPhysicalFile);

				//	await addedPhysicalFile.OpenAsync();
				//}
			}
		}
	}
}