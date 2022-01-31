using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class ExtensionsHelper
	{
		public string GetNamespace(Community.VisualStudio.Toolkit.Project project, Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			var @namespace = GetRootNamespace(project);

			var projectDirectory = System.IO.Path.GetDirectoryName(project.FullPath);

			var path = solutionItem.FullPath.TrimStart(projectDirectory).Trim('\\', '/');

			if (!string.IsNullOrWhiteSpace(path))
			{
				var pathParts = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

				if (pathParts.NullCheckedAny())
				{
					@namespace = string.Format("{0}.{1}", @namespace, string.Join(".", pathParts));
				}
			}

			return @namespace;
		}
	}
}
