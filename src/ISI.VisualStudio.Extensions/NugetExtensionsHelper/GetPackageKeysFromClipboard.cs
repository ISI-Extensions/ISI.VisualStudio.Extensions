using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensionsHelper
	{
		public ISI.Extensions.Nuget.NugetPackageKey[] GetPackageKeysFromClipboard()
		{
			var nugetPackageKeys = new System.Collections.Generic.List<ISI.Extensions.Nuget.NugetPackageKey>();

			if (System.Windows.Forms.Clipboard.ContainsText())
			{
				var clipboardText = System.Windows.Forms.Clipboard.GetText();
				if (clipboardText.Length != 0)
				{
					var lines = clipboardText.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).ToArray();

					nugetPackageKeys.AddRange(ParseNugetPackageKeyClipped(lines));
					nugetPackageKeys.AddRange(ParsePackageConfig(lines));
					nugetPackageKeys.AddRange(ParseCsProj(lines));

					return nugetPackageKeys.ToArray();
				}
			}

			return nugetPackageKeys.ToArray();
		}
	}
}
