using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNet_Helper
	{
		public string GetAreaDirectory(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			var directory = solutionItem.FullPath.TrimEnd('\\', '/');

			while (directory.Length > 3)
			{
				var directoryName = System.IO.Path.GetFileName(directory);

				directory =  System.IO.Path.GetDirectoryName(directory).TrimEnd('\\', '/');

				if (string.Equals(directoryName, ControllersFolderName, StringComparison.InvariantCultureIgnoreCase) ||
				    string.Equals(directoryName, JavaScriptsFolderName, StringComparison.InvariantCultureIgnoreCase) ||
				    string.Equals(directoryName, ModelBindersFolderName, StringComparison.InvariantCultureIgnoreCase) ||
				    string.Equals(directoryName, ModelsFolderName, StringComparison.InvariantCultureIgnoreCase) ||
				    string.Equals(directoryName, RoutesFolderName, StringComparison.InvariantCultureIgnoreCase) ||
				    string.Equals(directoryName, StyleSheetsFolderName, StringComparison.InvariantCultureIgnoreCase) ||
				    string.Equals(directoryName, ViewsFolderName, StringComparison.InvariantCultureIgnoreCase))
				{
					return directory;
				}
			}

			throw new Exception(string.Format("Can't find Area Directory from \"{0}\"", directory));
		}
	}
}
