using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.RecipeExtensions_Project_AddProjectProperties_MenuItemId)]
	public class RecipeExtensions_Project_AddProjectProperties_Command : BaseCommand<RecipeExtensions_Project_AddProjectProperties_Command>
	{
		private static RecipeExtensions_Project_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_Project_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_Project_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			showCommand = RecipeExtensionsHelper.IsProjectRoot(solutionItem);

			Command.Visible = showCommand;

			base.BeforeQueryStatus(eventArgs);
		}

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			try
			{
				var solutionItem = await VS.Solutions.GetActiveItemAsync();
				var solution = await VS.Solutions.GetCurrentSolutionAsync();
				var project = await VS.Solutions.GetActiveProjectAsync();

				var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
				var solutionRecipesDirectory = System.IO.Path.Combine(solutionDirectory, ".recipes");

				var projectDirectory = RecipeExtensionsHelper.GetProjectDirectory(project);

				var csProj = System.IO.File.ReadAllText(project.FullPath);

				var csProjXml = System.Xml.Linq.XElement.Parse(csProj);

				var sdkAttribute = csProjXml.GetAttributeByLocalName("Sdk")?.Value ?? string.Empty;

				var deterministic = csProjXml.GetElementByLocalName("PropertyGroup")?.GetElementByLocalName("Deterministic")?.Value?.ToBooleanNullable();
				var langVersionLatest = ToLangVersionNullable(csProjXml.GetElementByLocalName("PropertyGroup")?.GetElementByLocalName("LangVersion")?.Value);
				var generateAssemblyInfo = csProjXml.GetElementByLocalName("PropertyGroup")?.GetElementByLocalName("GenerateAssemblyInfo")?.Value?.ToBooleanNullable();
				var runtimeIdentifiers = csProjXml.GetElementByLocalName("PropertyGroup")?.GetElementByLocalName("RuntimeIdentifiers")?.Value;

				var sharedAssemblyInfos = GetSharedFullNames(solution, project, "*.AssemblyInfo.cs");
				var useSharedAssemblyInfo = GetDefaultValue(csProj, sharedAssemblyInfos);

				var sharedVersions = GetSharedFullNames(solution, project, "*.Version.cs");
				var useSharedVersion = GetDefaultValue(csProj, sharedVersions);

				var addAssemblyInfo = System.IO.File.Exists(GetAssemblyInfoFullName(project));

				var sharedLicenseHeaders = GetSharedFullNames(solution, project, "*.licenseheader");
				var useSharedLicenseHeader = GetDefaultValue(csProj, sharedLicenseHeaders);

				var inputDialog = new AddProjectPropertiesDialog(deterministic, langVersionLatest, generateAssemblyInfo, runtimeIdentifiers, sharedAssemblyInfos, useSharedAssemblyInfo, sharedVersions, useSharedVersion, addAssemblyInfo, sharedLicenseHeaders, useSharedLicenseHeader);

				var inputDialogResult = await inputDialog.ShowDialogAsync();

				if (inputDialogResult.GetValueOrDefault())
				{
					var propertyGroupElement = csProjXml.GetElementByLocalName("PropertyGroup");

					SetElement(propertyGroupElement, "Deterministic", inputDialog.Deterministic);

					if (inputDialog.LangVersionLatest.HasValue)
					{
						var langVersionElement = propertyGroupElement?.GetElementByLocalName("LangVersion");

						if (inputDialog.LangVersionLatest.GetValueOrDefault())
						{
							if (langVersionElement == null)
							{
								propertyGroupElement.Add(new System.Xml.Linq.XElement("LangVersion", "latest"));
							}
							else
							{
								langVersionElement.Value = "latest";
							}
						}
						else if (langVersionElement != null)
						{
							langVersionElement.Remove();
						}
					}

					SetElement(propertyGroupElement, "GenerateAssemblyInfo", inputDialog.GenerateAssemblyInfo);

					var runtimeIdentifiersElement = propertyGroupElement?.GetElementByLocalName("RuntimeIdentifiers");
					if (string.IsNullOrWhiteSpace(inputDialog.RuntimeIdentifiers))
					{
						if (runtimeIdentifiersElement != null)
						{
							runtimeIdentifiersElement.Remove();
						}
					}
					else
					{
						if (runtimeIdentifiersElement == null)
						{
							propertyGroupElement.Add(new System.Xml.Linq.XElement("RuntimeIdentifiers", inputDialog.RuntimeIdentifiers));
						}
						else
						{
							runtimeIdentifiersElement.Value = inputDialog.RuntimeIdentifiers;
						}
					}

					var neededIncludes = new List<(string ElementName, string Include, string Link)>();

					if (!TrySetInclude(csProjXml, "Compile", useSharedAssemblyInfo, inputDialog.UseSharedAssemblyInfo))
					{
						if (!string.IsNullOrWhiteSpace(inputDialog.UseSharedAssemblyInfo))
						{
							neededIncludes.Add((ElementName: "Compile", Include: inputDialog.UseSharedAssemblyInfo, Link: string.Format("Properties\\{0}", inputDialog.UseSharedAssemblyInfo.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last())));
						}
					}

					if (!TrySetInclude(csProjXml, "Compile", useSharedVersion, inputDialog.UseSharedVersion))
					{
						if (!string.IsNullOrWhiteSpace(inputDialog.UseSharedVersion))
						{
							neededIncludes.Add((ElementName: "Compile", Include: inputDialog.UseSharedVersion, Link: string.Format("Properties\\{0}", inputDialog.UseSharedVersion.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last())));
						}
					}

					if (inputDialog.AddAssemblyInfo)
					{
						var assemblyInfoFullName = GetAssemblyInfoFullName(project);

						var propertiesDirectory = System.IO.Path.GetDirectoryName(assemblyInfoFullName);
						System.IO.Directory.CreateDirectory(propertiesDirectory);

						if (!System.IO.File.Exists(assemblyInfoFullName))
						{
							var contentReplacements = new Dictionary<string, string>
							{
								{ "${Namespace}", project.GetRootNamespace() },
							};

							var recipes = new Extensions_Helper.RecipeItem[]
							{
								new Extensions_Helper.RecipeItem(System.IO.Path.Combine(propertiesDirectory, "AssemblyInfo.cs"), RecipeExtensionsHelper.GetContent(nameof(Options.Project_AssemblyInfo_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
							};

							await RecipeExtensionsHelper.AddFromRecipesAsync(project, recipes, contentReplacements);

							if (!sdkAttribute.StartsWith("Microsoft.NET", StringComparison.InvariantCultureIgnoreCase))
							{
								neededIncludes.Add((ElementName: "Compile", Include: "Properties\\AssemblyInfo.cs", Link: string.Empty));
							}
						}
					}

					if (!TrySetInclude(csProjXml, "None", useSharedLicenseHeader, inputDialog.UseSharedLicenseHeader))
					{
						if (!string.IsNullOrWhiteSpace(inputDialog.UseSharedLicenseHeader))
						{
							neededIncludes.Add((ElementName: "None", Include: inputDialog.UseSharedLicenseHeader, Link: inputDialog.UseSharedLicenseHeader.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last()));
						}
					}

					if (neededIncludes.Any())
					{
						if (sdkAttribute.StartsWith("Microsoft.NET"))
						{
							var itemGroupElement = new System.Xml.Linq.XElement("ItemGroup");
							foreach (var neededInclude in neededIncludes)
							{
								var element = new System.Xml.Linq.XElement(neededInclude.ElementName);
								element.Add(new System.Xml.Linq.XAttribute("Include", neededInclude.Include));
								if (!string.IsNullOrWhiteSpace(neededInclude.Link))
								{
									element.Add(new System.Xml.Linq.XAttribute("Link",neededInclude.Link));
								}

								itemGroupElement.Add(element);
							}

							var lastPropertyGroupElement = csProjXml.GetElementsByLocalName("PropertyGroup").NullCheckedLastOrDefault();
							if (lastPropertyGroupElement == null)
							{
								csProjXml.Add(itemGroupElement);
							}
							else
							{
								lastPropertyGroupElement.AddAfterSelf(itemGroupElement);
							}
						}
						else
						{
							var itemGroupElement = new System.Xml.Linq.XElement("ItemGroup");
							foreach (var neededInclude in neededIncludes)
							{
								var element = new System.Xml.Linq.XElement(neededInclude.ElementName);
								element.Add(new System.Xml.Linq.XAttribute("Include", neededInclude.Include));
								if (!string.IsNullOrWhiteSpace(neededInclude.Link))
								{
									element.Add(new System.Xml.Linq.XElement("Link",neededInclude.Link));
								}

								itemGroupElement.Add(element);
							}

							var lastPropertyGroupElement = csProjXml.GetElementsByLocalName("PropertyGroup").NullCheckedLastOrDefault();
							if (lastPropertyGroupElement == null)
							{
								csProjXml.Add(itemGroupElement);
							}
							else
							{
								lastPropertyGroupElement.AddAfterSelf(itemGroupElement);
							}
						}
					}

					csProj = csProjXml.ToString();

					if (!sdkAttribute.StartsWith("Microsoft.NET", StringComparison.InvariantCultureIgnoreCase))
					{
						csProj = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n{0}", csProj);
					}

					System.IO.File.WriteAllText(project.FullPath, csProj);
				}
			}
			catch (Exception exception)
			{
				var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

				await outputWindowPane.WriteLineAsync(exception.ErrorMessageFormatted());

				throw;
			}
		}

		public bool? ToLangVersionNullable(string value)
		{
			if (value == null)
			{
				return null;
			}

			if (string.Equals(value, "latest", StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}

			return false;
		}

		public string[] GetSharedFullNames(Solution solution, Project project, string searchPattern)
		{
			var sharedFileNames = new List<string>();

			var solutionDirectory = System.IO.Path.GetDirectoryName(solution.FullPath);
			var projectDirectory = System.IO.Path.GetDirectoryName(project.FullPath);

			var path = "..\\";

			if (projectDirectory.StartsWith(solutionDirectory, StringComparison.InvariantCultureIgnoreCase))
			{
				do
				{
					projectDirectory = System.IO.Path.GetDirectoryName(projectDirectory);

					sharedFileNames.AddRange(System.IO.Directory.EnumerateFiles(projectDirectory, searchPattern).Select(fullName => string.Format("{0}{1}", path, System.IO.Path.GetFileName(fullName))));

					path = string.Format("..\\{0}", path);
				} while (projectDirectory.StartsWith(solutionDirectory, StringComparison.InvariantCultureIgnoreCase));
			}
			else
			{
				sharedFileNames.AddRange(System.IO.Directory.EnumerateFiles(System.IO.Path.GetDirectoryName(projectDirectory), searchPattern).Select(fullName => string.Format("{0}{1}", path, System.IO.Path.GetFileName(fullName))));
			}

			return sharedFileNames.ToArray();
		}

		public string GetAssemblyInfoFullName(Project project)
		{
			var projectDirectory = System.IO.Path.GetDirectoryName(project.FullPath);

			return System.IO.Path.Combine(projectDirectory, "Properties", "AssemblyInfo.cs");
		}

		public string GetDefaultValue(string csProj, string[] values)
		{
			foreach (var value in values)
			{
				if (csProj.IndexOf(string.Format("\"{0}\"", value)) >= 0)
				{
					return value;
				}
			}

			return values.NullCheckedFirstOrDefault();
		}

		public void SetElement(System.Xml.Linq.XElement propertyGroupElement, string elementName, bool? value)
		{
			var element = propertyGroupElement.GetElementByLocalName(elementName);

			if (value.HasValue)
			{
				if (element != null)
				{
					element.Value = value.Value.TrueFalse(false, BooleanExtensions.TextCase.Lower);
				}
				else
				{
					propertyGroupElement.Add(new System.Xml.Linq.XElement(elementName, value.Value.TrueFalse(false, BooleanExtensions.TextCase.Lower)));
				}
			}
			else if (element != null)
			{
				element.Remove();
			}
		}

		public bool TrySetInclude(System.Xml.Linq.XElement csProjXml, string elementName, string currentFilePath, string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				if (!string.IsNullOrWhiteSpace(currentFilePath))
				{
					foreach (var itemGroupElement in csProjXml.GetElementsByLocalName("ItemGroup"))
					{
						foreach (var compileElement in itemGroupElement.GetElementsByLocalName(elementName))
						{
							var includeElement = compileElement.GetElementByLocalName("Include");
							if (includeElement != null)
							{
								if (string.Equals(includeElement.Value, currentFilePath, StringComparison.InvariantCultureIgnoreCase))
								{
									includeElement.Remove();
									return true;
								}
							}
							else
							{
								var includeAttribute = compileElement.GetAttributeByLocalName("Include");
								if (includeAttribute != null)
								{
									if (string.Equals(includeAttribute.Value, currentFilePath, StringComparison.InvariantCultureIgnoreCase))
									{
										includeAttribute.Remove();
										return true;
									}
								}
							}
						}
					}
				}
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(currentFilePath))
				{
					var currentLinkValue = currentFilePath.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
					var linkValue = filePath.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last();

					foreach (var itemGroupElement in csProjXml.GetElementsByLocalName("ItemGroup"))
					{
						foreach (var compileElement in itemGroupElement.GetElementsByLocalName(elementName))
						{
							var includeElement = compileElement.GetElementByLocalName("Include");
							if (includeElement != null)
							{
								if (string.Equals(includeElement.Value, currentFilePath, StringComparison.InvariantCultureIgnoreCase))
								{
									includeElement.Value = filePath;

									var linkElement = includeElement.GetElementByLocalName("Link");
									if (linkElement != null)
									{
										linkElement.Value = linkElement.Value.Replace(currentLinkValue, linkValue);
									}
									else
									{
										var linkAttribute = includeElement.GetAttributeByLocalName("Link");
										if (linkAttribute != null)
										{
											linkAttribute.Value = linkAttribute.Value.Replace(currentLinkValue, linkValue);
										}
									}

									return true;
								}
							}
							else
							{
								var includeAttribute = compileElement.GetAttributeByLocalName("Include");
								if (includeAttribute != null)
								{
									if (string.Equals(includeAttribute.Value, currentFilePath, StringComparison.InvariantCultureIgnoreCase))
									{
										includeAttribute.Value = filePath;

										var linkAttribute = compileElement.GetAttributeByLocalName("Link");
										if (linkAttribute != null)
										{
											linkAttribute.Value = linkAttribute.Value.Replace(currentLinkValue, linkValue);
										}

										return true;
									}
								}
							}
						}
					}
				}
			}

			return false;
		}
	}
}
