using System;
using System.Linq;
using System.Windows;

namespace ISI.VisualStudio.Extensions
{
	/// <summary>
	/// Interaction logic for AddPartialClassMethodDialog.xaml
	/// </summary>

	public partial class AddPartialClassMethodDialog
	{
		protected string PartialClassName { get; }
		public string NewPartialClassMethodName => txtNewPartialClassMethodName.Text.Replace(" ", string.Empty);
		public string ContractProjectDescription => cboContractProject.SelectedValue as string;
		public bool AddDTOs => chkAddDTOs.IsChecked.GetValueOrDefault();

		protected System.Collections.Generic.IDictionary<string, ISI.VisualStudio.Extensions.Extensions.SolutionExtensions.ProjectDescription> ProjectLookUp { get; }

		public AddPartialClassMethodDialog(string partialClassName, System.Collections.Generic.IEnumerable<ISI.VisualStudio.Extensions.Extensions.SolutionExtensions.ProjectDescription> projectDescriptions, ISI.VisualStudio.Extensions.Extensions.SolutionExtensions.ProjectDescription contractProject)
		{
			InitializeComponent();

			Title = Vsix.Name;

			PartialClassName = partialClassName;

			ProjectLookUp = projectDescriptions.ToDictionary(projectDescription => projectDescription.Description, projectDescription => projectDescription);

			foreach (var projectDescription in ProjectLookUp.Values.OrderBy(projectDescription => projectDescription.Description, StringComparer.InvariantCultureIgnoreCase))
			{
				cboContractProject.Items.Add(projectDescription.Description);

				if (string.Equals(contractProject?.Project?.FullPath ?? string.Empty, projectDescription.Project.FullPath, StringComparison.InvariantCultureIgnoreCase))
				{
					cboContractProject.SelectedValue = projectDescription.Description;
				}
			}

			chkAddDTOs.IsChecked = true;

			txtNewPartialClassMethodName.TextChanged += Update;
			cboContractProject.SelectionChanged += Update;

			txtNewPartialClassMethodName.Focus();
		}

		private void Update(object sender, object args)
		{
			ProjectLookUp.TryGetValue(cboContractProject.SelectedValue as string ?? string.Empty, out var projectDescription);

			var contractRootNamespace = projectDescription?.RootNamespace ?? string.Empty;
			
			txtAddDTOs.Text = string.Format("{0}.DataTransferObjects.{1}.{2}", contractRootNamespace, PartialClassName, NewPartialClassMethodName);

			if (AddDTOs && NewPartialClassMethodName.EndsWith("RecordManager", StringComparison.InvariantCultureIgnoreCase))
			{
				chkAddDTOs.IsChecked = false;
			}
		}

		private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
