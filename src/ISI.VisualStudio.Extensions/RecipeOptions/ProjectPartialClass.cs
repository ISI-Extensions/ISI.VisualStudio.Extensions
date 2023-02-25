using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeOptions
	{
		public const string  ProjectPartialClass_Category = "Project Partial Class Recipes";

		[Category(ProjectPartialClass_Category)]
		[DisplayName("Partial Class Template")]
		public string ProjectPartialClass_PartialClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.PartialClass_Template;

		[Category(ProjectPartialClass_Category)]
		[DisplayName("Partial Class Method Template")]
		public string ProjectPartialClass_PartialClassMethod_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.PartialClassMethod_Template;

		[Category(ProjectPartialClass_Category)]
		[DisplayName("Async Partial Class Method Template")]
		public string ProjectPartialClass_AsyncPartialClassMethod_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.AsyncPartialClassMethod_Template;

		[Category(ProjectPartialClass_Category)]
		[DisplayName("Partial Class Private Method Template")]
		public string ProjectPartialClass_PartialClassPrivateMethod_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.PartialClassPrivateMethod_Template;

		[Category(ProjectPartialClass_Category)]
		[DisplayName("Async Partial Class Private Method Template")]
		public string ProjectPartialClass_AsyncPartialClassPrivateMethod_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.AsyncPartialClassPrivateMethod_Template;

		[Category(ProjectPartialClass_Category)]
		[DisplayName("Partial Class SubClass Template")]
		public string ProjectPartialClass_PartialClassSubClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.PartialClassSubClass_Template;

		[Category(ProjectPartialClass_Category)]
		[DisplayName("Partial Class Interface Template")]
		public string ProjectPartialClass_PartialClassInterface_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes.PartialClassInterface_Template;

		[Category(ProjectPartialClass_Category)]
		[DisplayName("DataTransferObject Request Template")]
		public string Project_DataTransferObjectRequest_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.DataTransferObjectRequest_Template;

		[Category(ProjectPartialClass_Category)]
		[DisplayName("Async DataTransferObject Request Template")]
		public string Project_AsyncDataTransferObjectRequest_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.AsyncDataTransferObjectRequest_Template;

		[Category(ProjectPartialClass_Category)]
		[DisplayName("DataTransferObject Response Template")]
		public string Project_DataTransferObjectResponse_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.DataTransferObjectResponse_Template;
	}
}
