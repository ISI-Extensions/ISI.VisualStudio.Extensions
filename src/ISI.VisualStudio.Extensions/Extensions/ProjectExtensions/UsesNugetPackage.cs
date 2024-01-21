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

using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public static partial class ProjectExtensions
	{
		public static System.Collections.Generic.Dictionary<string, string> ISIExtensionsMessageBusTypes = new()
		{
			{ "ISI.Extensions.MessageBus.AzureServiceBus", "MessageBus.AzureServiceBus" },
			{ "ISI.Extensions.MessageBus.MassTransit", "MassTransit" },
			{ "ISI.Extensions.MessageBus.MassTransit.InMemory", "MassTransit.InMemory" },
			{ "ISI.Extensions.MessageBus.MassTransit.RabbitMQ", "MassTransit.RabbitMQ" },
			{ "ISI.Extensions.MessageBus.Redis", "MessageBus.Redis" },
		};

		public static System.Collections.Generic.Dictionary<string, string> ISIExtensionsRepositoryTypes = new()
		{
			{ "ISI.Extensions.Repository.Cassandra", "Cassandra" },
			{ "ISI.Extensions.Repository.Cosmos", "Cosmos" },
			{ "ISI.Extensions.Repository.Oracle", "Oracle" },
			{ "ISI.Extensions.Repository.PostgreSQL", "PostgreSQL" },
			{ "ISI.Extensions.Repository.RavenDb", "RavenDb" },
			{ "ISI.Extensions.Repository.SqlServer", "SqlServer" },
		};

		public static System.Collections.Generic.Dictionary<string, string> ISILibrariesRepositoryTypes = new()
		{
			{ "ISI.Libraries.Repository.Cosmos", "Cosmos" },
			{ "ISI.Libraries.Repository.DynamoDB", "DynamoDB" },
			{ "ISI.Libraries.Repository.FileSystem", "FileSystem" },
			{ "ISI.Libraries.Repository.HBase.Phoenix", "HBase.Phoenix" },
			{ "ISI.Libraries.Repository.Oracle", "Oracle" },
			{ "ISI.Libraries.Repository.RavenDB", "RavenDB" },
			{ "ISI.Libraries.Repository.SqlServer", "SqlServer" },
		};

		public static bool UsesISIExtensionsAspNetCore(this Community.VisualStudio.Toolkit.Project project) => UsesAnyNugetPackage(project, "ISI.Extensions.AspNetCore", "ISI.Platforms.AspNetCore");

		public static bool UsesISIExtensionsRepository(this Community.VisualStudio.Toolkit.Project project) => UsesAnyNugetPackage(project, ISIExtensionsRepositoryTypes.Keys);

		public static bool UsesISIExtensionsMessageBus(this Community.VisualStudio.Toolkit.Project project) => UsesAnyNugetPackage(project, ISIExtensionsMessageBusTypes.Keys);

		public static bool UsesISILibrariesWebMvc(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Libraries.Web.Mvc");

		public static bool UsesISILibrariesJQueryWebMvc(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Libraries.JQuery.Web.Mvc");

		public static bool UsesISICmsWebMvc(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Cms.Web.Mvc");

		public static bool UsesISILibrariesBootstrapWebMvc(this Community.VisualStudio.Toolkit.Project project) => UsesNugetPackage(project, "ISI.Libraries.Bootstrap.Web.Mvc");

		public static bool UsesISILibrariesRepository(this Community.VisualStudio.Toolkit.Project project) => UsesAnyNugetPackage(project, ISILibrariesRepositoryTypes.Keys);

		public static bool UsesNugetPackage(this Community.VisualStudio.Toolkit.Project project, string packageName) => UsesAnyNugetPackage(project, new[] { packageName });

		public static bool UsesAnyNugetPackage(this Community.VisualStudio.Toolkit.Project project, System.Collections.Generic.IEnumerable<string> packageNames) => UsesAnyNugetPackage(project, packageNames.ToArray());

		public static bool UsesAnyNugetPackage(this Community.VisualStudio.Toolkit.Project project, params string[] packageNames)
		{
			if (project != null)
			{
				var referenceNames = project.References.ToNullCheckedHashSet(reference => reference.Name, NullCheckCollectionResult.Empty);

				var content = System.IO.File.ReadAllText(project.FullPath);

				foreach (var packageName in packageNames)
				{
					if (referenceNames.Contains(packageName))
					{
						return true;
					}

					if (content.IndexOf(string.Format("\"{0}", packageName)) >= 0)
					{
						return true;
					}

					if (content.IndexOf(string.Format("\\{0}", packageName)) >= 0)
					{
						return true;
					}
				}
			}

			return false;
		}

		public static string UsesWhichRepositoryType(this Community.VisualStudio.Toolkit.Project project)
		{
			if (project != null)
			{
				var referenceNames = project.References.ToNullCheckedHashSet(reference => reference.Name, NullCheckCollectionResult.Empty);

				foreach (var repositoryTypes in new[] { ISIExtensionsRepositoryTypes, ISILibrariesRepositoryTypes })
				{
					foreach (var repositoryType in repositoryTypes)
					{
						if (referenceNames.Contains(repositoryType.Key))
						{
							return repositoryType.Value;
						}

						var content = System.IO.File.ReadAllText(project.FullPath);

						if (content.IndexOf(string.Format("\"{0}", repositoryType.Key)) >= 0)
						{
							return repositoryType.Value;
						}

						if (content.IndexOf(string.Format("\\{0}", repositoryType.Key)) >= 0)
						{
							return repositoryType.Value;
						}
					}
				}
			}

			return null;
		}
	}
}
