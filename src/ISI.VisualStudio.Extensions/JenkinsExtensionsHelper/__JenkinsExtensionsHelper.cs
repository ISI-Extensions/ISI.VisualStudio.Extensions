using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class JenkinsExtensionsHelper : ExtensionsHelper
	{
		protected ISI.Extensions.Jenkins.JenkinsApi JenkinsApi { get; }

		public JenkinsExtensionsHelper(ISI.Extensions.Jenkins.JenkinsApi jenkinsApi)
		{
			JenkinsApi = jenkinsApi;
		}
	}
}
