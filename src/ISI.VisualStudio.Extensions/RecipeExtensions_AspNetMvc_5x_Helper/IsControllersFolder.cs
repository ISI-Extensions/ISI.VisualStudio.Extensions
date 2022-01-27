using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNetMvc_5x_Helper
	{
		public bool IsControllersFolder(Community.VisualStudio.Toolkit.Project project, Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.PhysicalFolder)
			{
				var directory = solutionItem.FullPath.TrimEnd('\\','/');

				if (string.Equals(directory.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault(), ControllersFolderName, StringComparison.InvariantCultureIgnoreCase))
				{
					var projectRootDirectory = System.IO.Path.GetDirectoryName(project.FullPath);
					var rootDirectory = directory.TrimEnd(ControllersFolderName, StringComparison.InvariantCultureIgnoreCase);

					if (ISI.Extensions.IO.Path.IsPathEqual(projectRootDirectory, rootDirectory))
					{
						return true;
					}

					projectRootDirectory = string.Format("{0}\\", System.IO.Path.Combine(projectRootDirectory, AreasFolderName));

					return ISI.Extensions.IO.Path.IsPathEqual(projectRootDirectory, ISI.Extensions.IO.Path.GetCommonPath(new[] { rootDirectory, projectRootDirectory }));
				}
			}

			return false;
		}
	}
}
