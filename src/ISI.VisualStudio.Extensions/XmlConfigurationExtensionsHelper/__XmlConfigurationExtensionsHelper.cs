using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class XmlConfigurationExtensionsHelper : ExtensionsHelper
	{
		protected ISI.Extensions.VisualStudio.XmlTransformApi XmlTransformApi { get; }

		public XmlConfigurationExtensionsHelper(ISI.Extensions.VisualStudio.XmlTransformApi xmlTransformApi)
		{
			XmlTransformApi = xmlTransformApi;
		}
	}
}
