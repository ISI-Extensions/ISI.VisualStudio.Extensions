﻿#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

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
	[Command(PackageIds.ClipboardExtensions_PasteAsCmdAppend_MenuItemId)]
	public class ClipboardExtensions_PasteAsCmdAppend_Command : BaseCommand<ClipboardExtensions_PasteAsCmdAppend_Command>
	{
		protected override void BeforeQueryStatus(System.EventArgs eventArgs)
		{
			var showCommand = false;

			if (!string.IsNullOrWhiteSpace(System.Windows.Forms.Clipboard.GetText()))
			{
				//var project = VS.Solutions.GetActiveProjectAsync().GetAwaiter().GetResult();

				//showCommand = project.UsesNugetPackage("ISI.Libraries.DB");

				var activeDocumentView = VS.Documents.GetActiveDocumentViewAsync().GetAwaiter().GetResult();

				showCommand = (activeDocumentView?.TextBuffer?.CurrentSnapshot?.GetText() ?? string.Empty).IndexOf("ISI.Libraries.DB") >= 0;
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

				foreach (var line in clipboardText.Split(["\r\n", "\n", "\r"], StringSplitOptions.None))
				{
					formattedText.AppendFormat("cmd.Append(\"{0}\\n\");\r\n", line);
				}

				var selection = activeDocumentView.TextView?.Selection.SelectedSpans.FirstOrDefault();

				activeDocumentView?.TextBuffer.Replace(selection.Value, formattedText.ToString());
			}
		}
	}
}
