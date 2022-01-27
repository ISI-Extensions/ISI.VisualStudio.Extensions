using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_Helper
	{
		public void ReplaceFileContent(string fileName, IEnumerable<KeyValuePair<string, string>> replacementValues)
		{
			if (System.IO.File.Exists(fileName) && (replacementValues != null))
			{
				var content = System.IO.File.ReadAllText(fileName);

				foreach (var replacementValue in replacementValues)
				{
					content = content.Replace(replacementValue.Key, replacementValue.Value);
				}

				System.IO.File.WriteAllText(fileName, content);
			}
		}
	}
}