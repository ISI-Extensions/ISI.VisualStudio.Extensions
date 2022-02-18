using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class ProjectExtensions_Helper
	{
		public class Usings : List<string>
		{
			public Usings()
			{

			}
			public Usings(IEnumerable<string> usings)
				: base(usings)
			{

			}

			public string GetFormatted() => string.Join(Environment.NewLine, this.Select(@using => string.Format("using {0};", @using)));
		}

		public Usings GetSortedUsings(ISI.Extensions.VisualStudio.ICodeExtensionProvider codeExtensionProvider, IEnumerable<string> usings = null, IEnumerable<string> sourceFullNames = null)
		{
			var usingStatements = new HashSet<string>((usings ?? Array.Empty<string>()).Select(@using => @using.Replace('\t', ' ').Trim(' ').TrimStart("using ").Replace(';', ' ').Trim()), StringComparer.InvariantCultureIgnoreCase);

			if (sourceFullNames.NullCheckedAny())
			{
				foreach (var fileName in sourceFullNames)
				{
					if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
					{
						foreach (var @using in System.IO.File.ReadAllLines(fileName).Where(line => line.StartsWith("using ", StringComparison.InvariantCulture)))
						{
							usingStatements.Add(@using.Replace('\t', ' ').Replace(';', ' ').Trim(' ').Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries)[1]);
						}
					}
				}
			}

			var sortedUsingStatements = new List<string>();

			void removeUsedUsingStatements()
			{
				foreach (var usingStatement in sortedUsingStatements)
				{
					usingStatements.Remove(usingStatement);
				}
			}

			if ((codeExtensionProvider?.DefaultUsingStatements).NullCheckedAny())
			{
				foreach (var usingStatement in codeExtensionProvider.DefaultUsingStatements)
				{
					sortedUsingStatements.Add(usingStatement);
				}
			}

			removeUsedUsingStatements();

			foreach (var usingStatement in usingStatements.Where(usingStatement => usingStatement.StartsWith("System.")).OrderBy(usingStatement => usingStatement))
			{
				sortedUsingStatements.Add(usingStatement);
			}

			removeUsedUsingStatements();

			foreach (var usingStatement in usingStatements.Where(usingStatement => usingStatement.EndsWith(".Extensions")).OrderBy(usingStatement => usingStatement))
			{
				sortedUsingStatements.Add(usingStatement);
			}

			removeUsedUsingStatements();

			foreach (var usingStatement in usingStatements.Where(usingStatement => usingStatement.IndexOf("=") >= 0).OrderBy(usingStatement => usingStatement))
			{
				sortedUsingStatements.Add(usingStatement);
			}

			removeUsedUsingStatements();

			foreach (var usingStatement in usingStatements.OrderBy(usingStatement => usingStatement))
			{
				sortedUsingStatements.Add(usingStatement);
			}

			return new Usings(sortedUsingStatements);
		}
	}
}
