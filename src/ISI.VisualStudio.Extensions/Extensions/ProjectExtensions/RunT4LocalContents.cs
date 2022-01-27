using System.ComponentModel.Design;
using ISI.Extensions.Extensions;
using ISI.VisualStudio.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public static partial class ProjectExtensions
	{
		public static void RunT4LocalContents(this EnvDTE.Project project)
		{
			foreach (EnvDTE.ProjectItem projectItem in project.ProjectItems)
			{
				RunT4LocalContents(projectItem);
			}
		}

		public static void RunT4LocalContents(EnvDTE.ProjectItem projectItem)
		{
			if (projectItem.Object is EnvDTE.Project subProject)
			{
				RunT4LocalContents(subProject);
			}

			if (projectItem.ProjectItems != null)
			{
				foreach (EnvDTE.ProjectItem subProjectItem in projectItem.ProjectItems)
				{
					RunT4LocalContents(subProjectItem);
				}
			}

			if (string.Equals(System.IO.Path.GetFileName(projectItem.Name), "T4LocalContent.tt", System.StringComparison.InvariantCultureIgnoreCase))
			{
				if (projectItem.Object is VSLangProj.VSProjectItem t4TransformsProjectItem)
				{
					t4TransformsProjectItem.RunCustomTool();
				}
			}
		}
	}
}
