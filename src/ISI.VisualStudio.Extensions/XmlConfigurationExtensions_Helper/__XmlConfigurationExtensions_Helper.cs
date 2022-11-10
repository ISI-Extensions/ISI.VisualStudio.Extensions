using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class XmlConfigurationExtensions_Helper : Extensions_Helper
	{
		protected ISI.Extensions.VisualStudio.XmlTransformApi XmlTransformApi { get; }

		public XmlConfigurationExtensions_Helper(
			ISI.Extensions.VisualStudio.XmlTransformApi xmlTransformApi)
		{
			XmlTransformApi = xmlTransformApi;
		}
	}
}
