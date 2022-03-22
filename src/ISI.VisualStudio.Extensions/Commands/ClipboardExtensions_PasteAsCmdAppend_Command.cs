using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.ClipboardExtensionsPasteAsCmdAppendMenuItemId)]
	public class ClipboardExtensions_PasteAsCmdAppend_Command : BaseCommand<ClipboardExtensions_PasteAsCmdAppend_Command>
	{
		protected override void BeforeQueryStatus(System.EventArgs eventArgs)
		{
			var showCommand = false;

			if (!string.IsNullOrWhiteSpace(System.Windows.Forms.Clipboard.GetText()))
			{
				var project = VS.Solutions.GetActiveProjectAsync().GetAwaiter().GetResult();

				showCommand = project.UsesNugetPackage("ISI.Libraries.DB");
			}

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			var clipboardText = System.Windows.Forms.Clipboard.GetText();

			if (!string.IsNullOrWhiteSpace(clipboardText))
			{
				var activeDocumentView = await VS.Documents.GetActiveDocumentViewAsync();

				var formattedText = new System.Text.StringBuilder();

				foreach (var line in clipboardText.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None))
				{
					formattedText.AppendFormat("cmd.Append(\"{0}\\n\");\r\n", line);
				}

				var selection = activeDocumentView.TextView?.Selection.SelectedSpans.FirstOrDefault();

				activeDocumentView?.TextBuffer.Replace(selection.Value, formattedText.ToString());
			}
		}
	}
}
