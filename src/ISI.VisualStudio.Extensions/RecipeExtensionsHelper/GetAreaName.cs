using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsHelper
	{
		public string GetAreaName(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			var pathDirectoryNames = new Stack<string>(solutionItem.FullPath.Split(new [] { "\\" }, StringSplitOptions.RemoveEmptyEntries));
			pathDirectoryNames.Pop();
			pathDirectoryNames.Pop();

			var areaName = pathDirectoryNames.Pop();
			
			if (!string.Equals(AreasFolderName, pathDirectoryNames.Pop(), StringComparison.InvariantCultureIgnoreCase))
			{
				areaName = string.Empty;
			}

			return areaName;
		}
	}
}
