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
