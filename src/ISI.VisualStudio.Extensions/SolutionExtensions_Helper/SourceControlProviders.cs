using System;
using System.Linq;
using ISI.Extensions.Extensions;
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class SolutionExtensions_Helper
	{
		private static readonly object _sourceControlProvidersLock = new object();
		private static ISourceControlProvider[] _sourceControlProviders = null;
		public ISourceControlProvider[] SourceControlProviders
		{
			get
			{
				if (_sourceControlProviders == null)
				{
					lock (_sourceControlProvidersLock)
					{
						if (_sourceControlProviders == null)
						{
							_sourceControlProviders = ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementations<ISourceControlProvider>(ISI.Extensions.ServiceLocator.Current);
						}
					}
				}

				return _sourceControlProviders;
			}
		}
	}
}
