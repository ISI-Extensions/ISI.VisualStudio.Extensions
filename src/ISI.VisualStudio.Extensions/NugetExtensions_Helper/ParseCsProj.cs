#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class NugetExtensions_Helper
	{
		public System.Collections.Generic.IEnumerable<ISI.Extensions.Nuget.NugetPackageKey> ParseCsProj(string[] csProjLines)
		{
			var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();

			foreach (var line in csProjLines)
			{
				if (line.Trim(' ', '\t').StartsWith("<PackageReference ", StringComparison.InvariantCultureIgnoreCase))
				{
					try
					{
						var keyValues = line.Replace("<PackageReference ", string.Empty).Replace("/>", string.Empty).Split([' '], StringSplitOptions.RemoveEmptyEntries).Select(item => item.Split(["=\"", "\""], StringSplitOptions.None)).ToDictionary(item => item[0].Trim(' ', '\t'), item => item[1].Trim(' ', '\t'), StringComparer.InvariantCultureIgnoreCase);

						var nugetPackageKey = new ISI.Extensions.Nuget.NugetPackageKey();

						var value = string.Empty;

						if (keyValues.TryGetValue("Include", out value))
						{
							nugetPackageKey.Package = value;
						}

						if (keyValues.TryGetValue("Update", out value))
						{
							nugetPackageKey.Package = value;
						}

						if (keyValues.TryGetValue("Version", out value))
						{
							nugetPackageKey.Version = value;
						}

						nugetPackageKeys.TryAdd(nugetPackageKey);
					}
					catch
					{

					}
				}
			}

			return nugetPackageKeys;
		}
	}
}