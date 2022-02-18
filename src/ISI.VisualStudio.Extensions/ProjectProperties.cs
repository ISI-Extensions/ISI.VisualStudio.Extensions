using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public class ProjectProperties
	{
		public bool? Deterministic { get; set; }
		public bool? LangVersionLatest { get; set; }
		public bool? GenerateAssemblyInfo { get; set; }
		public bool? Nullable { get; set; }
		public bool? ImplicitUsings { get; set; }
		public string RuntimeIdentifiers { get; set; }
		public string UseSharedAssemblyInfo { get; set; }
		public string UseSharedVersion { get; set; }
		public bool AddAssemblyInfo { get; set; }
		public string UseSharedLicenseHeader { get; set; }
	}
}
