using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.GuidExtensionsInsertNewGuidMenuItemId)]
	internal sealed class InsertNewGuidCommand : BaseCommand<InsertNewGuidCommand>
	{
		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var activeDocumentView = await Community.VisualStudio.Toolkit.VS.Documents.GetActiveDocumentViewAsync();
			var position = activeDocumentView.TextView?.Selection.Start.Position.Position;

			if (position.HasValue)
			{
				activeDocumentView.TextBuffer.Insert(position.Value, string.Format("{0:d}", System.Guid.NewGuid()));
			}
		}
	}
}
