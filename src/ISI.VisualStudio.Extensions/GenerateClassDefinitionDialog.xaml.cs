using ISI.Extensions.Extensions;
using ISI.VisualStudio.Extensions;
using System.Windows;

namespace ISI.VisualStudio.Extensions
{
	/// <summary>
	/// Interaction logic for GenerateClassDefinitionDialog.xaml
	/// </summary>
	public partial class GenerateClassDefinitionDialog
	{
		private ISI.Extensions.VisualStudio.CodeGenerationApi CodeGenerationApi { get; set; }

		public Community.VisualStudio.Toolkit.Project Project { get; }
		public ISI.Extensions.VisualStudio.ClassDefinition ClassDefinition { get; }

		public string PasteText => txtPreview.Text;
		private bool ProcessUpdatePreview { get; set; }

		public static ISI.Extensions.VisualStudio.StringCaseFormat FormatPropertyName { get; private set; } = ISI.Extensions.VisualStudio.StringCaseFormat.No;
		public static ISI.Extensions.VisualStudio.IncludePropertyAttribute IncludeDataContractAttributes { get; private set; } = ISI.Extensions.VisualStudio.IncludePropertyAttribute.No;
		public static bool EmitDefaultValueFalse { get; private set; } = true;
		public static ISI.Extensions.VisualStudio.PreferredSerializer PreferredSerializer { get; private set; } = ISI.Extensions.VisualStudio.PreferredSerializer.Json;
		public static ISI.Extensions.VisualStudio.IncludePropertyAttribute IncludeRepositoryAttributes { get; private set; } = ISI.Extensions.VisualStudio.IncludePropertyAttribute.No;
		public static bool IncludeSpreadSheetsAttributes { get; private set; } = false;
		public static ISI.Extensions.VisualStudio.IncludePropertyAttribute IncludeDocumentDataAttributes { get; private set; } = ISI.Extensions.VisualStudio.IncludePropertyAttribute.No;

		public GenerateClassDefinitionDialog(
			ISI.Extensions.VisualStudio.CodeGenerationApi codeGenerationApi,
			Community.VisualStudio.Toolkit.Project project,
			ISI.Extensions.VisualStudio.ClassDefinition classDefinition)
		{
			CodeGenerationApi = codeGenerationApi;

			Project = project;
			ClassDefinition = classDefinition;

			ProcessUpdatePreview = false;

			InitializeComponent();

			SetFormatPropertyName();
			SetIncludeDataContractAttributes();
			SetEmitDefaultValueFalse();
			SetPreferredSerializer();
			SetIncludeRepositoryAttributes();
			SetIncludeSpreadSheetsAttributes();
			SetIncludeDocumentDataAttributes();

			ProcessUpdatePreview = true;

			UpdatePreview();
		}

		private void SetFormatPropertyName()
		{
			rdoFormatPropertyNameNo.IsChecked = (FormatPropertyName == ISI.Extensions.VisualStudio.StringCaseFormat.No);
			rdoFormatPropertyNameYesUsingCamelCase.IsChecked = (FormatPropertyName == ISI.Extensions.VisualStudio.StringCaseFormat.YesUsingCamelCase);
			rdoFormatPropertyNameYesUsingPascalCase.IsChecked = (FormatPropertyName == ISI.Extensions.VisualStudio.StringCaseFormat.YesUsingPascalCase);
		}

		private void SetFormatPropertyName(ISI.Extensions.VisualStudio.StringCaseFormat stringCaseFormat)
		{
			FormatPropertyName = stringCaseFormat;
			UpdatePreview();
		}

		private void rdoFormatPropertyNameNo_OnChecked(object sender, RoutedEventArgs e) => SetFormatPropertyName(ISI.Extensions.VisualStudio.StringCaseFormat.No);
		private void rdoFormatPropertyNameYesUsingCamelCase_OnChecked(object sender, RoutedEventArgs e) => SetFormatPropertyName(ISI.Extensions.VisualStudio.StringCaseFormat.YesUsingCamelCase);
		private void rdoFormatPropertyNameYesUsingPascalCase_OnChecked(object sender, RoutedEventArgs e) => SetFormatPropertyName(ISI.Extensions.VisualStudio.StringCaseFormat.YesUsingPascalCase);

