using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class Extensions_Helper
	{
		protected static IDictionary<string, Func<string>> _optionValueRetrievers = null;
		protected IDictionary<string, Func<string>> OptionValueRetrievers => (_optionValueRetrievers ??= BuildOptionValueRetrievers());

		private IDictionary<string, Func<string>> BuildOptionValueRetrievers()
		{
			var optionValueRetrievers = new Dictionary<string, Func<string>>(StringComparer.InvariantCultureIgnoreCase);

			var recipeExtensionsOptions = RecipeOptions.GetLiveInstanceAsync().GetAwaiter().GetResult();

			foreach (var propertyInfo in typeof(RecipeOptions).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance))
			{
				optionValueRetrievers.Add(propertyInfo.Name, () => propertyInfo.GetValue(recipeExtensionsOptions) as string);
			}

			return optionValueRetrievers;
		}

		public string GetContent(string key, params string[] directories)
		{
			var result = string.Empty;

			if (OptionValueRetrievers.TryGetValue(key, out var optionValueRetriever))
			{
				result = optionValueRetriever();
			}

			if (directories != null)
			{
				string fullName = null;

				foreach (var fileName in new[]
				{
					string.Format("ISI.Extensions.VisualStudio2019.Recipes.{0}.txt", key),
					string.Format("ISI.VisualStudio.Recipes.{0}.txt", key),
				})
				{
					foreach (var directory in directories)
					{
						if (string.IsNullOrWhiteSpace(fullName))
						{
							fullName = System.IO.Path.Combine(directory, fileName);

							if (!System.IO.File.Exists(fullName))
							{
								fullName = null;
							}
						}
					}
				}

				if (!string.IsNullOrWhiteSpace(fullName) && System.IO.File.Exists(fullName))
				{
					result = System.IO.File.ReadAllText(fullName);
				}
			}

			return result;
		}
	}
}
