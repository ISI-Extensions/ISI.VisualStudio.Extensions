using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeOptions
	{
		public const string  Project_Category = "Project Recipes";

		[Category(Project_Category)]
		[DisplayName("Enum Text Template")]
		public string Project_EnumText_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.EnumText_Template;

		[Category(Project_Category)]
		[DisplayName("StartUp Class Template")]
		public string Project_StartUpClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.StartUpClass_Template;

		[Category(Project_Category)]
		[DisplayName("DependencyRegister Class Template")]
		public string Project_DependencyRegisterClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.DependencyRegisterClass_Template;

		[Category(Project_Category)]
		[DisplayName("ServiceRegistrar Class Template")]
		public string Project_ServiceRegistrarClass_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.ServiceRegistrarClass_Template;
		
		[Category(Project_Category)]
		[DisplayName("AssemblyInfo Template")]
		public string Project_AssemblyInfo_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.AssemblyInfo;
	}
}
