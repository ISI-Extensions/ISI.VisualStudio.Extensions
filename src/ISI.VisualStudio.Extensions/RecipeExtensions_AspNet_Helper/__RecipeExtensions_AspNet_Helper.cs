using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNet_Helper : ProjectExtensions_Helper
	{
		public const string AreasFolderName = "Areas";
		public const string ControllersFolderName = "Controllers";
		public const string JavaScriptsFolderName = "JavaScripts";
		public const string ModelBindersFolderName = "ModelBinders";
		public const string ModelsFolderName = "Models";
		public const string RoutesFolderName = "Routes";
		public const string StyleSheetsFolderName = "StyleSheets";
		public const string ViewsFolderName = "Views";

		public RecipeExtensions_AspNet_Helper(
			ISI.Extensions.VisualStudio.SolutionApi solutionApi,
			ISI.Extensions.VisualStudio.ProjectApi projectApi)
			: base(solutionApi, projectApi)
		{

		}
	}
}