using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensionsHelper
	{
		public bool IsReferencesFolder(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (string.Equals(solutionItem.Text, "References", StringComparison.InvariantCultureIgnoreCase))
			{
				if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.VirtualFolder)
				{
					return (solutionItem.FullPath == null);
				}
			}

			return false;
		}
	}
}
