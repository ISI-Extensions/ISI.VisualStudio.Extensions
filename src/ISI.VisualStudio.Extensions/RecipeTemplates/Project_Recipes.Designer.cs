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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ISI.VisualStudio.Extensions.RecipeTemplates.Project_Recipes", typeof(Project_Recipes).Assembly);
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
        ///   Looks up a localized string similar to using System.Reflection;
        ///using System.Runtime.CompilerServices;
        ///using System.Runtime.InteropServices;
        ///
        ///// General Information about an assembly is controlled through the following 
        ///// set of attributes. Change these attribute values to modify the information
        ///// associated with an assembly.
        ///[assembly: AssemblyTitle(&quot;${Namespace}&quot;)]
        ///[assembly: AssemblyDescription(&quot;&quot;)]
        ///[assembly: AssemblyConfiguration(&quot;&quot;)]
        ///[assembly: AssemblyProduct(&quot;${Namespace}&quot;)]
        ///[assembly: AssemblyCulture(&quot;&quot;)].
        /// </summary>
        internal static string AssemblyInfo {
            get {
                return ResourceManager.GetString("AssemblyInfo", resourceCulture);
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
        ///	public class ${ClassNamePrefix}Request
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
        ///	public class ${ClassNamePrefix}Response
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
        internal static string EnumText_Template {
            get {
                return ResourceManager.GetString("EnumText_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class ${RecordName}Manager : ISI.Extensions.Repository.SqlServer.${RecordManager}, I${RecordName}Manager
        ///	{
        ///		public ${RecordName}Manager(
        ///			Microsoft.Extensions.Configuration.IConfiguration configuration,
        ///			Configuration recordMangerConfiguration,
        ///			Microsoft.Extensions.Logging.ILogger logger,
        ///			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
        ///			ISI.Extensions.JsonSerialization.IJsonSerializer serializer)
        ///			: base(configu [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ISI_Extensions_SqlServer_RecordManager_Template {
            get {
                return ResourceManager.GetString("ISI_Extensions_SqlServer_RecordManager_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	[ISI.Extensions.StartUp]
        ///	public class StartUp : ISI.Extensions.IStartUp
        ///	{
        ///		private static bool _isInitialized = false;
        ///		public void Start()
        ///		{
        ///			if (!_isInitialized)
        ///			{
        ///				_isInitialized = true;
        ///
        ///				ISI.Extensions.VirtualFileVolumesFileProvider.RegisterEmbeddedVolume(typeof(StartUp));
        ///			}
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string ISI_Extensions_StartUpClass_Template {
            get {
                return ResourceManager.GetString("ISI_Extensions_StartUpClass_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public partial class ${RecordName}Manager : ISI.Libraries.Repository.SqlServer.${RecordManager}, I${RecordName}Manager
        ///	{
        ///		public ${RecordName}Manager(
        ///			ISI.Libraries.Tracing.ITrace trace,
        ///			ISI.Libraries.IDateTimeStamper dateTimeStamper,
        ///			ISI.Libraries.IJsonSerializer serializer)
        ///			: base(trace, dateTimeStamper, serializer, Configuration.Current.ConnectionString)
        ///		{
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string ISI_Libraries_SqlServer_RecordManager_Template {
            get {
                return ResourceManager.GetString("ISI_Libraries_SqlServer_RecordManager_Template", resourceCulture);
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
        internal static string ISI_Libraries_StartUpClass_Template {
            get {
                return ResourceManager.GetString("ISI_Libraries_StartUpClass_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	[${codeExtensionProvider.Namespace}.Repository.Record(Schema = &quot;XXXXXXX&quot;, TableName = &quot;${RecordName}s&quot;)]
        ///	public class ${RecordName} : ${codeExtensionProvider.Namespace}.Repository.IRecordManagerPrimaryKeyRecord&lt;${PrimaryKeyType}&gt;, ${codeExtensionProvider.Namespace}.Repository.IRecordManagerRecord, ${codeExtensionProvider.Namespace}.Repository.IRecordIndexDescriptions&lt;${RecordName}&gt;
        ///	{
        ///
        ///		[${codeExtensionProvider.Namespace}.Repository.IgnoreRecordProperty]
        ///		${P [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Project_RecordManagerRecord_PrimaryKey_Template {
            get {
                return ResourceManager.GetString("Project_RecordManagerRecord_PrimaryKey_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	[${codeExtensionProvider.Namespace}.Repository.Record(Schema = &quot;XXXXXXX&quot;, TableName = &quot;${RecordName}s&quot;)]
        ///	public class ${RecordName} : ${codeExtensionProvider.Namespace}.Repository.IRecordManagerPrimaryKeyRecord&lt;${PrimaryKeyType}&gt;, ${codeExtensionProvider.Namespace}.Repository.IRecordManagerRecordWithArchiveDateTime, ${codeExtensionProvider.Namespace}.Repository.IRecordIndexDescriptions&lt;${RecordName}&gt;
        ///	{
        ///
        ///		[${codeExtensionProvider.Namespace}.Repository.IgnoreRec [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Project_RecordManagerRecord_PrimaryKeyWithArchive_Template {
            get {
                return ResourceManager.GetString("Project_RecordManagerRecord_PrimaryKeyWithArchive_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	[${codeExtensionProvider.Namespace}.Repository.Record(Schema = &quot;XXXXXXX&quot;, TableName = &quot;${RecordName}s&quot;)]
        ///	public class ${RecordName} : ${codeExtensionProvider.Namespace}.Repository.IRecordManagerRecord, ${codeExtensionProvider.Namespace}.Repository.IRecordIndexDescriptions&lt;${RecordName}&gt;
        ///	{
        ///
        ///		${codeExtensionProvider.Namespace}.Repository.RecordIndexCollection&lt;${RecordName}&gt; ${codeExtensionProvider.Namespace}.Repository.IRecordIndexDescriptions&lt;${RecordName}&gt;.Get [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Project_RecordManagerRecord_Template {
            get {
                return ResourceManager.GetString("Project_RecordManagerRecord_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	public interface I${RecordName}Manager
        ///	{
        ///	}
        ///}.
        /// </summary>
        internal static string RecordManagerInterface_Template {
            get {
                return ResourceManager.GetString("RecordManagerInterface_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ${Usings}
        ///
        ///namespace ${Namespace}
        ///{
        ///	[DataContract]
        ///	public class ${ClassNamePrefix}Request
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
        ///	public class ${ClassNamePrefix}Response
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
        ///
        ///namespace ${Namespace}
        ///{
        ///	[DataContract]
        ///	[${SerialNamespace}.PreferredSerializerJsonDataContract]
        ///	[${SerialNamespace}.SerializerContractUuid(&quot;${ContractUuid}&quot;)]
        ///	public class ${ClassName} : XXXXXX
        ///	{
        ///		public static ${ClassName} ToSerializable(LOCALENTITIES.${ClassName} source)
        ///		{
        ///			return new ${ClassName}()
        ///			{
        ///			};
        ///		}
        ///
        ///		public LOCALENTITIES.${ClassName} Export()
        ///		{
        ///			return new LOCALENTITIES.${ClassName}()
        ///			{
        ///			};
        ///		}
        ///	}
        ///}.
        /// </summary>
        internal static string SerializableObject_Template {
            get {
                return ResourceManager.GetString("SerializableObject_Template", resourceCulture);
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
        ///   Looks up a localized string similar to &lt;#
        ////*
        ///Copyright (c) 2023, Integrated Solutions, Inc.
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
