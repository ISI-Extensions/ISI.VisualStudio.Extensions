#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
		public const string  AspNetMvc_6x_Category = "AspNetMvc 6.x Recipes";

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Area Controller Template")]
		public string AspNetMvc_6x_Area_Controller_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.Area_Controller_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Area BaseModel Template")]
		public string AspNetMvc_6x_Area_BaseModel_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.Area_BaseModel_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Area Routes Template")]
		public string AspNetMvc_6x_Area_Routes_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.Area_Routes_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Controller BaseModel Template")]
		public string AspNetMvc_6x_Controller_BaseModel_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.Controller_BaseModel_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Controller BaseModelRoot Template")]
		public string AspNetMvc_6x_Controller_BaseModelRoot_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.Controller_BaseModelRoot_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Controller Controller Template")]
		public string AspNetMvc_6x_Controller_Controller_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.Controller_Controller_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Controller ControllerRoot Template")]
		public string AspNetMvc_6x_Controller_ControllerRoot_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.Controller_ControllerRoot_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Controller Routes Template")]
		public string AspNetMvc_6x_Controller_Routes_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.Controller_Routes_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Controller RoutesRoot Template")]
		public string AspNetMvc_6x_Controller_RoutesRoot_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.Controller_RoutesRoot_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Action with View Action Template")]
		public string AspNetMvc_6x_ActionWithView_Action_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ActionWithView_Action_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Action with View JavaScript Template")]
		public string AspNetMvc_6x_ActionWithView_JavaScript_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ActionWithView_JavaScript_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Action with View Model Template")]
		public string AspNetMvc_6x_ActionWithView_Model_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ActionWithView_Model_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Action with View StyleSheet Template")]
		public string AspNetMvc_6x_ActionWithView_StyleSheet_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ActionWithView_StyleSheet_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Action with View View Template")]
		public string AspNetMvc_6x_ActionWithView_View_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ActionWithView_View_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Action with Partial View Action Template")]
		public string AspNetMvc_6x_ActionWithPartialView_Action_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ActionWithPartialView_Action_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Action with Partial View Model Template")]
		public string AspNetMvc_6x_ActionWithPartialView_Model_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ActionWithPartialView_Model_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Action with Partial View PartialView Template")]
		public string AspNetMvc_6x_ActionWithPartialView_PartialView_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ActionWithPartialView_PartialView_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("JavaScriptsSharedLayout Template")]
		public string AspNetMvc_6x_JavaScriptsSharedLayout_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.JavaScriptsSharedLayout_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("JavaScriptsControllerLayout Template")]
		public string AspNetMvc_6x_JavaScriptsControllerLayout_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.JavaScriptsControllerLayout_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("StyleSheetsSharedLayout Template")]
		public string AspNetMvc_6x_StyleSheetsSharedLayout_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.StyleSheetsSharedLayout_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("StyleSheetsControllerLayout Template")]
		public string AspNetMvc_6x_StyleSheetsControllerLayout_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.StyleSheetsControllerLayout_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("ViewsSharedLayout Template")]
		public string AspNetMvc_6x_ViewsSharedLayout_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ViewsSharedLayout_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("ViewsControllerLayout Template")]
		public string AspNetMvc_6x_ViewsControllerLayout_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ViewsControllerLayout_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("ViewImports Template")]
		public string AspNetMvc_6x_ViewImports_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.ViewImports_Template;

		[Category(AspNetMvc_6x_Category)]
		[DisplayName("Rest Method Action Template")]
		public string AspNetMvc_6x_RestMethod_Action_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_6x_Recipes.RestMethod_Action_Template;
	}
}
