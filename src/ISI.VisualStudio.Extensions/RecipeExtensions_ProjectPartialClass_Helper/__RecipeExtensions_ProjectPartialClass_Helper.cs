using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_ProjectPartialClass_Helper : RecipeExtensions_Project_Helper
	{
		public RecipeExtensions_ProjectPartialClass_Helper(
			ISI.Extensions.VisualStudio.ProjectApi projectApi,
			ISI.Extensions.Nuget.NugetApi nugetApi)
			: base(projectApi, nugetApi)
		{

		}
	}
}