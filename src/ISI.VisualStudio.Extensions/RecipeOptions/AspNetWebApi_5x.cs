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
		public const string  WebApi_5x_Category = "WebApi 5.x Recipes";

		[Category(WebApi_5x_Category)]
		[DisplayName("Area ControllerRoot Template")]
		public string AspNetWebApi_5x_CArea_ControllerRoot { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.Area_ControllerRoot_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Area BaseModelRoot Template")]
		public string AspNetWebApi_5x_Area_BaseModelRoot { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.Area_BaseModelRoot_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Controller Template")]
		public string AspNetWebApi_5x_Controller_Controller { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.Controller_Controller_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Controller ControllerRoot Template")]
		public string AspNetWebApi_5x_Controller_ControllerRoot { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.Controller_ControllerRoot_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Controller BaseModel Template")]
		public string AspNetWebApi_5x_Controller_BaseModel { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.Controller_BaseModel_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Controller BaseModelRoot Template")]
		public string AspNetWebApi_5x_Controller_BaseModelRoot { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.Controller_BaseModelRoot_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Rest Method Template")]
		public string AspNetWebApi_5x_RestMethod_Method { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.RestMethod_Method_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Action with View Template")]
		public string AspNetWebApi_5x_ActionWithView_Action { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.ActionWithView_Action_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Action With View JavaScript Template")]
		public string AspNetWebApi_5x_ActionWithView_JavaScript { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.ActionWithView_JavaScript_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Action with View Model Template")]
		public string AspNetWebApi_5x_ActionWithView_Model { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.ActionWithView_Model_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Action with View StyleSheet Template")]
		public string AspNetWebApi_5x_ActionWithView_StyleSheet { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.ActionWithView_StyleSheet_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Action with View Template")]
		public string AspNetWebApi_5x_ActionWithView_View { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.ActionWithView_View_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("JavaScripts Controller Layout Template")]
		public string AspNetWebApi_5x_JavaScriptsControllerLayout { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.JavaScriptsControllerLayout_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("StyleSheets Controller Layout Template")]
		public string AspNetWebApi_5x_StyleSheetsControllerLayout { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.StyleSheetsControllerLayout_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Views Controller Layout Template")]
		public string AspNetWebApi_5x_ViewsControllerLayout { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.ViewsControllerLayout_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Action with Partial View Action Template")]
		public string AspNetWebApi_5x_ActionWithPartialView_Action { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.ActionWithPartialView_Action_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Action with Partial View Model Template")]
		public string AspNetWebApi_5x_ActionWithPartialView_Model { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.ActionWithPartialView_Model_Template;

		[Category(WebApi_5x_Category)]
		[DisplayName("Action with Partial View Template")]
		public string AspNetWebApi_5x_ActionWithPartialView_PartialView { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.AspNetWebApi_5x_Recipes.ActionWithPartialView_PartialView_Template;

	}
}
