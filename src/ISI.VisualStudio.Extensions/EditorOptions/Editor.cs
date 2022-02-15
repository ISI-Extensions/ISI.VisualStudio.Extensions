using System.ComponentModel;

namespace ISI.VisualStudio.Extensions
{
	public partial class EditorOptions
	{
		public const string Editor_Category = "Editor Recipes";

		[Category(Editor_Category)]
		[DisplayName("Lock Zoom")]
		public bool Editor_Zoom_Lock { get; set; } = true;
	}
}
