using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class SolutionExtensions_Helper : Extensions_Helper
	{
		public const string vsProjectKindSolutionItems = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";

		protected ISI.Extensions.VisualStudio.SolutionApi SolutionApi { get; }
		protected ISI.Extensions.VisualStudio.ProjectApi ProjectApi { get; }

		public SolutionExtensions_Helper(
			ISI.Extensions.VisualStudio.SolutionApi solutionApi,
			ISI.Extensions.VisualStudio.ProjectApi projectApi)
		{
			SolutionApi = solutionApi;
			ProjectApi = projectApi;
		}
	}
}
