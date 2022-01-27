using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNetMvc_5x_Helper
	{
		public string GetControllerName(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (IsControllerFolder(solutionItem))
			{
				return solutionItem.Text.TrimEnd("Controller");
			}

			return string.Empty;
		}
	}
}
