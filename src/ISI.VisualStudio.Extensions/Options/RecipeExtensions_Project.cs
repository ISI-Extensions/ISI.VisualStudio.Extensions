using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsOptions
	{
		[Category("Project Recipes")]
		[DisplayName("Add Enum Text Template")]
		public string Project_AddEnumTextTemplate { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddEnumTextTemplate;

		[Category("Project Recipes")]
		[DisplayName("Add Partial Class")]
		public string Project_AddPartialClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddPartialClass;

		[Category("Project Recipes")]
		[DisplayName("Add Partial Class Method")]
		public string Project_AddPartialClassMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddPartialClassMethod;

		[Category("Project Recipes")]
		[DisplayName("Add Async Partial Class Method")]
		public string Project_AddAsyncPartialClassMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddAsyncPartialClassMethod;

		[Category("Project Recipes")]
		[DisplayName("Add Partial Class Private Method")]
		public string Project_AddPartialClassPrivateMethod { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddPartialClassPrivateMethod;

		[Category("Project Recipes")]
		[DisplayName("Add Partial Class SubClass")]
		public string Project_AddPartialClassSubClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddPartialClassSubClass;

		[Category("Project Recipes")]
		[DisplayName("Add DataTransferObject Request")]
		public string Project_AddDataTransferObjectRequest { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddDataTransferObjectRequest;

		[Category("Project Recipes")]
		[DisplayName("Add Async DataTransferObject Request")]
		public string Project_AddAsyncDataTransferObjectRequest { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddAsyncDataTransferObjectRequest;

		[Category("Project Recipes")]
		[DisplayName("Add DataTransferObject Response")]
		public string Project_AddDataTransferObjectResponse { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddDataTransferObjectResponse;

		[Category("Project Recipes")]
		[DisplayName("Add StartUp Class")]
		public string Project_AddStartUpClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddStartUpClass;

		[Category("Project Recipes")]
		[DisplayName("Add DependencyRegister Class")]
		public string Project_AddDependencyRegisterClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddDependencyRegisterClass;
	}
}
