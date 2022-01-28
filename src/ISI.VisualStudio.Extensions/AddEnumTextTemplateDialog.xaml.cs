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
