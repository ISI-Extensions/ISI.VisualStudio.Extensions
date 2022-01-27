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
	[Command(PackageIds.ClipboardExtensionsPasteAsMenuItemId)]
	public class ClipboardExtensions_PasteAs_Command : BaseCommand<ClipboardExtensions_PasteAs_Command>
	{
		private static ISI.Extensions.VisualStudio.CodeGenerationApi _codeGenerationApi = null;
		protected ISI.Extensions.VisualStudio.CodeGenerationApi CodeGenerationApi => _codeGenerationApi ??= Package.GetServiceProvider().GetService<ISI.Extensions.VisualStudio.CodeGenerationApi>();

		protected override void BeforeQueryStatus(System.EventArgs eventArgs)
		{
			Command.Visible = System.Windows.Forms.Clipboard.ContainsText();

			base.BeforeQueryStatus(eventArgs);
		}

		private ISI.Extensions.VisualStudio.ClassDefinition ParseClipboardForProperties()
		{
			var clipboardText = System.Windows.Forms.Clipboard.GetText();
			if (clipboardText.Length != 0)
			{
				return CodeGenerationApi.ParseClassDefinition(new ISI.Extensions.VisualStudio.DataTransferObjects.CodeGenerationApi.ParseClassDefinitionRequest()
				{
					Definition = clipboardText,
				}).ClassDefinition;
			}

			return null;
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			var project = await VS.Solutions.GetActiveProjectAsync();

			var activeDocumentView = await VS.Documents.GetActiveDocumentViewAsync();

			var pasteAsDialog = new PasteAsDialog();

			var pasteAsDialogResult = await pasteAsDialog.ShowDialogAsync();

			if (pasteAsDialogResult.GetValueOrDefault())
			{
				if (pasteAsDialog.PasteAsProperties)
				{
					var generateClassDefinitionDialog = new GenerateClassDefinitionDialog(CodeGenerationApi, project, ParseClipboardForProperties());

					var generateClassDefinitionDialogShowDialogResult = await generateClassDefinitionDialog.ShowDialogAsync();

					if (generateClassDefinitionDialogShowDialogResult.GetValueOrDefault())
					{
						var selection = activeDocumentView.TextView?.Selection.SelectedSpans.FirstOrDefault();

						activeDocumentView?.TextBuffer.Replace(selection.Value, generateClassDefinitionDialog.PasteText);

						if (GenerateClassDefinitionDialog.IncludeDataContractAttributes != ISI.Extensions.VisualStudio.IncludePropertyAttribute.No)
						{
							//check to add System.Runtime.Serialization
						}
					}
				}

				if (pasteAsDialog.PasteAsConversion)
				{
					var generateClassDefinitionConversionDialog = new GenerateClassDefinitionConversionDialog(CodeGenerationApi, project, ParseClipboardForProperties());

					var generateClassDefinitionDialogShowDialogResult = await generateClassDefinitionConversionDialog.ShowDialogAsync();

					if (generateClassDefinitionDialogShowDialogResult.GetValueOrDefault())
					{
						var selection = activeDocumentView.TextView?.Selection.SelectedSpans.FirstOrDefault();

						activeDocumentView?.TextBuffer.Replace(selection.Value, generateClassDefinitionConversionDialog.PasteText);
					}
				}
			}
		}
	}
}
