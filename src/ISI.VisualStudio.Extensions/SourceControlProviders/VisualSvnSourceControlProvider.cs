using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[SourceControlProvider]
	public class VisualSvnSourceControlProvider : ISourceControlProvider
	{
		public const string SourceControlProviderUuid = "937cffd6-105a-4c00-a044-33ffb48a3b8f";

		Guid ISourceControlProvider.SourceControlProviderUuid => Guid.Parse(SourceControlProviderUuid);
		Guid[] ISourceControlProvider.SourceControlProviderPackageUuids => new[] { Guid.Parse("133240d5-fafa-4868-8fd7-5190a259e676"), Guid.Parse("83F1E506-04BC-4694-9C7D-C55B120E11F0") };

		protected ISI.Extensions.Svn.SvnApi SvnApi { get; }
		protected ISI.Extensions.Scm.ISourceControlClientApi SourceControlClientApi => SvnApi;

		public VisualSvnSourceControlProvider(ISI.Extensions.Svn.SvnApi svnApi)
		{
			SvnApi = svnApi;
		}

		public bool UsesScp(string path)
		{
			return SourceControlClientApi.UsesScc(path);
		}
	}
}