		private void SetIncludeDataContractAttributes()
		{
			rdoDataContractNameFormatNo.IsChecked = (IncludeDataContractAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.No);
			rdoDataContractNameFormatYes.IsChecked = (IncludeDataContractAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.Yes);
			rdoDataContractNameFormatYesWithNoNaming.IsChecked = (IncludeDataContractAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesWithNoNaming);
			rdoDataContractNameFormatYesUseCamelCaseIfNotDefined.IsChecked = (IncludeDataContractAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUseCamelCaseIfNameNotDefined);
			rdoDataContractNameFormatYesUsingCamelCase.IsChecked = (IncludeDataContractAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingCamelCase);
			rdoDataContractNameFormatYesUsePascalCaseIfNotDefined.IsChecked = (IncludeDataContractAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsePascalCaseIfNameNotDefined);
			rdoDataContractNameFormatYesUsingPascalCase.IsChecked = (IncludeDataContractAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingPascalCase);
		}

		private void SetIncludeDataContractAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute includePropertyAttribute)
		{
			IncludeDataContractAttributes = includePropertyAttribute;
			UpdatePreview();
		}

		private void rdoDataContractNameFormatNo_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDataContractAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.No);
		private void rdoDataContractNameFormatYes_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDataContractAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.Yes);
		private void rdoDataContractNameFormatYesWithNoNaming_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDataContractAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesWithNoNaming);
		private void rdoDataContractNameFormatYesUseCamelCaseIfNotDefined_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDataContractAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUseCamelCaseIfNameNotDefined);
		private void rdoDataContractNameFormatYesUsingCamelCase_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDataContractAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingCamelCase);
		private void rdoDataContractNameFormatYesUsePascalCaseIfNotDefined_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDataContractAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsePascalCaseIfNameNotDefined);
		private void rdoDataContractNameFormatYesUsingPascalCase_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDataContractAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingPascalCase);

		private void SetEmitDefaultValueFalse()
		{
			chkDataContractEmitDefaultValueFalse.IsChecked = EmitDefaultValueFalse;
		}

		private void chkDataContractEmitDefaultValueFalse_Changed(object sender, RoutedEventArgs e)
		{
			EmitDefaultValueFalse = chkDataContractEmitDefaultValueFalse.IsChecked.GetValueOrDefault();
			UpdatePreview();
		}

		private void SetPreferredSerializer()
		{
			rdoPreferredSerializerNone.IsChecked = (PreferredSerializer == ISI.Extensions.VisualStudio.PreferredSerializer.None);
			rdoPreferredSerializerJson.IsChecked = (PreferredSerializer == ISI.Extensions.VisualStudio.PreferredSerializer.Json);
			rdoPreferredSerializerXml.IsChecked = (PreferredSerializer == ISI.Extensions.VisualStudio.PreferredSerializer.Xml);
		}

		private void SetPreferredSerializer(ISI.Extensions.VisualStudio.PreferredSerializer preferredSerializer)
		{
			PreferredSerializer = preferredSerializer;
			UpdatePreview();
		}

		private void rdoPreferredSerializerNone_OnChecked(object sender, RoutedEventArgs e) => SetPreferredSerializer(ISI.Extensions.VisualStudio.PreferredSerializer.None);
		private void rdoPreferredSerializerJson_OnChecked(object sender, RoutedEventArgs e) => SetPreferredSerializer(ISI.Extensions.VisualStudio.PreferredSerializer.Json);
		private void rdoPreferredSerializerXml_OnChecked(object sender, RoutedEventArgs e) => SetPreferredSerializer(ISI.Extensions.VisualStudio.PreferredSerializer.Xml);

		private void SetIncludeRepositoryAttributes()
		{
			rdoRepositoryNameFormatNo.IsChecked = (IncludeRepositoryAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.No);
			rdoRepositoryNameFormatYes.IsChecked = (IncludeRepositoryAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.Yes);
			rdoRepositoryNameFormatYesWithNoNaming.IsChecked = (IncludeRepositoryAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesWithNoNaming);
			rdoRepositoryNameFormatYesUseCamelCaseIfNotDefined.IsChecked = (IncludeRepositoryAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUseCamelCaseIfNameNotDefined);
			rdoRepositoryNameFormatYesUsingCamelCase.IsChecked = (IncludeRepositoryAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingCamelCase);
			rdoRepositoryNameFormatYesUsePascalCaseIfNotDefined.IsChecked = (IncludeRepositoryAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsePascalCaseIfNameNotDefined);
			rdoRepositoryNameFormatYesUsingPascalCase.IsChecked = (IncludeRepositoryAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingPascalCase);
		}

		private void SetIncludeRepositoryAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute includePropertyAttribute)
		{
			IncludeRepositoryAttributes = includePropertyAttribute;
			UpdatePreview();
		}

		private void rdoRepositoryNameFormatNo_OnChecked(object sender, RoutedEventArgs e) => SetIncludeRepositoryAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.No);
		private void rdoRepositoryNameFormatYes_OnChecked(object sender, RoutedEventArgs e) => SetIncludeRepositoryAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.Yes);
		private void rdoRepositoryNameFormatYesWithNoNaming_OnChecked(object sender, RoutedEventArgs e) => SetIncludeRepositoryAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesWithNoNaming);
		private void rdoRepositoryNameFormatYesUseCamelCaseIfNotDefined_OnChecked(object sender, RoutedEventArgs e) => SetIncludeRepositoryAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUseCamelCaseIfNameNotDefined);
		private void rdoRepositoryNameFormatYesUsingCamelCase_OnChecked(object sender, RoutedEventArgs e) => SetIncludeRepositoryAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingCamelCase);
		private void rdoRepositoryNameFormatYesUsePascalCaseIfNotDefined_OnChecked(object sender, RoutedEventArgs e) => SetIncludeRepositoryAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsePascalCaseIfNameNotDefined);
		private void rdoRepositoryNameFormatYesUsingPascalCase_OnChecked(object sender, RoutedEventArgs e) => SetIncludeRepositoryAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingPascalCase);

		private void SetIncludeSpreadSheetsAttributes()
		{
			chkUseSpreadsheetAttributes.IsChecked = IncludeSpreadSheetsAttributes;
		}

		private void chkUseSpreadsheetAttributes_Changed(object sender, RoutedEventArgs e)
		{
			IncludeSpreadSheetsAttributes = chkUseSpreadsheetAttributes.IsChecked.GetValueOrDefault();
			UpdatePreview();
		}

		private void SetIncludeDocumentDataAttributes()
		{
			rdoDataDocumentNameFormatNo.IsChecked = (IncludeDocumentDataAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.No);
			rdoDataDocumentNameFormatYes.IsChecked = (IncludeDocumentDataAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.Yes);
			rdoDataDocumentNameFormatYesWithNoNaming.IsChecked = (IncludeDocumentDataAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesWithNoNaming);
			rdoDataDocumentNameFormatYesUseCamelCaseIfNotDefined.IsChecked = (IncludeDocumentDataAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUseCamelCaseIfNameNotDefined);
			rdoDataDocumentNameFormatYesUsingCamelCase.IsChecked = (IncludeDocumentDataAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingCamelCase);
			rdoDataDocumentNameFormatYesUsePascalCaseIfNotDefined.IsChecked = (IncludeDocumentDataAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsePascalCaseIfNameNotDefined);
			rdoDataDocumentNameFormatYesUsingPascalCase.IsChecked = (IncludeDocumentDataAttributes == ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingPascalCase);
		}

		private void SetIncludeDocumentDataAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute includePropertyAttribute)
		{
			IncludeDocumentDataAttributes = includePropertyAttribute;
			UpdatePreview();
		}

		private void rdoDataDocumentNameFormatNo_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDocumentDataAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.No);
		private void rdoDataDocumentNameFormatYes_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDocumentDataAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.Yes);
		private void rdoDataDocumentNameFormatYesWithNoNaming_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDocumentDataAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesWithNoNaming);
		private void rdoDataDocumentNameFormatYesUseCamelCaseIfNotDefined_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDocumentDataAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUseCamelCaseIfNameNotDefined);
		private void rdoDataDocumentNameFormatYesUsingCamelCase_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDocumentDataAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingCamelCase);
		private void rdoDataDocumentNameFormatYesUsePascalCaseIfNotDefined_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDocumentDataAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsePascalCaseIfNameNotDefined);
		private void rdoDataDocumentNameFormatYesUsingPascalCase_OnChecked(object sender, RoutedEventArgs e) => SetIncludeDocumentDataAttributes(ISI.Extensions.VisualStudio.IncludePropertyAttribute.YesUsingPascalCase);

		private void UpdatePreview()
		{
			if (ProcessUpdatePreview)
			{
				txtPreview.Text = CodeGenerationApi.GenerateClassDefinition(new ISI.Extensions.VisualStudio.DataTransferObjects.CodeGenerationApi.GenerateClassDefinitionRequest()
				{
					CodeExtensionProviderUuid = Project.GetCodeExtensionProvider().CodeExtensionProviderUuid,
					ClassDefinition = ClassDefinition,
					FormatPropertyName = FormatPropertyName,
					IncludeDataContractAttributes = IncludeDataContractAttributes,
					EmitDefaultValueFalse = EmitDefaultValueFalse,
					IncludeRepositoryAttributes = IncludeRepositoryAttributes,
					IncludeDocumentDataAttributes = IncludeDocumentDataAttributes,
					IncludeSpreadSheetsAttributes = IncludeSpreadSheetsAttributes,
					PreferredSerializer = PreferredSerializer,
				}).Content;
			}
		}

		private void btnPaste_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
	}
}
