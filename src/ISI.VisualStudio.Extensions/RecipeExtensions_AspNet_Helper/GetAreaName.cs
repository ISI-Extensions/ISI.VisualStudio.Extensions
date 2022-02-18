using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNet_Helper
	{
		public string GetAreaName(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			var pathDirectoryNames = new Stack<string>(solutionItem.FullPath.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries));

			var areaName = string.Empty;

			while (pathDirectoryNames.Any())
			{
				if (string.Equals(AreasFolderName, pathDirectoryNames.Peek(), StringComparison.InvariantCultureIgnoreCase))
				{
					return areaName;
				}

				areaName = pathDirectoryNames.Pop();
			}

			return string.Empty;
		}
	}
}
