using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsHelper
	{
		public delegate void RecipeItemPostAction(Community.VisualStudio.Toolkit.Project project, Community.VisualStudio.Toolkit.PhysicalFile physicalFile, string content, IEnumerable<KeyValuePair<string, string>> replacementValues);

		public class RecipeItem
		{
			public string FullName { get; }
			public string Content { get; }
			public bool Open { get; }
			public RecipeItemPostAction PostAction { get; }
			public Community.VisualStudio.Toolkit.PhysicalFile PhysicalFile { get; set; }

			public RecipeItem(string fullName, string content, bool open = false, RecipeItemPostAction postAction = null)
			{
				FullName = fullName;
				Content = content;
				Open = open;
				PostAction = postAction;
			}
		}
	}
}
