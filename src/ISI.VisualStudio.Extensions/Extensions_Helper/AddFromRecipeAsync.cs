using System.Collections.Generic;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class Extensions_Helper
	{
		public async System.Threading.Tasks.Task<Community.VisualStudio.Toolkit.PhysicalFile> AddFromRecipeAsync(Community.VisualStudio.Toolkit.Project project, string fullName, string content, IEnumerable<KeyValuePair<string, string>> replacementValues)
		{
			if (!string.IsNullOrEmpty(content))
			{
				if (replacementValues != null)
				{
					content = replacementValues.Aggregate(content, (current, replacementValue) => current.Replace(replacementValue.Key, replacementValue.Value));
				}

				using (var stream = System.IO.File.CreateText(fullName))
				{
					await stream.WriteAsync(content);
				}

				var physicalFiles = await project.AddExistingFilesAsync(fullName);

				var physicalFile = physicalFiles.NullCheckedFirstOrDefault();

				if (_contentFileExtensions.Contains(System.IO.Path.GetExtension(fullName)))
				{
					await physicalFile.TrySetAttributeAsync("BuildAction", "Content");
				}

				return physicalFile;
			}

			return null;
		}
	}
}
