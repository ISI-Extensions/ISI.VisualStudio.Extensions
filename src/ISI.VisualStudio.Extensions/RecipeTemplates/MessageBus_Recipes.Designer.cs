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
    internal class MessageBus_Recipes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MessageBus_Recipes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ISI.VisualStudio.Extensions.RecipeTemplates.MessageBus_Recipes", typeof(MessageBus_Recipes).Assembly);
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
        ///namespace ${Namespace}.Controllers
        ///{
        ///	[ISI.Extensions.MessageBus.MessageBusController]
        ///	public partial class ${ControllerKey}Controller
        ///	{
        ///		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
        ///
        ///		public ${ControllerKey}Controller(
        ///			Microsoft.Extensions.Logging.ILogger logger)
        ///		{
        ///			Logger = logger;
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
        ///namespace ${Namespace}.Controllers
        ///{
        ///	public partial class ${ControllerKey}Controller 
        ///	{
        ///		public async Task&lt;DTOs.${ControllerActionKey}Response&gt; ${ControllerActionKey}Async(DTOs.${ControllerActionKey}Request request, System.Threading.CancellationToken cancellationToken = default)
        ///		{
        ///			var response = new DTOs.${ControllerActionKey}Response();
        ///
        ///
        ///			return response;
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string Controller_Method_Action_Template {
            get {
                return ResourceManager.GetString("Controller_Method_Action_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class Subscriptions
        ///	{
        ///		public class ${ControllerKey}
        ///		{
        ///			public static ISI.Extensions.MessageBus.IMessageBusBuildRequest GetAddSubscriptions()
        ///			{
        ///				var response = new ISI.Extensions.MessageBus.DefaultMessageBusBuildRequest();
        ///
        ///				response.AddSubscriptions.Add(messageQueueConfigurator =&gt;
        ///				{
        ///					//${Subscriptions}
        ///				});
        ///
        ///				return response;
        ///			}
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string Controller_Subscriptions_Template {
            get {
                return ResourceManager.GetString("Controller_Subscriptions_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class Subscriptions
        ///	{
        ///		public static IEnumerable&lt;ISI.Extensions.MessageBus.IMessageBusBuildRequest&gt; GetAddSubscriptions()
        ///		{
        ///			return new[]
        ///			{
        ///				${ControllerKey}.GetAddSubscriptions(),
        ///			};
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string Controller_SubscriptionsRoot_Template {
            get {
                return ResourceManager.GetString("Controller_SubscriptionsRoot_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class Subscriptions
        ///	{
        ///		public class ${ControllerKey}
        ///		{
        ///			private static ISI.Services.XXXXX.Configuration _configuration = null;
        ///			private static ISI.Services.XXXXX.Configuration Configuration =&gt; _configuration ??= ISI.Extensions.ServiceLocator.Current.GetService&lt;ISI.Services.XXXXX.Configuration&gt;();
        ///
        ///			private static Microsoft.Extensions.Logging.ILogger _logger = null;
        ///			private static Microsoft.Extensions.Logging.ILogger Logger =&gt; _logg [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Controller_SubscriptionsWithAuthentication_Template {
            get {
                return ResourceManager.GetString("Controller_SubscriptionsWithAuthentication_Template", resourceCulture);
            }
        }
    }
}
