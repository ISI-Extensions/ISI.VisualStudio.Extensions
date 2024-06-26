﻿#region Copyright & License
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
		public const string  MessageBus_Category = "MessageBus Recipes";

		[Category(MessageBus_Category)]
		[DisplayName("Controller Controller Template")]
		public string MessageBus_Controller_Controller_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.MessageBus_Recipes.Controller_Controller_Template;

		[Category(MessageBus_Category)]
		[DisplayName("Controller SubscriptionsRoot Template")]
		public string MessageBus_Controller_SubscriptionsRoot_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.MessageBus_Recipes.Controller_SubscriptionsRoot_Template;

		[Category(MessageBus_Category)]
		[DisplayName("Controller Subscriptions Template")]
		public string MessageBus_Controller_Subscriptions_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.MessageBus_Recipes.Controller_Subscriptions_Template;

		[Category(MessageBus_Category)]
		[DisplayName("Controller Subscriptions With Authentication Template")]
		public string MessageBus_Controller_SubscriptionsWithAuthentication_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.MessageBus_Recipes.Controller_SubscriptionsWithAuthentication_Template;

		[Category(MessageBus_Category)]
		[DisplayName("Method Action Template")]
		public string MessageBus_Method_Action_Template { get; set; } = ISI.VisualStudio.Extensions.RecipeTemplates.MessageBus_Recipes.Controller_Method_Action_Template;
	}
}
