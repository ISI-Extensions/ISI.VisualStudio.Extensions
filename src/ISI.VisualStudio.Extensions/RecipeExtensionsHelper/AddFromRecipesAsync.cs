using System.Collections.Generic;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsHelper
	{
		public async System.Threading.Tasks.Task AddFromRecipesAsync(Community.VisualStudio.Toolkit.Project project, IEnumerable<RecipeItem> recipeItems, IEnumerable<KeyValuePair<string, string>> replacementValues)
		{
			foreach (var recipeItem in recipeItems)
			{
				if (!System.IO.File.Exists(recipeItem.FullName) && !string.IsNullOrEmpty(recipeItem.Content))
				{
					recipeItem.PhysicalFile = await AddFromRecipeAsync(project, recipeItem.FullName, recipeItem.Content, replacementValues);
				}

				recipeItem.PostAction?.Invoke(project, recipeItem.PhysicalFile, recipeItem.Content, replacementValues);
			}

			//project.Save();

			foreach (var recipeItem in recipeItems)
			{
				if (recipeItem.Open)
				{
					//recipeItem.ProjectItem?.Open(); //EnvDTE.Constants.vsViewKindAny
				}
			}
		}
	}
}
