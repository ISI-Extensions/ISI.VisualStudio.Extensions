using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class SolutionExtensions_Helper
	{
		public ISourceControlProvider GetSourceControlProvider(Community.VisualStudio.Toolkit.Solution solution)
		{
			var path = System.IO.Path.GetDirectoryName(solution.FullPath);

			foreach (var sourceControlProvider in SourceControlProviders)
			{
				if (sourceControlProvider.UsesScp(path))
				{
					return sourceControlProvider;
				}
			}

			return null;
		}
	}
}
