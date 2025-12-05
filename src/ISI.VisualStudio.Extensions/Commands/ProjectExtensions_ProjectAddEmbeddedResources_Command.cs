#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using ISI.VisualStudio.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.ProjectExtensions_ProjectAddEmbeddedResources_MenuItemId)]
	public class ProjectExtensions_ProjectAddEmbeddedResources_Command : BaseCommand<ProjectExtensions_ProjectAddEmbeddedResources_Command>
	{
		private static ProjectExtensions_Helper _projectExtensionsHelper = null;
		protected ProjectExtensions_Helper ProjectExtensionsHelper => _projectExtensionsHelper ??= Package.GetServiceProvider().GetService<ProjectExtensions_Helper>();

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var dte = Package.GetDTE2();
			IDictionary<string, bool> fileNames = null;

			string FormatExtension(string fileName)
			{
				fileName = fileName.Split(['\\'], StringSplitOptions.RemoveEmptyEntries).Last().Trim().ToLower();

				if (string.Equals(fileName, "web.config", StringComparison.InvariantCulture))
				{
					fileName = string.Format("**\\{0}", fileName);
				}
				else
				{
					fileName = fileName.Split(['.'], StringSplitOptions.RemoveEmptyEntries).Last().Trim();
					fileName = string.Format("**\\*.{0}", fileName);
				}

				return fileName;
			}

			var project = dte.GetSelectedProject();

			if (!project.Saved)
			{
				project.Save();
			}

			var buildProject = project.AsMSBuildProject();

			var targets = buildProject.Targets;

			if (targets.ContainsKey("BeforeBuild"))
			{
				var target = targets["BeforeBuild"];

				foreach (var child in target.Children)
				{
					if (child is Microsoft.Build.Execution.ProjectItemGroupTaskInstance itemGroup)
					{
						foreach (var itemInstance in itemGroup.Items)
						{
							if (string.Equals(itemInstance.ItemType, "EmbeddedResource", StringComparison.InvariantCulture))
							{
								fileNames ??= ProjectExtensionsHelper.GetEmbeddedFileNameSearchPatterns().ToDictionary(ef => ef.ToLower(), ef => false);

								foreach (var fileName in itemInstance.Include.Split([';'], StringSplitOptions.RemoveEmptyEntries).Select(FormatExtension))
								{
									if (ProjectExtensionsHelper.GetFilteredFileNameSearchPatterns().Any(f => fileName.StartsWith(f, StringComparison.InvariantCultureIgnoreCase)))
									{

									}
									else if (fileNames.ContainsKey(fileName))
									{
										fileNames[fileName] = true;
									}
									else
									{
										fileNames.Add(fileName, true);
									}
								}
							}
						}
					}
				}
			}

			fileNames ??= ProjectExtensionsHelper.GetEmbeddedFileNameSearchPatterns().ToDictionary(ef => ef.ToLower(), ef => true);

			foreach (var itemGroup in buildProject.Xml.ItemGroups)
			{
				foreach (var itemInstance in itemGroup.Items)
				{
					if (string.Equals(itemInstance.ItemType, "None", StringComparison.InvariantCulture) ||
							string.Equals(itemInstance.ItemType, "Content", StringComparison.InvariantCulture) ||
							string.Equals(itemInstance.ItemType, "EmbeddedResource", StringComparison.InvariantCulture))
					{
						foreach (var fileName in itemInstance.Include.Split([';'], StringSplitOptions.RemoveEmptyEntries).Select(FormatExtension))
						{
							if (ProjectExtensionsHelper.GetFilteredFileNameSearchPatterns().Any(f => fileName.StartsWith(f, StringComparison.InvariantCultureIgnoreCase)))
							{

							}
							else if (fileNames.ContainsKey(fileName))
							{

							}
							else
							{
								fileNames.Add(fileName, false);
							}
						}
					}
				}
			}

			var embeddedFileNames = fileNames.ToNullCheckedArray(fileName => new EmbeddedFileName()
			{
				FileName = fileName.Key,
				Active = fileName.Value
			}, NullCheckCollectionResult.Empty);

			var embeddedFileNamesDialog = new EmbeddedFileNamesDialog(embeddedFileNames);

			var embeddedFileNamesDialogResult = await embeddedFileNamesDialog.ShowDialogAsync();

			if (embeddedFileNamesDialogResult.GetValueOrDefault())
			{
				var projectRoot = buildProject.Xml;

				var embeddedTarget = projectRoot.Targets.FirstOrDefault(t => string.Equals(t.Name, "BeforeBuild", StringComparison.InvariantCulture));

				Microsoft.Build.Construction.ProjectItemGroupElement embeddedTargetGroup = null;

				if (embeddedTarget == null)
				{
					embeddedTarget = projectRoot.AddTarget("BeforeBuild");
				}
				else
				{
					foreach (var itemGroup in embeddedTarget.ItemGroups)
					{
						var items = itemGroup.Items.Where(ig => string.Equals(ig.ItemType, "EmbeddedResource", StringComparison.InvariantCulture)).ToArray();

						if (items.Any())
						{
							embeddedTargetGroup = itemGroup;

							foreach (var item in items)
							{
								itemGroup.RemoveChild(item);
							}
						}
					}
				}

				embeddedTargetGroup ??= embeddedTarget.AddItemGroup();

				foreach (var fileName in embeddedFileNames.Where(f => f.Active))
				{
					embeddedTargetGroup.AddItem("EmbeddedResource", fileName.FileName);
				}

				buildProject.Save();
			}
		}
	}
}
