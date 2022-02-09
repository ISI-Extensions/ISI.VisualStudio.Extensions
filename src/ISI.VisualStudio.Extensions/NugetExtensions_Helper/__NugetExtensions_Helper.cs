using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensions_Helper : Extensions_Helper
	{
		protected ISI.Extensions.Nuget.NugetApi NugetApi { get; }

		public NugetExtensions_Helper(ISI.Extensions.Nuget.NugetApi nugetApi)
		{
			NugetApi = nugetApi;
		}
	}
}
