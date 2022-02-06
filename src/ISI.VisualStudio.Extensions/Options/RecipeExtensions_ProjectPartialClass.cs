using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsOptions
	{
		[Category("Project Partial Class Recipes")]
		[DisplayName("Partial Class Template")]
		public string ProjectPartialClass_AddPartialClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddPartialClass;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Partial Class Method Template")]
		public string ProjectPartialClass_AddPartialClassMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddPartialClassMethod;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Async Partial Class Method Template")]
		public string ProjectPartialClass_AddAsyncPartialClassMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddAsyncPartialClassMethod;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Partial Class Private Method Template")]
		public string ProjectPartialClass_AddPartialClassPrivateMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddPartialClassPrivateMethod;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Async Partial Class Private Method Template")]
		public string ProjectPartialClass_AddAsyncPartialClassPrivateMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddAsyncPartialClassPrivateMethod;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Partial Class SubClass Template")]
		public string ProjectPartialClass_AddPartialClassSubClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.ProjectPartialClass_Recipes.AddPartialClassSubClass;
	}
}
