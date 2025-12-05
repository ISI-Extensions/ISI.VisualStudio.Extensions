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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions.Extensions
{
	public static class MSBuildProjectExtensions
	{
		private const string ReferenceProjectItem = "Reference";

		internal static Microsoft.Build.Evaluation.Project AsMSBuildProject(this EnvDTE.Project project)
		{
			return Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection.GetLoadedProjects(project.FullName).FirstOrDefault() ??
						 Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection.LoadProject(project.FullName);
		}

		internal static IEnumerable<Tuple<Microsoft.Build.Evaluation.ProjectItem, System.Reflection.AssemblyName>> GetAssemblyReferences(this Microsoft.Build.Evaluation.Project project)
		{
			foreach (var referenceProjectItem in project.GetItems(ReferenceProjectItem))
			{
				System.Reflection.AssemblyName assemblyName = null;

				try
				{
					assemblyName = new System.Reflection.AssemblyName(referenceProjectItem.EvaluatedInclude);
				}
				catch
				{
					// Swallow any exceptions we might get because of malformed assembly names
				}

				// We can't yield from within the try so we do it out here if everything was successful
				if (assemblyName != null)
				{
					yield return Tuple.Create(referenceProjectItem, assemblyName);
				}
			}
		}

		internal static bool AssemblyNamesMatch(System.Reflection.AssemblyName assemblyNameX, System.Reflection.AssemblyName assemblyNameY, Microsoft.Build.Evaluation.ProjectItem reference)
		{
			var useSpecificVersion = string.Equals(reference.GetMetadataValue("SpecificVersion") ?? string.Empty, "true", StringComparison.InvariantCultureIgnoreCase);

			return assemblyNameX.Name.Equals(assemblyNameY.Name, StringComparison.OrdinalIgnoreCase) &&
						 (EqualsIfNotNull(assemblyNameX.Version, assemblyNameY.Version) || !useSpecificVersion) &&
						 EqualsIfNotNull(assemblyNameX.CultureInfo, assemblyNameY.CultureInfo) &&
						 EqualsIfNotNull(assemblyNameX.GetPublicKeyToken(), assemblyNameY.GetPublicKeyToken(), Enumerable.SequenceEqual);
		}

		internal static bool EqualsIfNotNull<T>(T objectX, T objectY)
		{
			return EqualsIfNotNull(objectX, objectY, (a, b) => a.Equals(b));
		}

		internal static bool EqualsIfNotNull<T>(T objectX, T objectY, Func<T, T, bool> equals)
		{
			// If both objects are non null do the equals
			if ((objectX != null) && (objectY != null))
			{
				return equals(objectX, objectY);
			}

			// Otherwise consider them equal if either of the values are null
			return true;
		}

	}
}
