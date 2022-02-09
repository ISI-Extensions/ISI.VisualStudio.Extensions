using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensions_Helper
	{
		public System.Collections.Generic.IEnumerable<ISI.Extensions.Nuget.NugetPackageKey> ParseNugetPackageKeyClipped(string[] packageClippedLines)
		{
			var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();

			foreach (var line in packageClippedLines)
			{
				if (ISI.Extensions.Nuget.NugetPackageKey.TryParseClipboardToken(line, out var nugetPackageKey))
				{
					nugetPackageKeys.TryAdd(nugetPackageKey);
				}
			}

			return nugetPackageKeys;
		}
	}
}