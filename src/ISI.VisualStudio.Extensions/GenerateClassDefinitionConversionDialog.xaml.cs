#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using ISI.VisualStudio.Extensions;
using System.Windows;

namespace ISI.VisualStudio.Extensions
{
	/// <summary>
	/// Interaction logic for GenerateClassDefinitionConversionDialog.xaml
	/// </summary>

	public partial class GenerateClassDefinitionConversionDialog
	{
		internal enum GenerateClassDefinitionConversionDialogConversionPattern
		{
			Constructor,
			ConstructorExport,
			Request,
			Assignment,
			Custom,
		}

		private ISI.Extensions.VisualStudio.CodeGenerationApi CodeGenerationApi { get; set; }

		public Community.VisualStudio.Toolkit.Project Project { get; }
		public ISI.Extensions.VisualStudio.ClassDefinition ClassDefinition { get; }

		public string PasteText => txtPreview.Text;
		private bool ProcessUpdatePreview { get; set; }

		private static ISI.Extensions.VisualStudio.StringCaseFormat FormatPropertyName { get; set; } = ISI.Extensions.VisualStudio.StringCaseFormat.No;

		private static string CustomTargetEntityName { get; set; } = "target";
		private static string CustomSourceEntityName { get; set; } = "source";
		private static string CustomConversionSeparator { get; set; } = ";";

		private static GenerateClassDefinitionConversionDialogConversionPattern ConversionPattern { get; set; } = GenerateClassDefinitionConversionDialogConversionPattern.Constructor;

		public GenerateClassDefinitionConversionDialog(
			ISI.Extensions.VisualStudio.CodeGenerationApi codeGenerationApi,
			Community.VisualStudio.Toolkit.Project project,
			ISI.Extensions.VisualStudio.ClassDefinition classDefinition)
		{
			CodeGenerationApi = codeGenerationApi;

			Project = project;
			ClassDefinition = classDefinition;

			ProcessUpdatePreview = false;

			InitializeComponent();

			txtTargetEntityNameCustom.Text = CustomTargetEntityName;
			txtSourceEntityNameCustom.Text = CustomSourceEntityName;
			txtConversionSeparatorCustom.Text = CustomConversionSeparator;

			ProcessUpdatePreview = true;

			UpdatePreview();
		}

		private void rdoConversionPatternConstructor_OnChecked(object sender, RoutedEventArgs e)
		{
			ConversionPattern = GenerateClassDefinitionConversionDialogConversionPattern.Constructor;

			UpdatePreview();
		}

		private void rdoConversionPatternConstructorExport_OnChecked(object sender, RoutedEventArgs e)
		{
			ConversionPattern = GenerateClassDefinitionConversionDialogConversionPattern.ConstructorExport;

			UpdatePreview();
		}

		private void rdoConversionPatternRequest_OnChecked(object sender, RoutedEventArgs e)
		{
			ConversionPattern = GenerateClassDefinitionConversionDialogConversionPattern.Request;

			UpdatePreview();
		}

		private void rdoConversionPatternAssignment_OnChecked(object sender, RoutedEventArgs e)
		{
			ConversionPattern = GenerateClassDefinitionConversionDialogConversionPattern.Assignment;

			UpdatePreview();
		}

		private void rdoConversionPatternCustom_OnChecked(object sender, RoutedEventArgs e)
		{
			ConversionPattern = GenerateClassDefinitionConversionDialogConversionPattern.Custom;

			UpdatePreview();
		}

		private void txtTargetEntityNameCustom_OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			CustomTargetEntityName = txtTargetEntityNameCustom.Text;

			UpdatePreview();
		}

		private void txtSourceEntityNameCustom_OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			CustomSourceEntityName = txtSourceEntityNameCustom.Text;

			UpdatePreview();
		}

		private void txtConversionSeparatorCustom_OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			CustomConversionSeparator = txtConversionSeparatorCustom.Text;

			UpdatePreview();
		}


		private void UpdatePreview()
		{
			if (ProcessUpdatePreview)
			{
				gpxConversionPatternConstructor.Visibility = (ConversionPattern == GenerateClassDefinitionConversionDialogConversionPattern.Constructor ? Visibility.Visible : Visibility.Hidden);
				gpxConversionPatternConstructorExport.Visibility = (ConversionPattern == GenerateClassDefinitionConversionDialogConversionPattern.ConstructorExport ? Visibility.Visible : Visibility.Hidden);
				gpxConversionPatternRequest.Visibility = (ConversionPattern == GenerateClassDefinitionConversionDialogConversionPattern.Request ? Visibility.Visible : Visibility.Hidden);
				gpxConversionPatternAssignment.Visibility = (ConversionPattern == GenerateClassDefinitionConversionDialogConversionPattern.Assignment ? Visibility.Visible : Visibility.Hidden);
				gpxConversionPatternCustom.Visibility = (ConversionPattern == GenerateClassDefinitionConversionDialogConversionPattern.Custom ? Visibility.Visible : Visibility.Hidden);

				string targetEntityName;
				string sourceEntityName;
				string conversionSeparator;

				switch (ConversionPattern)
				{
					case GenerateClassDefinitionConversionDialogConversionPattern.Constructor:
						targetEntityName = null;
						sourceEntityName = "source";
						conversionSeparator = ",";
						break;

					case GenerateClassDefinitionConversionDialogConversionPattern.ConstructorExport:
						targetEntityName = null;
						sourceEntityName = null;
						conversionSeparator = ",";
						break;

					case GenerateClassDefinitionConversionDialogConversionPattern.Request:
						targetEntityName = null;
						sourceEntityName = "request";
						conversionSeparator = ",";
						break;

					case GenerateClassDefinitionConversionDialogConversionPattern.Assignment:
						targetEntityName = "target";
						sourceEntityName = "source";
						conversionSeparator = ";";
						break;

					case GenerateClassDefinitionConversionDialogConversionPattern.Custom:
						targetEntityName = CustomTargetEntityName;
						sourceEntityName = CustomSourceEntityName;
						conversionSeparator = CustomConversionSeparator;
						break;

					default:
						throw new System.ArgumentOutOfRangeException();
				}


				txtPreview.Text = CodeGenerationApi.GenerateClassDefinitionConversion(new ISI.Extensions.VisualStudio.DataTransferObjects.CodeGenerationApi.GenerateClassDefinitionConversionRequest()
				{
					CodeExtensionProviderUuid = Project.GetCodeExtensionProvider().CodeExtensionProviderUuid,
					ClassDefinition = ClassDefinition,
					FormatPropertyName = FormatPropertyName,
					TargetEntityName = targetEntityName,
					SourceEntityName = sourceEntityName,
					ConversionSeparator = conversionSeparator,
				}).Content;
			}
		}

		private void btnPaste_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
