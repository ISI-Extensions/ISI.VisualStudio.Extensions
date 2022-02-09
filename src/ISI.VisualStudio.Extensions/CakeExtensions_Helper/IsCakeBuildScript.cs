using System;

namespace ISI.VisualStudio.Extensions
{
	public partial class CakeExtensions_Helper
	{
		public bool IsCakeBuildScript(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.PhysicalFile)
			{
				return string.Equals(System.IO.Path.GetFileName(solutionItem.FullPath), "build.cake", StringComparison.InvariantCultureIgnoreCase);
			}

			return false;
		}
	}
}
