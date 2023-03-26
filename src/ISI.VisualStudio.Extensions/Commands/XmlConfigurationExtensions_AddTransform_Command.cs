﻿#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
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

				var fullName = System.IO.Path.Combine(configDirectory, string.Format("{0}.{1}.config", System.IO.Path.GetFileNameWithoutExtension(parentPhysicalFile.FullPath), configurationKey));

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
					await transformFile.TrySetAttributeAsync(PhysicalFileAttribute.DependentUpon, parentPhysicalFile.Name.Split('/', '\\').LastOrDefault()).ContinueWith(async _ =>
					{
						await project.SaveAsync().ContinueWith(async _ =>
						{
							await project.UnloadAsync().ContinueWith(async _ =>
							{
								await project.LoadAsync();
							});
						});
					});
				}
			}
		}
	}
}