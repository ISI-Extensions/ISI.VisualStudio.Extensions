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
	[Command(PackageIds.XmlConfigurationExtensions_AddTransform_MenuItemId)]
	public class XmlConfigurationExtensions_AddTransform_Command : BaseCommand<XmlConfigurationExtensions_AddTransform_Command>
	{
		private static XmlConfigurationExtensions_Helper _xmlConfigurationExtensionsHelper = null;
		protected XmlConfigurationExtensions_Helper XmlConfigurationExtensionsHelper => _xmlConfigurationExtensionsHelper ??= Package.GetServiceProvider().GetService<XmlConfigurationExtensions_Helper>();

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

				var addExistingFilesResponse = await project.AddExistingFilesAsync(fullName);

				var transformFile = addExistingFilesResponse.NullCheckedFirstOrDefault();
				if (transformFile != null)
				{
					await transformFile.TrySetAttributeAsync(PhysicalFileAttribute.DependentUpon, parentPhysicalFile.Name.Split('/','\\').LastOrDefault());

					//await parentPhysicalFile.AddNestedFileAsync(transformFile);
				}
			}
		}
	}
}