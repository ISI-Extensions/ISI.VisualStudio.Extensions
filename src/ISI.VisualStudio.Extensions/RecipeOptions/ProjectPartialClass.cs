#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
		[DisplayName("DataTransferObject Response Template")]
		public string Project_DataTransferObjectResponse_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes.DataTransferObjectResponse_Template;
	}
}
