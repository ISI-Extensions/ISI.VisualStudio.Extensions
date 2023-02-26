using Community.VisualStudio.Toolkit;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[Command(PackageIds.AboutMenuItemId)]
	public class AboutExtensions_About_Command : BaseCommand<AboutExtensions_About_Command>
	{
		protected override async Task ExecuteAsync(OleMenuCmdEventArgs oleMenuCmdEventArgs)
		{
			var version = ISI.Extensions.SystemInformation.GetAssemblyVersion(this.GetType().Assembly);

			var message = string.Format("{0}, Version: {1}", Vsix.Name, version);

			void resetOptions()
			{
				var recipeExtensionsOptions = RecipeOptions.GetLiveInstanceAsync().GetAwaiter().GetResult();

				var recipeOptions = new RecipeOptions();

				foreach (var propertyInfo in typeof(RecipeOptions).GetProperties())
				{
					if (propertyInfo.PropertyType == typeof(string))
					{
						propertyInfo.SetValue(recipeExtensionsOptions, propertyInfo.GetValue(recipeOptions));
					}
				}

				recipeExtensionsOptions.Save();
			}

			var inputDialog = new AboutDialog(message, resetOptions);

			var inputDialogResult = await inputDialog.ShowDialogAsync();
		}
	}
}
