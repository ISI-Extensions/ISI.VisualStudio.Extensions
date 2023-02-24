using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeOptions
	{
		public const string  Project_Category = "Project Recipes";

		[Category(Project_Category)]
		[DisplayName("AssemblyInfo Template")]
		public string Project_AssemblyInfo_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.AssemblyInfo;

		[Category(Project_Category)]
		[DisplayName("Enum Text Template")]
		public string Project_EnumText_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.EnumText_Template;

		[Category(Project_Category)]
		[DisplayName("ISI Extensions StartUp Class Template")]
		public string Project_ISI_Extensions_StartUpClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.ISI_Extensions_StartUpClass_Template;

		[Category(Project_Category)]
		[DisplayName("ISI Libraries StartUp Class Template")]
		public string Project_ISI_Libraries_StartUpClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.ISI_Libraries_StartUpClass_Template;

		[Category(Project_Category)]
		[DisplayName("DependencyRegister Class Template")]
		public string Project_DependencyRegisterClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.DependencyRegisterClass_Template;

		[Category(Project_Category)]
		[DisplayName("ServiceRegistrar Class Template")]
		public string Project_ServiceRegistrarClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.ServiceRegistrarClass_Template;
		
		[Category(Project_Category)]
		[DisplayName("RecordManager Interface Template")]
		public string Project_RecordManagerInterface_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.RecordManagerInterface_Template;
		
		[Category(Project_Category)]
		[DisplayName("RecordManager Record Template")]
		public string Project_RecordManagerRecord_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.Project_RecordManagerRecord_Template;
		
		[Category(Project_Category)]
		[DisplayName("RecordManager Record with PrimaryKey Template")]
		public string Project_RecordManagerRecord_PrimaryKey_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.Project_RecordManagerRecord_PrimaryKey_Template;
		
		[Category(Project_Category)]
		[DisplayName("RecordManager Record with PrimaryKey and Archive Template")]
		public string Project_RecordManagerRecord_PrimaryKeyWithArchive_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.Project_RecordManagerRecord_PrimaryKeyWithArchive_Template;
		
		[Category(Project_Category)]
		[DisplayName("ISI Extensions SqlServer RecordManager Template")]
		public string Project_ISI_Extensions_SqlServer_RecordManager_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.ISI_Extensions_SqlServer_RecordManager_Template;

		[Category(Project_Category)]
		[DisplayName("ISI Libraries SqlServer RecordManager Template")]
		public string Project_ISI_Libraries_SqlServer_RecordManager_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.ISI_Libraries_SqlServer_RecordManager_Template;
	}
}
