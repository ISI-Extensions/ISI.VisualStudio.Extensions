using System.Runtime.InteropServices;

namespace ISI.VisualStudio.Extensions
{
	internal partial class OptionsProvider
	{
		// Register the options with these attributes on your package class:
		[ComVisible(true)]
		public class OptionsPage : Community.VisualStudio.Toolkit.BaseOptionPage<Options> { }
	}

	public partial class Options : Community.VisualStudio.Toolkit.BaseOptionModel<Options>
	{

	}
}
