using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class CakeExtensions_Helper : Extensions_Helper
	{
		protected ISI.Extensions.Cake.CakeApi CakeApi { get; }

		public CakeExtensions_Helper(ISI.Extensions.Cake.CakeApi cakeApi)
		{
			CakeApi = cakeApi;
		}
	}
}
