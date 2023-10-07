#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
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
		[DisplayName("ISI Extensions Repository RecordManager Template")]
		public string Project_ISI_Extensions_Repository_RecordManager_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.ISI_Extensions_Repository_RecordManager_Template;

		[Category(Project_Category)]
		[DisplayName("ISI Libraries Repository RecordManager Template")]
		public string Project_ISI_Libraries_Repository_RecordManager_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.ISI_Libraries_Repository_RecordManager_Template;

		[Category(Project_Category)]
		[DisplayName("Serializable Object Template")]
		public string Project_SerializableObject_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.SerializableObject_Template;

		[Category(Project_Category)]
		[DisplayName("Serializable DataTransferObject Request Template")]
		public string Project_SerializableDataTransferObjectRequest_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.SerializableDataTransferObjectRequest_Template;

		[Category(Project_Category)]
		[DisplayName("Serializable DataTransferObject Response Template")]
		public string Project_SerializableDataTransferObjectResponse_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.SerializableDataTransferObjectResponse_Template;
	}
}
