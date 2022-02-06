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
