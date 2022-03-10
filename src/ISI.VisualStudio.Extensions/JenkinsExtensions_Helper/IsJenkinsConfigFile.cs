using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class JenkinsExtensions_Helper
	{
		public bool IsJenkinsConfigFile(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			if (solutionItem?.Type == Community.VisualStudio.Toolkit.SolutionItemType.PhysicalFile)
			{
				return JenkinsApi.IsJenkinsConfigFile(new ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi.IsJenkinsConfigFileRequest()
				{
					FileName = solutionItem.FullPath,
				}).IsJenkinsConfigFile;
			}

			return false;
		}
	}
}
