#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
	/// Interaction logic for AddEnumTextTemplateDialog.xaml
	/// </summary>

	public partial class AddEnumTextTemplateDialog
	{
		public string EnumName => txtEnumName.Text;
		public string Namespace => txtNamespace.Text;
		public string ConnectionString => txtConnectionString.Text;
		public string EnumTableName => txtEnumTableName.Text;
		public string EnumIdColumnName => txtEnumIdColumnName.Text;
		public string EnumUuidColumnName => txtEnumUuidColumnName.Text;
		public string AliasesDelimiter => txtAliasesDelimiter.Text;
		
		private bool EnumTableNameUnSet { get; set; } = true;
		private bool EnumIdColumnNameUnSet { get; set; } = true;
		private bool EnumUuidColumnNameUnSet { get; set; } = true;

		public AddEnumTextTemplateDialog(string enumName, string @namespace, string connectionString, string enumTableName, string enumIdColumnName, string enumUuidColumnName, string aliasesDelimiter)
		{
			InitializeComponent();

			Title = Vsix.Name;

			txtEnumName.Text = enumName;
			txtNamespace.Text = @namespace;
			txtConnectionString.Text = connectionString;
			txtEnumTableName.Text = enumTableName;
			txtEnumIdColumnName.Text = enumIdColumnName;
			txtEnumUuidColumnName.Text = enumUuidColumnName;
			txtAliasesDelimiter.Text = aliasesDelimiter;

			txtEnumName.TextChanged += (sender, args) =>
			{
				if (EnumTableNameUnSet)
				{
					txtEnumTableName.Text = string.Format("{0}s", txtEnumName.Text);
				}
				if (EnumIdColumnNameUnSet)
				{
					txtEnumIdColumnName.Text = string.Format("{0}Id", txtEnumName.Text);
				}
				if (EnumUuidColumnNameUnSet)
				{
					txtEnumUuidColumnName.Text = string.Format("{0}Uuid", txtEnumName.Text);
				}
			};

			txtEnumTableName.TextChanged += (sender, args) => { EnumTableNameUnSet = string.Equals(txtEnumTableName.Text, string.Format("{0}s", txtEnumName.Text), StringComparison.InvariantCulture); };
			txtEnumIdColumnName.TextChanged += (sender, args) => { EnumIdColumnNameUnSet = string.Equals(txtEnumIdColumnName.Text, string.Format("{0}Id", txtEnumName.Text), StringComparison.InvariantCulture); };
			txtEnumUuidColumnName.TextChanged += (sender, args) => { EnumUuidColumnNameUnSet = string.Equals(txtEnumUuidColumnName.Text, string.Format("{0}Id", txtEnumName.Text), StringComparison.InvariantCulture); };
		}

		private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
