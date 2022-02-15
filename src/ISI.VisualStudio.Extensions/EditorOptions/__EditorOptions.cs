using System.Runtime.InteropServices;

namespace ISI.VisualStudio.Extensions
{
	internal partial class OptionsProvider
	{
		// Register the options with these attributes on your package class:
		[ComVisible(true)]
		public class EditorOptionsPage : Community.VisualStudio.Toolkit.BaseOptionPage<EditorOptions> { }
	}

	public partial class EditorOptions : Community.VisualStudio.Toolkit.BaseOptionModel<EditorOptions>
	{

	}
}
