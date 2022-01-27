﻿using System;
using System.Linq;
using System.Collections.Generic;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class RecipeExtensions_Helper
	{
		protected virtual HashSet<string> _contentFileExtensions { get; } = new HashSet<string>(new[] { "tt" }, StringComparer.InvariantCultureIgnoreCase);
	}
}