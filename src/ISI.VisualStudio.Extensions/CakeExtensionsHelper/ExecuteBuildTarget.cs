using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class CakeExtensionsHelper
	{
		public async Task ExecuteBuildTargetAsync(Community.VisualStudio.Toolkit.SolutionItem solutionItem, string target = null)
		{
			var _outputWindowPane = await GetOutputWindowPaneAsync();

			await _outputWindowPane.ActivateAsync();

			await System.Threading.Tasks.Task.Run(() =>
			{
				CakeApi.ExecuteBuildTarget(new ISI.Extensions.Cake.DataTransferObjects.CakeApi.ExecuteBuildTargetRequest()
				{
					BuildScriptFullName = solutionItem.FullPath,
					Target = target,
					AddToLog = description => _outputWindowPane.WriteLine(description.TrimEnd('\r', '\n')),
				});
			});
		}
	}
}
