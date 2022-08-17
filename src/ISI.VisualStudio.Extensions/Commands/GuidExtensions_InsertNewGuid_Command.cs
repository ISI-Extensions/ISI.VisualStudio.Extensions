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
	[Command(PackageIds.GuidExtensions_InsertNewGuid_MenuItemId)]
	public class GuidExtensions_InsertNewGuid_Command : BaseCommand<GuidExtensions_InsertNewGuid_Command>
	{
		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var activeDocumentView = await Community.VisualStudio.Toolkit.VS.Documents.GetActiveDocumentViewAsync();

			var selection = activeDocumentView.TextView?.Selection.SelectedSpans.FirstOrDefault();

			activeDocumentView?.TextBuffer.Replace(selection.Value, string.Format("{0:d}", System.Guid.NewGuid()));
		}
	}
}
