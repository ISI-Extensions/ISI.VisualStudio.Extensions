using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsHelper
	{
		public bool IsProjectRoot(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.Project)
			{
				return true;
			}

			if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.PhysicalFile)
			{
				return (string.Equals(System.IO.Path.GetExtension(solutionItem.FullPath), ".csproj", System.StringComparison.InvariantCultureIgnoreCase));
			}

			return false;
		}
	}
}
