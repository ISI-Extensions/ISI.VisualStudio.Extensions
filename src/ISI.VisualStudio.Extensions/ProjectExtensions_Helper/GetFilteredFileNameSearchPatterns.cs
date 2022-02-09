using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class ProjectExtensions_Helper
	{
		private readonly string[] _filteredFileNameSearchPatterns = new[]
		{
			"**\\*.tt",
			"**\\*.t4",
			"**\\*.@",
			"**\\*.$",
			"**\\*.config"
		};

		public string[] GetFilteredFileNameSearchPatterns() => _filteredFileNameSearchPatterns;
	}
}
