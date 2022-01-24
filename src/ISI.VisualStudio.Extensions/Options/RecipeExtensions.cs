using System.Runtime.InteropServices;

namespace ISI.VisualStudio.Extensions
{
	internal partial class OptionsProvider
	{
		// Register the options with these attributes on your package class:
		[ComVisible(true)]
		public class RecipeExtensionsOptionsPage : Community.VisualStudio.Toolkit.BaseOptionPage<RecipeExtensionsOptions> { }
	}

	public partial class RecipeExtensionsOptions : Community.VisualStudio.Toolkit.BaseOptionModel<RecipeExtensionsOptions>
	{

	}
}
