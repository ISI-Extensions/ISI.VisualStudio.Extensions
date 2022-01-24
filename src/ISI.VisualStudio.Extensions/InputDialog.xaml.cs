using ISI.VisualStudio.Extensions.Extensions;
using System.Windows;

namespace ISI.VisualStudio.Extensions
{
	/// <summary>
	/// Interaction logic for InputDialog.xaml
	/// </summary>

	public partial class InputDialog
	{
		public string Value => txtAnswer.Text;

		public InputDialog(
			string question,
			string defaultValue = null)
		{
			InitializeComponent();

			Title = Vsix.Name;

			lblQuestion.Text = question;

			txtAnswer.Text = defaultValue ?? string.Empty;

			txtAnswer.Focus();
		}

		private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
