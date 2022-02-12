using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsOptions
	{
		[Category("Project Partial Class Recipes")]
		[DisplayName("Partial Class Template")]
		public string ProjectPartialClass_PartialClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.PartialClass_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Partial Class Method Template")]
		public string ProjectPartialClass_PartialClassMethod_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.PartialClassMethod_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Async Partial Class Method Template")]
		public string ProjectPartialClass_AsyncPartialClassMethod_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.AsyncPartialClassMethod_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Partial Class Private Method Template")]
		public string ProjectPartialClass_PartialClassPrivateMethod_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.PartialClassPrivateMethod_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Async Partial Class Private Method Template")]
		public string ProjectPartialClass_AsyncPartialClassPrivateMethod_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.AsyncPartialClassPrivateMethod_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Partial Class SubClass Template")]
		public string ProjectPartialClass_PartialClassSubClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.PartialClassSubClass_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Partial Class Interface Template")]
		public string ProjectPartialClass_PartialClassInterface_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.PartialClassInterface_Template;
	}
}
