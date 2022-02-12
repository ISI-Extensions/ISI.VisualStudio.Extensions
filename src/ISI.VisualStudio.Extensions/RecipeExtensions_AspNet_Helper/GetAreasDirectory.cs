using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNet_Helper
	{
		public string GetAreasDirectory(Community.VisualStudio.Toolkit.Project project)
		{
			return System.IO.Path.Combine(GetProjectDirectory(project), AreasFolderName);
		}
	}
}
