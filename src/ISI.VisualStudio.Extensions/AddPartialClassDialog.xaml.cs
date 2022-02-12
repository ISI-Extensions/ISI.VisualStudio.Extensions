using System;
using System.Linq;
using System.Windows;

namespace ISI.VisualStudio.Extensions
{
	/// <summary>
	/// Interaction logic for AddPartialClassDialog.xaml
	/// </summary>

	public partial class AddPartialClassDialog
	{
		public string NewPartialClassName => txtNewPartialClassName.Text.Replace(" ", string.Empty);
		public string ContractProjectDescription => cboContractProject.SelectedValue as string;
		public bool AddInterface => chkAddInterface.IsChecked.GetValueOrDefault();
		public bool AddDTOsFolder => chkAddDTOsFolder.IsChecked.GetValueOrDefault();
		public bool AddIocRegistry => chkAddIocRegistry.IsChecked.GetValueOrDefault();

		protected System.Collections.Generic.IDictionary<string, ISI.VisualStudio.Extensions.Extensions.SolutionExtensions.ProjectDescription> ProjectLookUp { get; }

		public AddPartialClassDialog(System.Collections.Generic.IEnumerable<ISI.VisualStudio.Extensions.Extensions.SolutionExtensions.ProjectDescription> projectDescriptions, ISI.VisualStudio.Extensions.Extensions.SolutionExtensions.ProjectDescription contractProject)
		{
			InitializeComponent();

			Title = Vsix.Name;

			ProjectLookUp = projectDescriptions.ToDictionary(projectDescription => projectDescription.Description, projectDescription => projectDescription);

			foreach (var projectDescription in ProjectLookUp.Values.OrderBy(projectDescription => projectDescription.Description, StringComparer.InvariantCultureIgnoreCase))
			{
				cboContractProject.Items.Add(projectDescription.Description);

				if (string.Equals(contractProject?.Project?.FullPath ?? string.Empty, projectDescription.Project.FullPath, StringComparison.InvariantCultureIgnoreCase))
				{
					cboContractProject.SelectedValue = projectDescription.Description;
				}
			}

			chkAddInterface.IsChecked = true;
			chkAddDTOsFolder.IsChecked = true;
			chkAddIocRegistry.IsChecked = true;

			txtNewPartialClassName.TextChanged += Update;
			cboContractProject.SelectionChanged += Update;

			txtNewPartialClassName.Focus();
		}

		private void Update(object sender, object args)
		{
			ProjectLookUp.TryGetValue(cboContractProject.SelectedValue as string ?? string.Empty, out var projectDescription);

			var contractRootNamespace = projectDescription?.RootNamespace ?? string.Empty;
			
			txtInterface.Text = string.Format("{0}.I{1}", contractRootNamespace, NewPartialClassName);
			txtAddDTOsFolder.Text = string.Format("{0}.DataTransferObjects.{1}", contractRootNamespace, NewPartialClassName);
		}

		private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
