﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensionsHelper : ExtensionsHelper
	{
		protected ISI.Extensions.Nuget.NugetApi NugetApi { get; }

		public NugetExtensionsHelper(ISI.Extensions.Nuget.NugetApi nugetApi)
		{
			NugetApi = nugetApi;
		}
	}
}