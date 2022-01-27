using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public static partial class PackageExtensions
	{
		public static System.IServiceProvider GetServiceProvider(this Microsoft.VisualStudio.Shell.AsyncPackage package)
		{
			return (package as Package)?.ServiceProvider;
		}
	}
}
