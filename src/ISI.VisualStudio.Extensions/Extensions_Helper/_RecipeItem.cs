using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class Extensions_Helper
	{
		public delegate void RecipeItemPreAction(Community.VisualStudio.Toolkit.Project project, string fullName, string content, IDictionary<string, string> replacementValues);
		public delegate void RecipeItemPostAction(Community.VisualStudio.Toolkit.Project project, string fullName, string content, IDictionary<string, string> replacementValues);

		public class RecipeItem
		{
			public string FullName { get; }
			public string Content { get; }
			public bool Open { get; }
			public RecipeItemPostAction PreAction { get; }
			public RecipeItemPostAction PostAction { get; }

			public RecipeItem(string fullName, string content, bool open = false, RecipeItemPostAction preAction = null, RecipeItemPostAction postAction = null)
			{
				FullName = fullName;
				Content = content;
				Open = open;
				PreAction = preAction;
				PostAction = postAction;
			}
		}
	}
}
