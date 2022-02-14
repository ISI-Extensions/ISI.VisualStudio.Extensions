using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class Options
	{
		[Category("Editor Recipes")]
		[DisplayName("Lock Zoom")]
		public bool Editor_Zoom_Lock { get; set; } = true;
	}
}
