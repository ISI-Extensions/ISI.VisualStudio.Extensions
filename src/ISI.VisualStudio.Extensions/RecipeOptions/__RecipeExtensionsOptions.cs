using System.Runtime.InteropServices;

namespace ISI.VisualStudio.Extensions
{
	internal partial class OptionsProvider
	{
		// Register the options with these attributes on your package class:
		[ComVisible(true)]
		public class RecipeOptionsPage : Community.VisualStudio.Toolkit.BaseOptionPage<RecipeOptions> { }
	}

	public partial class RecipeOptions : Community.VisualStudio.Toolkit.BaseOptionModel<RecipeOptions>
	{

	}
}
