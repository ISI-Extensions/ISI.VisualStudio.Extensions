using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public static partial class ProjectExtensions
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

			var content = System.IO.File.ReadAllText(project.FullPath);

			foreach (var codeExtensionProvider in ISI.Extensions.VisualStudio.CodeExtensionProviders.GetCodeExtensionProviders())
			{
				if (content.IndexOf(string.Format("\"{0}", codeExtensionProvider.Namespace)) >= 0)
				{
					return codeExtensionProvider;
				}
				if (content.IndexOf(string.Format("\\{0}", codeExtensionProvider.Namespace)) >= 0)
				{
					return codeExtensionProvider;
				}
			}

			var providerUuid = ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Extensions.CodeExtensionProvider.CodeExtensionProviderUuid;

			if(content.IndexOf("<TargetFrameworkVersion>v4.", System.StringComparison.InvariantCultureIgnoreCase) >= 0)
			{
				providerUuid = ISI.Extensions.VisualStudio.CodeExtensionProviders.ISI.Libraries.CodeExtensionProvider.CodeExtensionProviderUuid;
			}
			
			ISI.Extensions.VisualStudio.CodeExtensionProviders.TryGetCodeExtensionProvider(providerUuid, out var isiExtensionsCodeExtensionProvider);

			return isiExtensionsCodeExtensionProvider;
		}
	}
}
