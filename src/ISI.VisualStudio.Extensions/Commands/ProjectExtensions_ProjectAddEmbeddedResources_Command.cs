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
	[Command(PackageIds.ProjectExtensionsProjectAddEmbeddedResourcesMenuItemId)]
	public class ProjectExtensions_ProjectAddEmbeddedResources_Command : BaseCommand<ProjectExtensions_ProjectAddEmbeddedResources_Command>
	{
		private static ProjectExtensionsHelper _projectExtensionsHelper = null;
		protected ProjectExtensionsHelper ProjectExtensionsHelper => _projectExtensionsHelper ??= Package.GetServiceProvider().GetService<ProjectExtensionsHelper>();

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var dte = Package.GetDTE2();
			IDictionary<string, bool> fileNames = null;

			string FormatExtension(string fileName)
			{
				fileName = fileName.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last().Trim().ToLower();

				if (string.Equals(fileName, "web.config", StringComparison.InvariantCulture))
				{
					fileName = string.Format("**\\{0}", fileName);
				}
				else
				{
					fileName = fileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last().Trim();
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

								foreach (var fileName in itemInstance.Include.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(FormatExtension))
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
						foreach (var fileName in itemInstance.Include.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(FormatExtension))
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

			using (var form = new ISI.Extensions.VisualStudio.Forms.EmbeddedFileNamesForm(fileNames))
			{
				if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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

					if (embeddedTargetGroup == null)
					{
						embeddedTargetGroup = embeddedTarget.AddItemGroup();
					}

					foreach (var fileName in fileNames.Where(f => f.Value))
					{
						embeddedTargetGroup.AddItem("EmbeddedResource", fileName.Key);
					}

					buildProject.Save();
				}
			}
		}
	}
}
