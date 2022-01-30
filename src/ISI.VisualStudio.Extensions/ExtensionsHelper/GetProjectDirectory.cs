using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class ExtensionsHelper
	{
		public string GetProjectDirectory(Community.VisualStudio.Toolkit.Project project)
		{
			return System.IO.Path.GetDirectoryName(project.FullPath);
		}
	}
}
