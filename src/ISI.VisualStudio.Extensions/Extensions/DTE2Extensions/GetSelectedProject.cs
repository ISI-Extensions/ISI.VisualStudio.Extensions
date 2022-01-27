using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public static partial class DTE2Extensions
	{
		public static EnvDTE.Project GetSelectedProject(this EnvDTE80.DTE2 dte2)
		{
			var item = dte2.SelectedItems.Item(1);

			if (item?.Project != null)
			{
				return item.Project;
			}

			if (item?.ProjectItem != null)
			{
				return item.ProjectItem.ContainingProject;
			}

			if (item is VSLangProj.Reference projectReference)
			{
				return projectReference.ContainingProject;
			}

			if (dte2.ActiveSolutionProjects is Array activeSolutionProjects && activeSolutionProjects.Length > 0)
			{
				if (activeSolutionProjects.GetValue(0) is EnvDTE.Project activeProject)
				{
					return activeProject;
				}
			}

			return null;
		}
	}
}
