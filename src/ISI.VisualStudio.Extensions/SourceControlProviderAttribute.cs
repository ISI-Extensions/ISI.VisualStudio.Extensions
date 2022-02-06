using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public class SourceControlProviderAttribute : ISI.Extensions.TypeLocatorAttribute
	{
		public SourceControlProviderAttribute()
			: base(typeof(ISourceControlProvider))
		{
		}
	}
}