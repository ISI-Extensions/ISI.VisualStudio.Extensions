using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class JenkinsExtensions_Helper : Extensions_Helper
	{
		protected ISI.Extensions.Jenkins.JenkinsApi JenkinsApi { get; }

		public JenkinsExtensions_Helper(
			ISI.Extensions.Nuget.NugetApi nugetApi,
			ISI.Extensions.Jenkins.JenkinsApi jenkinsApi)
			: base(nugetApi)
		{
			JenkinsApi = jenkinsApi;
		}
	}
}
