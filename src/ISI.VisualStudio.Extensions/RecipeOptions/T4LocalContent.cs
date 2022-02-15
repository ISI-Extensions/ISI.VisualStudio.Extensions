using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeOptions
	{
		public const string  T4LocalContent_Category = "T4LocalContent Recipes";

		[Category(T4LocalContent_Category)]
		[DisplayName("Settings Template")]
		public string Project_T4LocalContent_Settings_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.T4LocalContent_settings_t4_Template;

		[Category(T4LocalContent_Category)]
		[DisplayName("Generator Template")]
		public string Project_T4LocalContent_Generator_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.T4LocalContent_Generator_t4_Template;

		[Category(T4LocalContent_Category)]
		[DisplayName("Base Template")]
		public string Project_T4LocalContent_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.T4LocalContent_tt_Template;
	}
}
