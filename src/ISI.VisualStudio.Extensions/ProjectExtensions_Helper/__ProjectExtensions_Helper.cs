﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class ProjectExtensions_Helper : Extensions_Helper
	{
		protected ISI.Extensions.VisualStudio.SolutionApi SolutionApi { get; }
		protected ISI.Extensions.VisualStudio.ProjectApi ProjectApi { get; }

		public ProjectExtensions_Helper(
			ISI.Extensions.VisualStudio.SolutionApi solutionApi,
			ISI.Extensions.VisualStudio.ProjectApi projectApi)
		{
			SolutionApi = solutionApi;
			ProjectApi = projectApi;
		}
	}
}
