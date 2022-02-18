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

	public partial class ProjectPropertiesDialog
	{
		public bool? Deterministic => (cboDeterministic.SelectedValue as string).ToBooleanNullable();
		public bool? LangVersionLatest => (cboLangVersion.SelectedValue as string).Replace("Latest", "true").ToBooleanNullable();
		public bool? GenerateAssemblyInfo => (cboGenerateAssemblyInfo.SelectedValue as string).ToBooleanNullable();
		public bool? Nullable => (cboNullable.SelectedValue as string).ToBooleanNullable();
		public bool? ImplicitUsings => (cboImplicitUsings.SelectedValue as string).ToBooleanNullable();
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

		protected ProjectProperties DefaultProjectProperties { get; }

		public ProjectPropertiesDialog(IEnumerable<string> sharedAssemblyInfos, IEnumerable<string> sharedVersions, IEnumerable<string> sharedLicenseHeaders, ProjectProperties currentProjectProperties, ProjectProperties defaultProjectProperties)
		{
			InitializeComponent();

			Title = Vsix.Name;

			cboDeterministic.Items.Add("Remove");
			cboDeterministic.Items.Add("true");
			cboDeterministic.Items.Add("false");

			cboLangVersion.Items.Add(string.Empty);
			cboLangVersion.Items.Add("Remove");
			cboLangVersion.Items.Add("Latest");

			cboGenerateAssemblyInfo.Items.Add("Remove");
			cboGenerateAssemblyInfo.Items.Add("true");
			cboGenerateAssemblyInfo.Items.Add("false");

			cboNullable.Items.Add(string.Empty);
			cboNullable.Items.Add("Remove");
			cboNullable.Items.Add("Enable");

			cboImplicitUsings.Items.Add(string.Empty);
			cboImplicitUsings.Items.Add("Remove");
			cboNullable.Items.Add("Enable");

			cboUseSharedAssemblyInfo.Items.Add(string.Empty);
			foreach (var sharedAssemblyInfo in (sharedAssemblyInfos ?? Array.Empty<string>()))
			{
				cboUseSharedAssemblyInfo.Items.Add(sharedAssemblyInfo);
			}

			cboUseSharedVersion.Items.Add(string.Empty);
			foreach (var sharedVersion in (sharedVersions ?? Array.Empty<string>()))
			{
				cboUseSharedVersion.Items.Add(sharedVersion);
			}

			cboUseSharedLicenseHeader.Items.Add(string.Empty);
			foreach (var sharedLicenseHeader in (sharedLicenseHeaders ?? Array.Empty<string>()))
			{
				cboUseSharedLicenseHeader.Items.Add(sharedLicenseHeader);
			}

			SetValues(currentProjectProperties);

			DefaultProjectProperties = defaultProjectProperties;
		}

		public void SetValues(ProjectProperties projectProperties)
		{
			cboDeterministic.SelectedValue = (projectProperties.Deterministic.HasValue ? projectProperties.Deterministic.Value.TrueFalse(false, BooleanExtensions.TextCase.Lower) : "Remove");

			cboLangVersion.SelectedValue = (projectProperties.LangVersionLatest.HasValue ? (projectProperties.LangVersionLatest.Value ? "Latest" : "Remove") : string.Empty);

			cboGenerateAssemblyInfo.SelectedValue = (projectProperties.GenerateAssemblyInfo.HasValue ? projectProperties.GenerateAssemblyInfo.Value.TrueFalse(false, BooleanExtensions.TextCase.Lower) : "Remove");

			cboNullable.SelectedValue = (projectProperties.Nullable.HasValue ? (projectProperties.Nullable.Value ? "Enable" : "Remove") : string.Empty);

			cboImplicitUsings.SelectedValue = (projectProperties.ImplicitUsings.HasValue ? (projectProperties.ImplicitUsings.Value ? "Enable" : "Remove") : string.Empty);

			var hasRuntimeIdentifiers = new HashSet<string>((projectProperties.RuntimeIdentifiers ?? string.Empty).Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries), StringComparer.InvariantCultureIgnoreCase);
			chkRuntimeIdentifiers_win.IsChecked = hasRuntimeIdentifiers.Contains("win");
			chkRuntimeIdentifiers_win_x86.IsChecked = hasRuntimeIdentifiers.Contains("win-x86");
			chkRuntimeIdentifiers_win_x64.IsChecked = hasRuntimeIdentifiers.Contains("win-x64");

			cboUseSharedAssemblyInfo.SelectedValue = projectProperties.UseSharedAssemblyInfo ?? string.Empty;

			cboUseSharedVersion.SelectedValue = projectProperties.UseSharedVersion ?? string.Empty;

			chkAddAssemblyInfo.IsChecked = projectProperties.AddAssemblyInfo;

			cboUseSharedLicenseHeader.SelectedValue = projectProperties.UseSharedLicenseHeader ?? string.Empty;
		}

		private void btnDefault_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			SetValues(DefaultProjectProperties);
		}

		private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
