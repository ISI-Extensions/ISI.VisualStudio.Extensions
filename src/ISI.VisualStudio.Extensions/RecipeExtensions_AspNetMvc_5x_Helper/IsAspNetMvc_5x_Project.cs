using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNetMvc_5x_Helper
	{
		public bool IsAspNetMvc_5x_Project(Community.VisualStudio.Toolkit.Project project)
		{
			var referenceNames = project.References.ToNullCheckedHashSet(reference => reference.Name, NullCheckCollectionResult.Empty);

			return referenceNames.Contains("ISI.Libraries.Web.Mvc");
		}
	}
}
