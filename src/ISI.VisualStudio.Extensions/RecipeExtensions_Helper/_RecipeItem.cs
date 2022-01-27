using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_Helper
	{
		public delegate void RecipeItemPostAction(Community.VisualStudio.Toolkit.Project project, string fullName, string content, IEnumerable<KeyValuePair<string, string>> replacementValues);

		public class RecipeItem
		{
			public string FullName { get; }
			public string Content { get; }
			public bool Open { get; }
			public RecipeItemPostAction PostAction { get; }

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
