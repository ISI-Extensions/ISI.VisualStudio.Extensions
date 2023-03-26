#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
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
	[Command(PackageIds.RecipeExtensions_Project_AddEnumTextTemplate_MenuItemId)]
	public class RecipeExtensions_Project_AddEnumTextTemplate_Command : BaseCommand<RecipeExtensions_Project_AddEnumTextTemplate_Command>
	{
		private static RecipeExtensions_Project_Helper _recipeExtensionsHelper = null;
		protected RecipeExtensions_Project_Helper RecipeExtensionsHelper => _recipeExtensionsHelper ??= Package.GetServiceProvider().GetService<RecipeExtensions_Project_Helper>();

		protected override void BeforeQueryStatus(EventArgs eventArgs)
		{
			var showCommand = false;

			var solutionItem = VS.Solutions.GetActiveItemAsync().GetAwaiter().GetResult();

			if (RecipeExtensionsHelper.IsProjectRoot(solutionItem) || RecipeExtensionsHelper.IsProjectFolder(solutionItem))
			{
				showCommand = true;
			}

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

				var directory = solutionItem.FullPath;
				while (!System.IO.Directory.Exists(directory))
				{
					directory = System.IO.Path.GetDirectoryName(directory);
				}

				var @namespace = RecipeExtensionsHelper.GetNamespace(project, solutionItem);

				var addEnumTextTemplateDialog = new AddEnumTextTemplateDialog(string.Empty, @namespace, string.Empty, string.Empty, string.Empty, string.Empty, "|");

				var addEnumTextTemplateDialogResult = await addEnumTextTemplateDialog.ShowDialogAsync();

				if (addEnumTextTemplateDialogResult.GetValueOrDefault())
				{
					var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

					await outputWindowPane.ActivateAsync();

					await outputWindowPane.ClearAsync();

					await outputWindowPane.WriteLineAsync("New Enum Text Template");

					var contentReplacements = new Dictionary<string, string>
					{
						{ "${Namespace}", addEnumTextTemplateDialog.Namespace },
						{ "${ConnectionString}", addEnumTextTemplateDialog.ConnectionString },
						{ "${EnumTableName}", addEnumTextTemplateDialog.EnumTableName },
						{ "${EnumIdColumnName}", addEnumTextTemplateDialog.EnumIdColumnName },
						{ "${EnumUuidColumnName}", addEnumTextTemplateDialog.EnumUuidColumnName },
						{ "${AliasesDelimiter}", addEnumTextTemplateDialog.AliasesDelimiter },
					};

					var recipes = new Extensions_Helper.RecipeItem[]
					{
						new Extensions_Helper.RecipeItem(System.IO.Path.Combine(directory, string.Format("{0}.tt", addEnumTextTemplateDialog.EnumName)), RecipeExtensionsHelper.GetContent(nameof(RecipeOptions.Project_EnumText_Template), projectDirectory, solutionRecipesDirectory, solutionDirectory), true),
					};

					await RecipeExtensionsHelper.AddFromRecipesAsync(project, recipes, contentReplacements);

					await outputWindowPane.WriteLineAsync("Done\n");
					await outputWindowPane.ActivateAsync();
				}
			}
			catch (Exception exception)
			{
				var outputWindowPane = await RecipeExtensionsHelper.GetOutputWindowPaneAsync();

				await outputWindowPane.WriteLineAsync(exception.ErrorMessageFormatted());

				throw;
			}
		}
	}
}
