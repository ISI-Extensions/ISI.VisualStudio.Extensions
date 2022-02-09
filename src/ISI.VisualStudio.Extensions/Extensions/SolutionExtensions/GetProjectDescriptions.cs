using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions.Extensions
{
	public static class SolutionExtensions
	{
		public class ProjectDescription
		{
			public Community.VisualStudio.Toolkit.Project Project { get; set; }
			public string Description { get; set; }
			public string RootNamespace { get; set; }
		}

		public static ProjectDescription[] GetProjectDescriptions(this Community.VisualStudio.Toolkit.Solution solution)
		{
			var projects = new List<ProjectDescription>();

			void addChildren(string path, IEnumerable<Community.VisualStudio.Toolkit.SolutionItem> solutionItems)
			{
				if (solutionItems.NullCheckedAny())
				{
					foreach (var solutionItem in solutionItems)
					{
						switch (solutionItem.Type)
						{
							case Community.VisualStudio.Toolkit.SolutionItemType.Project:
								projects.Add(new ProjectDescription()
								{
									Project = solutionItem as Community.VisualStudio.Toolkit.Project,
									Description = string.Format("{0}{1}", path, solutionItem.Name),
									RootNamespace = (solutionItem as Community.VisualStudio.Toolkit.Project)?.GetRootNamespace(),
								});
								break;

							case Community.VisualStudio.Toolkit.SolutionItemType.SolutionFolder:
								addChildren((string.IsNullOrWhiteSpace(path) ? string.Format("{0}\\", solutionItem.Name) : string.Format("{0}\\{1}\\", path, solutionItem.Name)), solutionItem.Children);
								break;
						}
					}
				}
			}

			addChildren(string.Empty, solution.Children);

			return projects.OrderBy(project => project.Description, StringComparer.InvariantCultureIgnoreCase).ToArray();
		}
	}
}
