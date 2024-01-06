#region Copyright & License
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
 
using System;
using System.Windows;

namespace ISI.VisualStudio.Extensions
{
	/// <summary>
	/// Interaction logic for AddT4LocalContentDialog.xaml
	/// </summary>

	public partial class AddT4LocalContentDialog
	{
		public bool IsWebRoot
		{
			get => chkIsWebRoot.IsChecked.GetValueOrDefault();
			set => chkIsWebRoot.IsChecked = value;
		}
		public bool BuildT4Files
		{
			get => chkBuildT4Files.IsChecked.GetValueOrDefault();
			set => chkBuildT4Files.IsChecked = value;
		}
		public bool BuildT4Links
		{
			get => chkBuildT4Links.IsChecked.GetValueOrDefault();
			set => chkBuildT4Links.IsChecked = value;
		}
		public bool BuildT4Embedded
		{
			get => chkBuildT4Embedded.IsChecked.GetValueOrDefault();
			set => chkBuildT4Embedded.IsChecked = value;
		}
		public bool BuildT4Resources
		{
			get => chkBuildT4Resources.IsChecked.GetValueOrDefault();
			set => chkBuildT4Resources.IsChecked = value;
		}

		public AddT4LocalContentDialog(bool isWebRoot, bool buildT4Files, bool buildT4Links, bool buildT4Embedded, bool buildT4Resources)
		{
			InitializeComponent();

			Title = Vsix.Name;

			IsWebRoot = isWebRoot;
			BuildT4Files = buildT4Files;
			BuildT4Links = buildT4Links;
			BuildT4Embedded = buildT4Embedded;
			BuildT4Resources = buildT4Resources;
		}

		private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
