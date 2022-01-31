using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsOptions
	{
		[Category("Project Recipes")]
		[DisplayName("Add Enum Text Template")]
		public string Project_AddEnumTextTemplate { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddEnumTextTemplate;

		[Category("Project Recipes")]
		[DisplayName("Add StartUp Class")]
		public string Project_AddStartUpClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddStartUpClass;

		[Category("Project Recipes")]
		[DisplayName("Add DependencyRegister Class")]
		public string Project_AddDependencyRegisterClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddDependencyRegisterClass;

		[Category("Project Recipes")]
		[DisplayName("Add ServiceRegistrar Class")]
		public string Project_AddServiceRegistrarClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddServiceRegistrarClass;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Add DataTransferObject Request")]
		public string Project_AddDataTransferObjectRequest { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddDataTransferObjectRequest;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Add Async DataTransferObject Request")]
		public string Project_AddAsyncDataTransferObjectRequest { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddAsyncDataTransferObjectRequest;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Add DataTransferObject Response")]
		public string Project_AddDataTransferObjectResponse { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddDataTransferObjectResponse;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Add Serializable DataTransferObject Request")]
		public string Project_AddSerializableDataTransferObjectRequest { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddSerializableDataTransferObjectRequest;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Add Serializable DataTransferObject Response")]
		public string Project_AddSerializableDataTransferObjectResponse { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AddSerializableDataTransferObjectResponse;
	}
}
