using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	/// <summary>
	/// Interaction logic for AddProjectPropertiesDialog.xaml
	/// </summary>

	public partial class AddProjectPropertiesDialog
	{
		public bool? Deterministic => (cboDeterministic.SelectedValue as string).ToBooleanNullable();
		public bool? LangVersionLatest => (cboLangVersion.SelectedValue as string).Replace("Latest", "true").ToBooleanNullable();
		public bool? GenerateAssemblyInfo => (cboGenerateAssemblyInfo.SelectedValue as string).ToBooleanNullable();
		public string RuntimeIdentifiers
		{
			get
			{
				var runtimeIdentifiers = new List<string>();

				if (chkRuntimeIdentifiers_win.IsChecked.GetValueOrDefault())
				{
					runtimeIdentifiers.Add("win");
				}
				if (chkRuntimeIdentifiers_win_x86.IsChecked.GetValueOrDefault())
				{
					runtimeIdentifiers.Add("win-x86");
				}
				if (chkRuntimeIdentifiers_win_x64.IsChecked.GetValueOrDefault())
				{
					runtimeIdentifiers.Add("win-x64");
				}

				return string.Join(";", runtimeIdentifiers);
			}
		}
		public string UseSharedAssemblyInfo => cboUseSharedAssemblyInfo.SelectedValue as string;
		public string UseSharedVersion => cboUseSharedVersion.SelectedValue as string;
		public bool AddAssemblyInfo => chkAddAssemblyInfo.IsChecked.GetValueOrDefault();
		public string UseSharedLicenseHeader => cboUseSharedLicenseHeader.SelectedValue as string;

		public AddProjectPropertiesDialog(bool? deterministic, bool? langVersionLatest, bool? generateAssemblyInfo, string runtimeIdentifiers,
			IEnumerable<string> sharedAssemblyInfos, string useSharedAssemblyInfo,
			IEnumerable<string> sharedVersions, string useSharedVersion,
			bool addAssemblyInfo,
			IEnumerable<string> sharedLicenseHeaders, string useSharedLicenseHeader)
		{
			InitializeComponent();

			Title = Vsix.Name;

			cboDeterministic.SelectedValue = (deterministic.HasValue ? deterministic.Value.TrueFalse(false, BooleanExtensions.TextCase.Lower) : "Remove");
			cboLangVersion.SelectedValue = (langVersionLatest.HasValue ? (langVersionLatest.Value ? "Latest" : "Remove") : string.Empty);
			cboGenerateAssemblyInfo.SelectedValue = (generateAssemblyInfo.HasValue ? generateAssemblyInfo.Value.TrueFalse(false, BooleanExtensions.TextCase.Lower) : "Remove");

			var hasRuntimeIdentifiers = new HashSet<string>((runtimeIdentifiers ?? string.Empty).Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries), StringComparer.InvariantCultureIgnoreCase);
			chkRuntimeIdentifiers_win.IsChecked = hasRuntimeIdentifiers.Contains("win");
			chkRuntimeIdentifiers_win_x86.IsChecked = hasRuntimeIdentifiers.Contains("win-x86");
			chkRuntimeIdentifiers_win_x64.IsChecked = hasRuntimeIdentifiers.Contains("win-x64");

			cboUseSharedAssemblyInfo.Items.Add(string.Empty);
			foreach (var sharedAssemblyInfo in (sharedAssemblyInfos ?? Array.Empty<string>()))
			{
				cboUseSharedAssemblyInfo.Items.Add(sharedAssemblyInfo);
			}
			cboUseSharedAssemblyInfo.SelectedValue = useSharedAssemblyInfo ?? string.Empty;

			cboUseSharedVersion.Items.Add(string.Empty);
			foreach (var sharedVersion in (sharedVersions ?? Array.Empty<string>()))
			{
				cboUseSharedVersion.Items.Add(sharedVersion);
			}
			cboUseSharedVersion.SelectedValue = useSharedVersion ?? string.Empty;

			chkAddAssemblyInfo.IsChecked = addAssemblyInfo;

			cboUseSharedLicenseHeader.Items.Add(string.Empty);
			foreach (var sharedLicenseHeader in (sharedLicenseHeaders ?? Array.Empty<string>()))
			{
				cboUseSharedLicenseHeader.Items.Add(sharedLicenseHeader);
			}
			cboUseSharedLicenseHeader.SelectedValue = useSharedLicenseHeader ?? string.Empty;
		}

		private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
