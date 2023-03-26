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
