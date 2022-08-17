using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNetMvc_5x_Helper : RecipeExtensions_AspNet_Helper
	{
		public RecipeExtensions_AspNetMvc_5x_Helper(
			ISI.Extensions.VisualStudio.ProjectApi projectApi,
			ISI.Extensions.Nuget.NugetApi nugetApi)
			: base(projectApi, nugetApi)
		{

		}
	}
}