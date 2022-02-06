using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class ExtensionsHelper
	{
		public string GetNamespace(Community.VisualStudio.Toolkit.Project project, Community.VisualStudio.Toolkit.SolutionItem solutionItem, string className = null)
		{
			var @namespace = GetRootNamespace(project);

			var projectDirectory = System.IO.Path.GetDirectoryName(project.FullPath);

			var path = System.IO.Path.GetDirectoryName(solutionItem.FullPath).TrimStart(projectDirectory).Trim('\\', '/');

			if (!string.IsNullOrWhiteSpace(path))
			{
				var pathParts = new List<string>(path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries));

				if (pathParts.NullCheckedAny())
				{
					if (string.Equals(pathParts.Last(), className, StringComparison.InvariantCulture))
					{
						pathParts.RemoveAt(pathParts.Count - 1);
					}

					if (pathParts.NullCheckedAny())
					{
						@namespace = string.Format("{0}.{1}", @namespace, string.Join(".", pathParts));
					}
				}
			}

			return @namespace;
		}
	}
}
