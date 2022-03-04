using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public class EmbeddedFileName
	{
		public string FileName  { get; set; }
		public bool Active  { get; set; }
	}
	/// <summary>
	/// Interaction logic for EmbeddedFileNamesDialog.xaml
	/// </summary>

	public partial class EmbeddedFileNamesDialog
	{
		public EmbeddedFileNamesDialog(EmbeddedFileName[] embeddedFileNames)
		{
			InitializeComponent();

			Title = Vsix.Name;

			gridTest.ItemsSource = embeddedFileNames;
		}

		private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
