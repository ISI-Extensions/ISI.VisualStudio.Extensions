using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[SourceControlProvider]
	public class GitSourceControlProvider : ISourceControlProvider
	{
		public const string SourceControlProviderUuid = "11b8e6d7-c08b-4385-b321-321078cdd1f8";
		
		Guid ISourceControlProvider.SourceControlProviderUuid => Guid.Parse(SourceControlProviderUuid);
		Guid[] ISourceControlProvider.SourceControlProviderPackageUuids => new[] { Guid.Parse("7fe30a77-37f9-4cf2-83dd-96b207028e1b") };

		protected ISI.Extensions.Git.GitApi GitApi { get; }
		protected ISI.Extensions.Scm.ISourceControlClientApi SourceControlClientApi => GitApi;

		public GitSourceControlProvider(ISI.Extensions.Git.GitApi gitApi)
		{
			GitApi = gitApi;
		}

		public bool UsesScp(string path)
		{
			return SourceControlClientApi.UsesScc(path);
		}
	}
}
