using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class CakeExtensionsHelper : ExtensionsHelper
	{
		protected ISI.Extensions.Cake.CakeApi CakeApi { get; }

		public CakeExtensionsHelper(ISI.Extensions.Cake.CakeApi cakeApi)
		{
			CakeApi = cakeApi;
		}
	}
}
