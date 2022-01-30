#region Copyright & License
/*
Copyright (c) 2018, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISI.VisualStudio.Extensions
{
	public partial class EmbeddedFileNamesForm : Form
	{
		public IDictionary<string, bool> FileNames = null;

		public EmbeddedFileNamesForm(IDictionary<string, bool> fileNames)
		{
			InitializeComponent();

			this.Icon = new Icon(this.GetType().Assembly.GetManifestResourceStream(T4Resources.Lantern_ico));
			this.ControlBox = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.ShowIcon = true;

			this.Load += EmbeddedFileNamesForm_Load;
			this.btnApply.Click += ApplySettings_Click;
			this.btnCancel.Click += Cancel_Click;

			FileNames = fileNames;
		}

		public const string CHECKBOX_PREFIX = "cboFileName_";

		private void EmbeddedFileNamesForm_Load(object sender, EventArgs e)
		{
			foreach (var fileName in FileNames)
			{
				var cboCheckBox = new CheckBox()
				{
					Text = fileName.Key,
					Name = string.Format("{0}{1}", CHECKBOX_PREFIX, fileName.Key),
					Checked = fileName.Value,
					Tag = fileName.Key
				};

				this.flpFileNames.Controls.Add(cboCheckBox);
			}
		}

		private void ApplySettings_Click(object sender, EventArgs e)
		{
			foreach (Control control in this.flpFileNames.Controls)
			{
				if ((control is CheckBox checkBox) && control.Name.StartsWith(CHECKBOX_PREFIX, StringComparison.CurrentCultureIgnoreCase))
				{
					FileNames[(string)control.Tag] = checkBox.Checked;
				}
			}

			// cleanup 
			if (this.Modal)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
			else
			{
				this.Close();
			}
		}


		private void Cancel_Click(object sender, EventArgs e)
		{
			// no logic needed here when shown as a dialog, thanks to form wiring
			if (!this.Modal)
			{
				this.Close();
			}
		}
	}
}
