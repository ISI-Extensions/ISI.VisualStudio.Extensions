using System;

namespace ISI.VisualStudio.Extensions
{
	public partial class XmlConfigurationExtensionsHelper
	{
		public bool IsXmlConfiguration(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.PhysicalFile)
			{
				return string.Equals(System.IO.Path.GetExtension(solutionItem.FullPath), ".config", StringComparison.InvariantCultureIgnoreCase);
			}

			return false;
		}
	}
}
