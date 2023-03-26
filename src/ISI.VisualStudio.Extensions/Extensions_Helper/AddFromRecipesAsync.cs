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
 
using System.Collections.Generic;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class Extensions_Helper
	{
		public async System.Threading.Tasks.Task AddFromRecipesAsync(Community.VisualStudio.Toolkit.Project project, IEnumerable<RecipeItem> recipeItems, IEnumerable<KeyValuePair<string, string>> replacementValues)
		{
			foreach (var recipeItem in recipeItems)
			{
				var recipeReplacementValues = new Dictionary<string, string>();
				foreach (var replacementValue in replacementValues ?? System.Array.Empty<KeyValuePair<string, string>>())
				{
					recipeReplacementValues.Add(replacementValue.Key, replacementValue.Value);
				}

				recipeItem.PreAction?.Invoke(project, recipeItem.FullName, recipeItem.Content, recipeReplacementValues);

				if (!System.IO.File.Exists(recipeItem.FullName) && !string.IsNullOrEmpty(recipeItem.Content))
				{
					await AddFromRecipeAsync(project, recipeItem.FullName, recipeItem.Content, recipeReplacementValues).ContinueWith(task =>
					{
						if (recipeItem.Open)
						{
							Community.VisualStudio.Toolkit.VS.Documents.OpenViaProjectAsync(recipeItem.FullName).GetAwaiter().GetResult();
						}
					});
				}

				recipeItem.PostAction?.Invoke(project, recipeItem.FullName, recipeItem.Content, recipeReplacementValues);
			}
		}
	}
}
