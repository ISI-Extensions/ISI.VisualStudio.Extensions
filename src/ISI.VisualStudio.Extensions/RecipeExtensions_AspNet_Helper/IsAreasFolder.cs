using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNet_Helper
	{
		public bool IsAreasFolder(Community.VisualStudio.Toolkit.Project project, Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.PhysicalFolder)
			{
				var areasDirectory = GetAreasDirectory(project);
				var directory = solutionItem.FullPath;

				return ISI.Extensions.IO.Path.IsPathEqual(directory, areasDirectory);
			}

			return false;
		}
	}
}
