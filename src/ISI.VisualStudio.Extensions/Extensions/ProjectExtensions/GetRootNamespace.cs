using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public static partial class ProjectExtensions
	{
		public static string GetRootNamespace(this Community.VisualStudio.Toolkit.Project project)
		{
			var csProj = System.IO.File.ReadAllText(project.FullPath);

			var csProjXml = System.Xml.Linq.XElement.Parse(csProj);

			var sdkAttribute = csProjXml.GetAttributeByLocalName("Sdk")?.Value ?? string.Empty;

			if (!sdkAttribute.StartsWith("Microsoft.NET", StringComparison.InvariantCultureIgnoreCase))
			{
				foreach (var propertyGroup in csProjXml.GetElementsByLocalName("PropertyGroup"))
				{
					foreach (var rootNamespace in propertyGroup.GetElementsByLocalName("RootNamespace"))
					{
						return rootNamespace.Value;
					}
				}
			}

			return System.IO.Path.GetFileNameWithoutExtension(project.FullPath);
		}
	}
}
