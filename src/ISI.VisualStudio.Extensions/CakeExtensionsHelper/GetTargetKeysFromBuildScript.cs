using System;

namespace ISI.VisualStudio.Extensions
{
	public partial class CakeExtensionsHelper
	{
		public string[] GetTargetKeysFromBuildScript(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			return CakeApi.GetTargetKeysFromBuildScript(new ISI.Extensions.Cake.DataTransferObjects.CakeApi.GetTargetKeysFromBuildScriptRequest()
			{
				BuildScriptFullName = solutionItem.FullPath,
			}).Targets ?? Array.Empty<string>();
		}
	}
}
