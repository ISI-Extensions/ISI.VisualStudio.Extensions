using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsHelper
	{
		public bool IsAreasFolder(Community.VisualStudio.Toolkit.Project project, Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.PhysicalFile)
			{
				var directory = solutionItem.FullPath.TrimEnd('\\', '/');

				if (string.Equals(directory.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault(), "Areas", StringComparison.InvariantCultureIgnoreCase))
				{
					var projectRootDirectory = System.IO.Path.GetDirectoryName(project.FullPath).TrimEnd('\\', '/');
					var rootDirectory = directory.TrimEnd("Areas", StringComparison.InvariantCultureIgnoreCase).TrimEnd('\\', '/');

					return (string.Equals(projectRootDirectory, rootDirectory, System.StringComparison.InvariantCultureIgnoreCase));
				}
			}

			return false;
		}
	}
}
