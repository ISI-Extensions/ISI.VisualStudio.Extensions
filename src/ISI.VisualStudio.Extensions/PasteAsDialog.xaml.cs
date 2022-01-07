using ISI.Extensions.Extensions;
using ISI.VisualStudio.Extensions.Extensions;
using System.Windows;

namespace ISI.VisualStudio.Extensions
{
	/// <summary>
	/// Interaction logic for PasteAsDialog.xaml
	/// </summary>
	public partial class PasteAsDialog
	{
		public bool PasteAsProperties { get; set; }
		public bool PasteAsConversion { get; set; }

		public PasteAsDialog()
		{
			InitializeComponent();
		}

		private void btnProperties_Click(object sender, RoutedEventArgs e)
		{
			PasteAsProperties = true;
			DialogResult = true;
		}

		private void btnConversion_Click(object sender, RoutedEventArgs e)
		{
			PasteAsConversion = true;
			DialogResult = true;
		}
	}
}
