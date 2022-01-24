using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsOptions
	{
		[Category("Solution Recipes")]
		[DisplayName("editorconfig")]
		public string Solution_EditorConfig { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Solution_Recipes.editorconfig;
	}
}
