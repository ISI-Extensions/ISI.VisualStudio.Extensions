using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions.Extensions
{
	public static class ProjectExtensions
	{
		public static ISI.Extensions.VisualStudio.ICodeExtensionProvider GetCodeExtensionProvider(this Community.VisualStudio.Toolkit.Project project)
		{
			var referenceNames = project.References.ToNullCheckedHashSet(reference => reference.Name, NullCheckCollectionResult.Empty);

			foreach (var codeExtensionProvider in ISI.Extensions.VisualStudio.CodeExtensionProviders.GetCodeExtensionProviders())
			{
				if (referenceNames.Contains(codeExtensionProvider.Namespace))
				{
					return codeExtensionProvider;
				}
			}

			ISI.Extensions.VisualStudio.CodeExtensionProviders.TryGetCodeExtensionProvider(ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Extensions.CodeExtensionProvider.CodeExtensionProviderUuid, out var isiExtensionsCodeExtensionProvider);

			return isiExtensionsCodeExtensionProvider;
		}
	}
}
