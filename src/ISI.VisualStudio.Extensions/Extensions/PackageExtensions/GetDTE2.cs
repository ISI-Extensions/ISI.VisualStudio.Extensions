using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public static partial class PackageExtensions
	{
		public static EnvDTE80.DTE2 GetDTE2(this Microsoft.VisualStudio.Shell.AsyncPackage package)
		{
			return (package as Package)?.DTE2;
		}
	}
}
