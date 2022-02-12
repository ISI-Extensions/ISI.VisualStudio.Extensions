using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensionsOptions
	{
		[Category("Project Recipes")]
		[DisplayName("Enum Text Template")]
		public string Project_EnumText_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.EnumText_Template;

		[Category("Project Recipes")]
		[DisplayName("StartUp Class Template")]
		public string Project_StartUpClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.StartUpClass_Template;

		[Category("Project Recipes")]
		[DisplayName("DependencyRegister Class Template")]
		public string Project_DependencyRegisterClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.DependencyRegisterClass_Template;

		[Category("Project Recipes")]
		[DisplayName("ServiceRegistrar Class Template")]
		public string Project_ServiceRegistrarClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.ServiceRegistrarClass_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("DataTransferObject Request Template")]
		public string Project_DataTransferObjectRequest_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.DataTransferObjectRequest_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Async DataTransferObject Request Template")]
		public string Project_AsyncDataTransferObjectRequest_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.AsyncDataTransferObjectRequest_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("DataTransferObject Response Template")]
		public string Project_DataTransferObjectResponse_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.DataTransferObjectResponse_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Serializable DataTransferObject Request Template")]
		public string Project_SerializableDataTransferObjectRequest_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.SerializableDataTransferObjectRequest_Template;

		[Category("Project Partial Class Recipes")]
		[DisplayName("Serializable DataTransferObject Response Template")]
		public string Project_SerializableDataTransferObjectResponse_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.SerializableDataTransferObjectResponse_Template;

		[Category("T4LocalContent Recipes")]
		[DisplayName("Settings Template")]
		public string Project_T4LocalContent_Settings_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.T4LocalContent_settings_t4_Template;

		[Category("T4LocalContent Recipes")]
		[DisplayName("Generator Template")]
		public string Project_T4LocalContent_Generator_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.T4LocalContent_Generator_t4_Template;

		[Category("T4LocalContent Recipes")]
		[DisplayName("Base Template")]
		public string Project_T4LocalContent_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.T4LocalContent_tt_Template;
	}
}
