using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public interface ISourceControlProvider
	{
		public Guid SourceControlProviderUuid { get; }
		public Guid[] SourceControlProviderPackageUuids { get; }

		public bool UsesScp(string path);
	}
}
