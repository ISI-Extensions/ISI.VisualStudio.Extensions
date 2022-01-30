using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public static partial class DTE2Extensions
	{
		public static EnvDTE.UIHierarchyItem[] GetSelectedSolutionItems(this EnvDTE80.DTE2 dte2)
		{
			var solutionTree = dte2.ToolWindows.SolutionExplorer;

			if ((solutionTree.SelectedItems != null) && ((object[])solutionTree.SelectedItems).Any())
			{
				return ((object[])solutionTree.SelectedItems).Cast<EnvDTE.UIHierarchyItem>().ToArray();
			}

			return null;
		}
	}
}
