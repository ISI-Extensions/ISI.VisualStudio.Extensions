using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsHelper
	{
		public bool IsControllerFolder(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.PhysicalFolder)
			{
				var directory = solutionItem.FullPath;

				var fileNameParts = new System.Collections.Generic.Stack<string>(directory.Split(new [] { "\\" }, StringSplitOptions.RemoveEmptyEntries));
				fileNameParts.Pop();

				if (fileNameParts.Count > 0)
				{
					var fileName = fileNameParts.Pop();

					return string.Equals(fileName, ControllersFolderName, System.StringComparison.OrdinalIgnoreCase);
				}
			}

			return false;
		}
	}
}
