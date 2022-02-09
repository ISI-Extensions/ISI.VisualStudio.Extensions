using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensions_Helper
	{
		public System.Collections.Generic.IEnumerable<ISI.Extensions.Nuget.NugetPackageKey> ParsePackageConfig(string[] packageConfigLines)
		{
			var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();

			foreach (var line in packageConfigLines)
			{
				if (line.Trim(' ', '\t').StartsWith("<package ", StringComparison.InvariantCultureIgnoreCase))
				{
					try
					{
						var keyValues = line.Replace("<package ", string.Empty).Replace("/>", string.Empty).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Split(new[] { "=\"", "\"" }, StringSplitOptions.None)).ToDictionary(item => item[0].Trim(' ', '\t'), item => item[1].Trim(' ', '\t'), StringComparer.InvariantCultureIgnoreCase);

						var nugetPackageKey = new ISI.Extensions.Nuget.NugetPackageKey();

						var value = string.Empty;

						if (keyValues.TryGetValue("id", out value))
						{
							nugetPackageKey.Package = value;
						}

						if (keyValues.TryGetValue("version", out value))
						{
							nugetPackageKey.Version = value;
						}

						//if (keyValues.TryGetValue("targetFramework", out value))
						//{
						//	nugetPackageKey.TargetFramework = value;
						//}

						//if (keyValues.TryGetValue("allowedVersions", out value))
						//{
						//	nugetPackageKey.AllowedVersions = value;
						//}

						nugetPackageKeys.TryAdd(nugetPackageKey);
					}
					catch
					{

					}
				}
			}

			return nugetPackageKeys;
		}
	}
}