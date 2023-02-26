using System.Collections.Generic;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class Extensions_Helper
	{
		public async System.Threading.Tasks.Task AddFromRecipesAsync(Community.VisualStudio.Toolkit.Project project, IEnumerable<RecipeItem> recipeItems, IEnumerable<KeyValuePair<string, string>> replacementValues)
		{
			foreach (var recipeItem in recipeItems)
			{
				var recipeReplacementValues = new Dictionary<string, string>();
				foreach (var replacementValue in replacementValues ?? System.Array.Empty<KeyValuePair<string, string>>())
				{
					recipeReplacementValues.Add(replacementValue.Key, replacementValue.Value);
				}

				recipeItem.PreAction?.Invoke(project, recipeItem.FullName, recipeItem.Content, recipeReplacementValues);

				if (!System.IO.File.Exists(recipeItem.FullName) && !string.IsNullOrEmpty(recipeItem.Content))
				{
					await AddFromRecipeAsync(project, recipeItem.FullName, recipeItem.Content, recipeReplacementValues).ContinueWith(task =>
					{
						if (recipeItem.Open)
						{
							Community.VisualStudio.Toolkit.VS.Documents.OpenViaProjectAsync(recipeItem.FullName).GetAwaiter().GetResult();
						}
					});
				}

				recipeItem.PostAction?.Invoke(project, recipeItem.FullName, recipeItem.Content, recipeReplacementValues);
			}
		}
	}
}
