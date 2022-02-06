using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsOptions
	{
		[Category("Project Recipes")]
		[DisplayName("Enum Text Template Template")]
		public string Project_AddEnumTextTemplate { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.EnumTextTemplate_Template;

		[Category("Project Recipes")]
		[DisplayName("StartUp Class Template")]
		public string Project_AddStartUpClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.StartUpClass_Template;

		[Category("Project Recipes")]
		[DisplayName("DependencyRegister Class Template")]
		public string Project_AddDependencyRegisterClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.DependencyRegisterClass_Template;

		[Category("Project Recipes")]
		[DisplayName("ServiceRegistrar Class Template")]
		public string Project_AddServiceRegistrarClass { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.ServiceRegistrarClass_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("DataTransferObject Request Template")]
		public string Project_AddDataTransferObjectRequest { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.DataTransferObjectRequest_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Async DataTransferObject Request Template")]
		public string Project_AddAsyncDataTransferObjectRequest { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.AsyncDataTransferObjectRequest_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("DataTransferObject Response Template")]
		public string Project_AddDataTransferObjectResponse { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.DataTransferObjectResponse_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Serializable DataTransferObject Request Template")]
		public string Project_AddSerializableDataTransferObjectRequest { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.SerializableDataTransferObjectRequest_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Serializable DataTransferObject Response Template")]
		public string Project_AddSerializableDataTransferObjectResponse { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.SerializableDataTransferObjectResponse_Template;

		[Category("T4LocalContent Recipes")]
		[DisplayName("Settings Template")]
		public string Project_T4LocalContent_Settings { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.T4LocalContent_settings_t4_Template;

		[Category("T4LocalContent Recipes")]
		[DisplayName("Generator Template")]
		public string Project_T4LocalContent_Generator { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.T4LocalContent_Generator_t4_Template;

		[Category("T4LocalContent Recipes")]
		[DisplayName("Base Template")]
		public string Project_T4LocalContent { get; set; } = ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes.T4LocalContent_tt_Template;
	}
}
