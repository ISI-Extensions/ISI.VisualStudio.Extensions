using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensions_Helper
	{
		public System.Collections.Generic.IEnumerable<ISI.Extensions.Nuget.NugetPackageKey> ParseCsProj(string[] csProjLines)
		{
			var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();

			foreach (var line in csProjLines)
			{
				if (line.Trim(' ', '\t').StartsWith("<PackageReference ", StringComparison.InvariantCultureIgnoreCase))
				{
					try
					{
						var keyValues = line.Replace("<PackageReference ", string.Empty).Replace("/>", string.Empty).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Split(new[] { "=\"", "\"" }, StringSplitOptions.None)).ToDictionary(item => item[0].Trim(' ', '\t'), item => item[1].Trim(' ', '\t'), StringComparer.InvariantCultureIgnoreCase);

						var nugetPackageKey = new ISI.Extensions.Nuget.NugetPackageKey();

						var value = string.Empty;

						if (keyValues.TryGetValue("Include", out value))
						{
							nugetPackageKey.Package = value;
						}

						if (keyValues.TryGetValue("Update", out value))
						{
							nugetPackageKey.Package = value;
						}

						if (keyValues.TryGetValue("Version", out value))
						{
							nugetPackageKey.Version = value;
						}

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