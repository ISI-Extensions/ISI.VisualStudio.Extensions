﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISI.VisualStudio.Extensions.RecipeTemplates {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AspNetMvc_5x_Recipes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AspNetMvc_5x_Recipes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ISI.VisualStudio.Extensions.RecipeTemplates.AspNetMvc_5x_Recipes", typeof(AspNetMvc_5x_Recipes).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace.Area}.Controllers
        ///{
        ///	public partial class ${ControllerKey}Controller 
        ///	{
        ///		[AcceptVerbs(HttpVerbs.Post)]
        ///		public virtual ActionResult ${ControllerActionKey}()
        ///		{
        ///			ActionResult result = null;
        ///
        ///			var model = new ${Namespace.Area}.Models.${ControllerKey}.${ControllerActionKey}Model();
        ///
        ///			return result ?? PartialView(${Namespace}.T4Files.${Areas.AreaName}Views.${ControllerKey}.Partials.${ControllerActionKey}_cshtml, model);
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string ActionWithPartialView_Action_Template {
            get {
                return ResourceManager.GetString("ActionWithPartialView_Action_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace.Area}.Models.${ControllerKey}
        ///{
        ///	public class ${ControllerActionKey}Model : BaseModel
        ///	{
        ///
        ///	}
        ///}.
        /// </summary>
        internal static string ActionWithPartialView_Model_Template {
            get {
                return ResourceManager.GetString("ActionWithPartialView_Model_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model ${Namespace.Area}.Models.${ControllerKey}.${ControllerActionKey}Model
        ///.
        /// </summary>
        internal static string ActionWithPartialView_PartialView_Template {
            get {
                return ResourceManager.GetString("ActionWithPartialView_PartialView_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace.Area}.Controllers
        ///{
        ///	public partial class ${ControllerKey}Controller 
        ///	{
        ///		[AcceptVerbs(HttpVerbs.Get)]
        ///		public virtual ActionResult ${ControllerActionKey}()
        ///		{
        ///			ActionResult result = null;
        ///
        ///			var model = new ${Namespace.Area}.Models.${ControllerKey}.${ControllerActionKey}Model();
        ///
        ///			return result ?? View(${Namespace}.T4Files.${Areas.AreaName}Views.${ControllerKey}.${ControllerActionKey}_cshtml, model);
        ///		}
        ///
        ///		[AcceptVerbs(HttpVerbs.Post), ActionName(na [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ActionWithView_Action_Template {
            get {
                return ResourceManager.GetString("ActionWithView_Action_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to jQuery.namespace(&quot;${Namespace.Area}.${ControllerKey}.${ControllerActionKey}&quot;, function(jQuery) {
        ///	var model = {};
        ///	var view = {};
        ///	var controller = {
        ///		setup: function (config) {
        ///			controller.eventBinder();
        ///		},
        ///		eventBinder: function () {
        ///		}
        ///	};
        ///
        ///	return {
        ///		Setup: controller.setup
        ///	};
        ///} (jQuery));
        ///
        ///jQuery(document).ready(function (jQuery) {
        ///	${Namespace.Area}.${ControllerKey}.${ControllerActionKey}.Setup();
        ///});.
        /// </summary>
        internal static string ActionWithView_JavaScript_Template {
            get {
                return ResourceManager.GetString("ActionWithView_JavaScript_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Collections.Generic;
        ///using System.Linq;
        ///using System.Web;
        ///
        ///namespace ${Namespace.Area}.Models.${ControllerKey}
        ///{
        ///	public class ${ControllerActionKey}Model : BaseModel
        ///	{
        ///
        ///	}
        ///}.
        /// </summary>
        internal static string ActionWithView_Model_Template {
            get {
                return ResourceManager.GetString("ActionWithView_Model_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @Html.ImportStyleSheet(${Namespace}.T4Links.StyleSheets._Shared._Definitions_csless).
        /// </summary>
        internal static string ActionWithView_StyleSheet_Template {
            get {
                return ResourceManager.GetString("ActionWithView_StyleSheet_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model ${Namespace.Area}.Models.${ControllerKey}.${ControllerActionKey}Model
        ///
        ///@{
        ///	Model.SetTitle(&quot;${ViewTitle}&quot;);
        ///	Layout = ${Namespace}.T4Files.${Areas.AreaName}Views.${ControllerKey}._Layout_cshtml;
        ///}
        ///
        ///@section StyleSheetIncludes
        ///{
        ///	@Html.AddStyleSheet(${Namespace}.T4Links.${Areas.AreaName}StyleSheets.${ControllerKey}.${ControllerActionKey}_csless)
        ///}
        ///
        ///@section JavaScriptIncludes
        ///{
        ///	@Html.AddJavaScript(${Namespace}.T4Links.${Areas.AreaName}JavaScripts.${ControllerKey}.${ControllerActionKey}_c [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ActionWithView_View_Template {
            get {
                return ResourceManager.GetString("ActionWithView_View_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}.Areas.${AreaName}
        ///{
        ///	public class AreaRegistration : System.Web.Mvc.AreaRegistration
        ///	{
        ///		public override string AreaName { get { return GetType().Namespace; } }
        ///
        ///		public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context)
        ///		{
        ///			Routes.RegisterRoutes(System.Web.Http.GlobalConfiguration.Configuration.Routes);
        ///
        ///			Routes.RegisterRoutes(context.Routes);
        ///
        ///			ModelBinders.RegisterModelBinders(System.Web.Mvc.ModelBinders.Binders);
        ///		}
        ///	}
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Area_AreaRegistration_Template {
            get {
                return ResourceManager.GetString("Area_AreaRegistration_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///namespace ${Namespace.Area}.Models
        ///{
        ///	public abstract class BaseModel : ${Namespace}.Models.BaseModel
        ///	{
        ///	}
        ///}.
        /// </summary>
        internal static string Area_BaseModel_Template {
            get {
                return ResourceManager.GetString("Area_BaseModel_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}.Areas.${AreaName}.Controllers
        ///{
        ///	public abstract partial class Controller : ${Namespace}.Controllers.Controller
        ///	{
        ///		protected Controller(ISI.Libraries.Tracing.ITrace trace)
        ///			: base(trace)
        ///		{
        ///
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string Area_Controller_Template {
            get {
                return ResourceManager.GetString("Area_Controller_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}.Areas.${AreaName}
        ///{
        ///	public partial class Routes
        ///	{
        ///		internal static readonly string UrlRoot = ${Namespace}.Routes.UrlRoot + &quot;${BaseUrl}/&quot;;
        ///
        ///		public static void RegisterRoutes(RouteCollection routes)
        ///		{
        ///			//${Routes}
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string Area_Routes_Template {
            get {
                return ResourceManager.GetString("Area_Routes_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace.Area}.Models.${ControllerKey}
        ///{
        ///	public abstract class BaseModel : Models.BaseModel
        ///	{
        ///	}
        ///}.
        /// </summary>
        internal static string Controller_BaseModel_Template {
            get {
                return ResourceManager.GetString("Controller_BaseModel_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}.Models
        ///{
        ///	public abstract class BaseModel
        ///	{
        ///
        ///	}
        ///}.
        /// </summary>
        internal static string Controller_BaseModelRoot_Template {
            get {
                return ResourceManager.GetString("Controller_BaseModelRoot_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace.Area}.Controllers
        ///{
        ///	public partial class ${ControllerKey}Controller : Controller
        ///	{
        ///		public ${ControllerKey}Controller(ISI.Libraries.Tracing.ITrace trace)
        ///			: base(trace)
        ///		{
        ///
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string Controller_Controller_Template {
            get {
                return ResourceManager.GetString("Controller_Controller_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace.Area}.Controllers
        ///{
        ///	[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
        ///	public abstract partial class Controller : ISI.Libraries.Web.Mvc.AbstractController
        ///	{
        ///		protected Controller(ISI.Libraries.Tracing.ITrace trace)
        ///			: base(trace)
        ///		{
        ///
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string Controller_ControllerRoot_Template {
            get {
                return ResourceManager.GetString("Controller_ControllerRoot_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace.Area}
        ///{
        ///	public partial class Routes
        ///	{
        ///		public partial class ${ControllerKey}
        ///		{
        ///			#pragma warning disable 649
        ///			public class RouteNames : IRouteNames
        ///			{
        ///				//${RouteNames}
        ///			}
        ///			#pragma warning restore 649
        ///
        ///			static ${ControllerKey}()
        ///			{
        ///				(new RouteNames()).AssignRouteNames();
        ///			}
        ///
        ///			internal static readonly string UrlRoot = Routes.UrlRoot${RouteUrl};
        ///
        ///			internal static void RegisterRoutes(RouteCollection routes)
        ///			{
        ///				//${Route [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Controller_Routes_Template {
            get {
                return ResourceManager.GetString("Controller_Routes_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace.Area}
        ///{
        ///	public partial class Routes
        ///	{
        ///		internal static readonly string UrlRoot = string.Empty;
        ///
        ///		public static void RegisterRoutes(RouteCollection routes)
        ///		{
        ///			routes.IgnoreRoute(&quot;{resource}.axd/{*pathInfo}&quot;);
        ///			routes.IgnoreRoute(&quot;{*favicon}&quot;, new { favicon = @&quot;(.*/)?favicon.(ico|png)(/.*)?&quot; });
        ///
        ///			//${Routes}
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string Controller_RoutesRoot_Template {
            get {
                return ResourceManager.GetString("Controller_RoutesRoot_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to jQuery.namespace(&quot;${Namespace.Area}.${ControllerKey}.Layout&quot;, function(jQuery) {
        ///	var model = {};
        ///	var view = {};
        ///	var controller = {
        ///		setup: function (config) {
        ///			controller.eventBinder();
        ///		},
        ///		eventBinder: function () {
        ///		}
        ///	};
        ///
        ///	return {
        ///		Setup: controller.setup
        ///	};
        ///} (jQuery));
        ///
        ///jQuery(document).ready(function (jQuery) {
        ///	${Namespace.Area}.${ControllerKey}.Layout.Setup();
        ///});.
        /// </summary>
        internal static string JavaScriptsControllerLayout_Template {
            get {
                return ResourceManager.GetString("JavaScriptsControllerLayout_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to jQuery.namespace(&quot;${Namespace}.${Areas.AreaName}Layout&quot;, function(jQuery) {
        ///	var model = {};
        ///	var view = {};
        ///	var controller = {
        ///		setup: function (config) {
        ///			controller.eventBinder();
        ///		},
        ///		eventBinder: function () {
        ///		}
        ///	};
        ///
        ///	return {
        ///		Setup: controller.setup
        ///	};
        ///} (jQuery));
        ///
        ///jQuery(document).ready(function (jQuery) {
        ///	${Namespace}.${Areas.AreaName}Layout.Setup();
        ///});.
        /// </summary>
        internal static string JavaScriptsSharedLayout_Template {
            get {
                return ResourceManager.GetString("JavaScriptsSharedLayout_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;configuration&gt;
        ///	&lt;configSections&gt;
        ///		&lt;sectionGroup name=&quot;system.web.webPages.razor&quot; type=&quot;System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35&quot;&gt;
        ///			&lt;section name=&quot;host&quot; type=&quot;System.Web.WebPages.Razor.Configuration.HostSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35&quot; requirePermission=&quot;false&quot; /&gt;
        ///			&lt;section name=&quot;pages&quot; type=&quot;Sy [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JavaScriptsWebConfig_Template {
            get {
                return ResourceManager.GetString("JavaScriptsWebConfig_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace.Area}.Controllers
        ///{
        ///	public partial class ${ControllerKey}Controller 
        ///	{
        ///		[AcceptVerbs(HttpVerbs.Post)]
        ///		public virtual ActionResult ${ControllerActionKey}()
        ///		{
        ///			ActionResult result = null;
        ///
        ///			var model = new ${Namespace.Area}.Models.${ControllerKey}.${ControllerActionKey}Model();
        ///
        ///			return result;
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string RestMethod_Action_Template {
            get {
                return ResourceManager.GetString("RestMethod_Action_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace.Area}.Models.${ControllerKey}
        ///{
        ///	public class ${ControllerActionKey}Model : BaseModel
        ///	{
        ///
        ///	}
        ///}.
        /// </summary>
        internal static string RestMethod_Model_Template {
            get {
                return ResourceManager.GetString("RestMethod_Model_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @Html.ImportStyleSheet(${Namespace}.T4Links.StyleSheets._Shared._Definitions_csless).
        /// </summary>
        internal static string StyleSheetsControllerLayout_Template {
            get {
                return ResourceManager.GetString("StyleSheetsControllerLayout_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @Html.ImportStyleSheet(${Namespace}.T4Links.StyleSheets._Shared._Definitions_csless).
        /// </summary>
        internal static string StyleSheetsSharedLayout_Template {
            get {
                return ResourceManager.GetString("StyleSheetsSharedLayout_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;configuration&gt;
        ///	&lt;configSections&gt;
        ///		&lt;sectionGroup name=&quot;system.web.webPages.razor&quot; type=&quot;System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35&quot;&gt;
        ///			&lt;section name=&quot;host&quot; type=&quot;System.Web.WebPages.Razor.Configuration.HostSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35&quot; requirePermission=&quot;false&quot; /&gt;
        ///			&lt;section name=&quot;pages&quot; type=&quot;Sy [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string StyleSheetsWebConfig_Template {
            get {
                return ResourceManager.GetString("StyleSheetsWebConfig_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model ${Namespace.Area}.Models.${ControllerKey}.BaseModel
        ///
        ///@{
        ///	Layout = ${Namespace}.T4Files.${Areas.AreaName}Views._Shared._Layout_cshtml;
        ///}
        ///
        ///@section StyleSheetIncludes
        ///{
        ///	@Html.AddStyleSheet(${Namespace}.T4Links.${Areas.AreaName}StyleSheets.${ControllerKey}._Layout_csless)
        ///	@RenderSection(&quot;StyleSheetIncludes&quot;, false)
        ///}
        ///
        ///@RenderBody()
        ///
        ///@section JavaScriptIncludes
        ///{
        ///	@Html.AddJavaScript(${Namespace}.T4Links.${Areas.AreaName}JavaScripts.${ControllerKey}._Layout_csjs)
        ///	@RenderSection(&quot;JavaS [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ViewsControllerLayout_Template {
            get {
                return ResourceManager.GetString("ViewsControllerLayout_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model ${Namespace.Area}.Models.BaseModel
        ///
        ///@{
        ///	Layout = ${Namespace}.T4Files.Views._Shared._Layout_cshtml;
        ///}
        ///
        ///@section StyleSheetIncludes
        ///{
        ///	@Html.AddStyleSheet(${Namespace}.T4Links.${Areas.AreaName}StyleSheets._Shared._Layout_csless)
        ///	@RenderSection(&quot;StyleSheetIncludes&quot;, false)
        ///}
        ///
        ///@RenderBody()
        ///
        ///@section JavaScriptIncludes
        ///{
        ///	@Html.AddJavaScript(${Namespace}.T4Links.${Areas.AreaName}JavaScripts._Shared._Layout_csjs)
        ///	@RenderSection(&quot;JavaScriptIncludes&quot;, false)
        ///}
        ///
        ///@section JavaScriptCont [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ViewsSharedLayout_Template {
            get {
                return ResourceManager.GetString("ViewsSharedLayout_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;configuration&gt;
        ///	&lt;configSections&gt;
        ///		&lt;sectionGroup name=&quot;system.web.webPages.razor&quot; type=&quot;System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35&quot;&gt;
        ///			&lt;section name=&quot;host&quot; type=&quot;System.Web.WebPages.Razor.Configuration.HostSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35&quot; requirePermission=&quot;false&quot; /&gt;
        ///			&lt;section name=&quot;pages&quot; type=&quot;Sy [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ViewsWebConfig_Template {
            get {
                return ResourceManager.GetString("ViewsWebConfig_Template", resourceCulture);
            }
        }
    }
}
