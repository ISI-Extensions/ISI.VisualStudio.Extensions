using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsOptions
	{
		[Category("Project Partial Class Recipes")]
		[DisplayName("Add Partial Class")]
		public string ProjectPartialClass_AddPartialClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddPartialClass;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Add Partial Class Method")]
		public string ProjectPartialClass_AddPartialClassMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddPartialClassMethod;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Add Async Partial Class Method")]
		public string ProjectPartialClass_AddAsyncPartialClassMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddAsyncPartialClassMethod;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Add Partial Class Private Method")]
		public string ProjectPartialClass_AddPartialClassPrivateMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddPartialClassPrivateMethod;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Add Async Partial Class Private Method")]
		public string ProjectPartialClass_AddAsyncPartialClassPrivateMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddAsyncPartialClassPrivateMethod;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Add Partial Class SubClass")]
		public string ProjectPartialClass_AddPartialClassSubClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddPartialClassSubClass;
	}
}
