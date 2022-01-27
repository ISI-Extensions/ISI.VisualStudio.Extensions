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
	[Command(PackageIds.GuidExtensionsInsertNewGuidMenuItemId)]
	public class GuidExtensions_InsertNewGuid_Command : BaseCommand<GuidExtensions_InsertNewGuid_Command>
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
