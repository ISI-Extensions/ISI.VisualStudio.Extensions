using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsHelper
	{
		public string FilterWebConfig(Community.VisualStudio.Toolkit.Project project, string content)
		{
			var contentLines = content.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

			if (!IsProjectUsingCmsExtensions(project))
			{
				contentLines.RemoveAll(line => (line.IndexOf(".Cms.Web.Mvc", StringComparison.InvariantCultureIgnoreCase) >= 0));
			}

			if (!IsProjectUsingJQueryExtensions(project))
			{
				contentLines.RemoveAll(line => (line.IndexOf(".Libraries.JQuery.Web.Mvc", StringComparison.InvariantCultureIgnoreCase) >= 0));
			}

			if (!IsProjectUsingBootstrapExtensions(project))
			{
				contentLines.RemoveAll(line => (line.IndexOf(".Libraries.Bootstrap.Web.Mvc", StringComparison.InvariantCultureIgnoreCase) >= 0));
			}

			return string.Join("\r\n", contentLines);
		}
	}
}
