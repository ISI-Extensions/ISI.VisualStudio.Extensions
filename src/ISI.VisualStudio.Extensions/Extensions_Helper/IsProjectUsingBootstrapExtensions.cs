using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class Extensions_Helper
	{
		public bool IsProjectUsingBootstrapExtensions(Community.VisualStudio.Toolkit.Project project)
		{
			return project.UsesISILibrariesBootstrapWebMvc();
		}
	}
}
