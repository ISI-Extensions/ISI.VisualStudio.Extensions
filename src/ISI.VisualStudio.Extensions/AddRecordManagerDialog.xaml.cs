﻿#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.RpcContracts.Commands;

namespace ISI.VisualStudio.Extensions
{
	/// <summary>
	/// Interaction logic for AddRecordManagerDialog.xaml
	/// </summary>

	public partial class AddRecordManagerDialog
	{
		public string NewRecordManagerName => txtNewRecordManagerName.Text.Replace(" ", string.Empty);
		public string ContractProjectDescription => cboContractProject.SelectedValue as string;
		public string PrimaryKeyType => cboPrimaryKeyType.SelectedValue as string;
		public bool HasArchive => !string.IsNullOrWhiteSpace(PrimaryKeyType) && chkHasArchive.IsChecked.GetValueOrDefault();

		protected System.Collections.Generic.IDictionary<string, ISI.VisualStudio.Extensions.Extensions.SolutionExtensions.ProjectDescription> ProjectLookUp { get; }

		public AddRecordManagerDialog(System.Collections.Generic.IEnumerable<ISI.VisualStudio.Extensions.Extensions.SolutionExtensions.ProjectDescription> projectDescriptions, ISI.VisualStudio.Extensions.Extensions.SolutionExtensions.ProjectDescription contractProject)
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

			cboPrimaryKeyType.Items.Add(string.Empty);
			cboPrimaryKeyType.Items.Add("Guid");
			cboPrimaryKeyType.Items.Add("int");
			cboPrimaryKeyType.Items.Add("string");

			cboPrimaryKeyType.SelectionChanged += Update;

			Update(null, null);

			txtNewRecordManagerName.Focus();
		}
		
		private void Update(object sender, object args)
		{
			chkHasArchive.IsEnabled = !string.IsNullOrWhiteSpace(PrimaryKeyType);
		}

		private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
