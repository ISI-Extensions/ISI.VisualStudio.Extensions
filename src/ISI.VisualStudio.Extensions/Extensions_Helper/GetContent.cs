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
