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
    internal class ProjectPartialClass_Recipes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ProjectPartialClass_Recipes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ISI.VisualStudio.Extensions.RecipeTemplates.ProjectPartialClass_Recipes", typeof(ProjectPartialClass_Recipes).Assembly);
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
        ///namespace ${Namespace}
        ///{
        ///	public partial class ${ClassName}
        ///	{
        ///		public async Task&lt;DTOs.${MethodName}Response&gt; ${MethodName}Async(DTOs.${MethodName}Request request, System.Threading.CancellationToken cancellationToken = default)
        ///		{
        ///			var response = new DTOs.${MethodName}Response();
        ///			
        ///			
        ///			return response;
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string AsyncPartialClassMethod_Template {
            get {
                return ResourceManager.GetString("AsyncPartialClassMethod_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class ${ClassName}
        ///	{
        ///		private async Task ${MethodName}Async(System.Threading.CancellationToken cancellationToken = default)
        ///		{
        ///
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string AsyncPartialClassPrivateMethod_Template {
            get {
                return ResourceManager.GetString("AsyncPartialClassPrivateMethod_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class ${ClassName} : I${ClassName}
        ///	{
        ///${ClassInjectorProperties}
        ///		public ${ClassName}(${ClassInjectors})
        ///		{
        ///${ClassInjectorAssignments}
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string PartialClass_Template {
            get {
                return ResourceManager.GetString("PartialClass_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public interface ${InterfaceName}
        ///	{
        ///
        ///	}
        ///}.
        /// </summary>
        internal static string PartialClassInterface_Template {
            get {
                return ResourceManager.GetString("PartialClassInterface_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class ${ClassName}
        ///	{
        ///		public DTOs.${MethodName}Response ${MethodName}(DTOs.${MethodName}Request request)
        ///		{
        ///			var response = new DTOs.${MethodName}Response();
        ///			
        ///			
        ///			return response;
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string PartialClassMethod_Template {
            get {
                return ResourceManager.GetString("PartialClassMethod_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class ${ClassName}
        ///	{
        ///		private void ${MethodName}()
        ///		{
        ///
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string PartialClassPrivateMethod_Template {
            get {
                return ResourceManager.GetString("PartialClassPrivateMethod_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class ${ClassName}
        ///	{
        ///		public partial class ${SubClassName}
        ///		{
        ///
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string PartialClassSubClass_Template {
            get {
                return ResourceManager.GetString("PartialClassSubClass_Template", resourceCulture);
            }
        }
    }
}
