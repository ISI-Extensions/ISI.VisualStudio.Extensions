﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISI.VisualStudio.Extensions.RecipeExtensions {
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
    internal class Project_Recipes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Project_Recipes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ISI.VisualStudio.Extensions.RecipeExtensions.Project_Recipes", typeof(Project_Recipes).Assembly);
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
        ///	public partial class ${ClassNamePrefix}Request
        ///	{
        ///
        ///		public System.Threading.CancellationToken CancellationToken { get; set; } = default;
        ///	}
        ///}.
        /// </summary>
        internal static string AsyncDataTransferObjectRequest_Template {
            get {
                return ResourceManager.GetString("AsyncDataTransferObjectRequest_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class ${ClassNamePrefix}Request
        ///	{
        ///	}
        ///}.
        /// </summary>
        internal static string DataTransferObjectRequest_Template {
            get {
                return ResourceManager.GetString("DataTransferObjectRequest_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class ${ClassNamePrefix}Response
        ///	{
        ///	}
        ///}.
        /// </summary>
        internal static string DataTransferObjectResponse_Template {
            get {
                return ResourceManager.GetString("DataTransferObjectResponse_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	[ISI.Libraries.DependencyRegister]
        ///	public class DependencyRegister : ISI.Libraries.IDependencyRegister
        ///	{
        ///		void ISI.Libraries.IDependencyRegister.Register(ISI.Libraries.IDependencyResolver dependencyResolver)
        ///		{
        ///			dependencyResolver.Register&lt;IXXXXXXXXXXXXXXXXXXXXXXXXXXXX, XXXXXXXXXXXXXXXXXXXXXXXXXXXX&gt;(ISI.Libraries.DependencyResolverLifetime.Singleton);
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string DependencyRegisterClass_Template {
            get {
                return ResourceManager.GetString("DependencyRegisterClass_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;#@ template debug=&quot;true&quot; hostSpecific=&quot;true&quot; #&gt;
        ///&lt;#@ output extension=&quot;.generated.cs&quot; #&gt;
        ///&lt;#@ template language=&quot;C#&quot; #&gt;
        ///&lt;#@ assembly name=&quot;System.Core&quot; #&gt;
        ///&lt;#@ Assembly Name=&quot;EnvDTE&quot; #&gt;
        ///&lt;#@ Assembly Name=&quot;System.Data&quot; #&gt;
        ///&lt;#@ import namespace=&quot;EnvDTE&quot; #&gt;
        ///&lt;#@ import namespace=&quot;System.Data&quot; #&gt;
        ///&lt;#@ import namespace=&quot;System.Data.SqlClient&quot; #&gt;
        ///&lt;#@ import namespace=&quot;System.IO&quot; #&gt;
        ///&lt;#@ import namespace=&quot;System.Text.RegularExpressions&quot; #&gt;
        ///&lt;#@ import namespace=&quot;System.Linq&quot; #&gt;
        ///&lt;#
        ///		string enumName = Path.Ge [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EnumTextTemplate_Template {
            get {
                return ResourceManager.GetString("EnumTextTemplate_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	[DataContract]
        ///	public partial class ${ClassNamePrefix}Request
        ///	{
        ///	}
        ///}.
        /// </summary>
        internal static string SerializableDataTransferObjectRequest_Template {
            get {
                return ResourceManager.GetString("SerializableDataTransferObjectRequest_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	[DataContract]
        ///	public partial class ${ClassNamePrefix}Response
        ///	{
        ///	}
        ///}.
        /// </summary>
        internal static string SerializableDataTransferObjectResponse_Template {
            get {
                return ResourceManager.GetString("SerializableDataTransferObjectResponse_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///using Microsoft.Extensions.DependencyInjection;
        ///
        ///namespace ${Namespace}
        ///{
        ///	[ISI.Extensions.DependencyInjection.ServiceRegistrar]
        ///	public class ServiceRegistrar : ISI.Extensions.DependencyInjection.IServiceRegistrar
        ///	{
        ///		public void ServiceRegister(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        ///		{
        ///			services.AddSingleton&lt;IXXXXXXXXXXXXXXXXXXXXXXXXXXXX, XXXXXXXXXXXXXXXXXXXXXXXXXXXX&gt;();
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string ServiceRegistrarClass_Template {
            get {
                return ResourceManager.GetString("ServiceRegistrarClass_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	[ISI.Libraries.StartUp]
        ///	public class StartUp : ISI.Libraries.IStartUp
        ///	{
        ///		private static bool _isInitialized = false;
        ///		public void Start()
        ///		{
        ///			if (!_isInitialized)
        ///			{
        ///				_isInitialized = true;
        ///
        ///				ISI.Libraries.VirtualFileRepositories.Register(typeof(StartUp));
        ///			}
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string StartUpClass_Template {
            get {
                return ResourceManager.GetString("StartUpClass_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;#
        ////*
        ///Copyright (c) 2022, Integrated Solutions, Inc.
        ///All rights reserved.
        ///
        ///Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
        ///
        ///		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
        ///		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string T4LocalContent_Generator_t4_Template {
            get {
                return ResourceManager.GetString("T4LocalContent.Generator.t4_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;#+
        ///		readonly string Namespace = &quot;${Namespace}&quot;;
        ///		
        ///		readonly bool IsWebRoot = ${IsWebRoot};
        ///		readonly bool BuildT4Files = ${BuildT4Files};
        ///		readonly string FilesSubClassName = &quot;T4Files&quot;;
        ///		readonly bool BuildT4Links = ${BuildT4Links};
        ///		readonly string LinksSubClassName = &quot;T4Links&quot;;
        ///		readonly bool BuildT4Embedded = ${BuildT4Embedded};
        ///		readonly string EmbeddedSubClassName = &quot;T4Embedded&quot;;
        ///		readonly bool BuildT4Resources = ${BuildT4Resources};
        ///		readonly string ResourcesSubClassName = &quot;T4Re [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string T4LocalContent_settings_t4_Template {
            get {
                return ResourceManager.GetString("T4LocalContent.settings.t4_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;#@ template language=&quot;C#&quot; debug=&quot;true&quot; hostspecific=&quot;true&quot; #&gt;
        ///&lt;#@ output extension=&quot;.generatedcode.cs&quot; #&gt;
        ///&lt;#@ Include File=&quot;T4LocalContent.settings.t4&quot; #&gt;
        ///&lt;#@ Include File=&quot;T4LocalContent.Generator.t4&quot; #&gt;.
        /// </summary>
        internal static string T4LocalContent_tt_Template {
            get {
                return ResourceManager.GetString("T4LocalContent.tt_Template", resourceCulture);
            }
        }
    }
}
