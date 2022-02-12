using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_AspNetMvc_6x_Helper
	{
		private Community.VisualStudio.Toolkit.OutputWindowPane _outputWindowPane = null;

		public async System.Threading.Tasks.Task<Community.VisualStudio.Toolkit.OutputWindowPane> GetOutputWindowPaneAsync()
		{
			return _outputWindowPane ??= await Community.VisualStudio.Toolkit.VS.Windows.CreateOutputWindowPaneAsync("ISI.VisualStudio.Extensions.RecipeExtensions_AspNetMvc_6x");
		}
	}
}
