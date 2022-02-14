using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[System.ComponentModel.Composition.Export(typeof(Microsoft.VisualStudio.Text.Editor.IWpfTextViewCreationListener))]
	[Microsoft.VisualStudio.Utilities.ContentType(Community.VisualStudio.Toolkit.ContentTypes.Text)]
	[Microsoft.VisualStudio.Text.Editor.TextViewRole(Microsoft.VisualStudio.Text.Editor.PredefinedTextViewRoles.Zoomable)]
	internal class WpfTextViewCreationListener : Microsoft.VisualStudio.Text.Editor.IWpfTextViewCreationListener
	{
		public void TextViewCreated(Microsoft.VisualStudio.Text.Editor.IWpfTextView textView)
		{
			textView.Options.SetOptionValue(Microsoft.VisualStudio.Text.Editor.DefaultWpfViewOptions.EnableMouseWheelZoomId, Options.Instance.Editor_Zoom_Lock);
		}
	}
}