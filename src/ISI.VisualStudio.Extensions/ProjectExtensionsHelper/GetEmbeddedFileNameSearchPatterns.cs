using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	public partial class ProjectExtensionsHelper
	{
		private readonly string[] _embeddedFileNameSearchPatterns = new[]
		{
			"**\\*.aspx",
			"**\\*.ascx",
			"**\\*.cshtml",
			"**\\*.vbhtml",
			"**\\*.swf",
			"**\\*.gif",
			"**\\*.png",
			"**\\*.jpg",
			"**\\*.ico",
			"**\\*.icon",
			"**\\*.svg",
			"**\\*.svgz",
			"**\\*.mp4",
			"**\\*.m4v",
			"**\\*.ogg",
			"**\\*.ogv",
			"**\\*.webm",
			"**\\*.oga",
			"**\\*.spx",
			"**\\*.eot",
			"**\\*.otf",
			"**\\*.ttf",
			"**\\*.woff",
			"**\\*.css",
			"**\\*.cssx",
			"**\\*.cscss",
			"**\\*.vbcss",
			"**\\*.less",
			"**\\*.lessx",
			"**\\*.csless",
			"**\\*.vbless",
			"**\\*.scss",
			"**\\*.scssx",
			"**\\*.csscss",
			"**\\*.vbscss",
			"**\\*.js",
			"**\\*.jsx",
			"**\\*.csjs",
			"**\\*.vbjs",
			"**\\web.config"
		};

		public string[] GetEmbeddedFileNameSearchPatterns() => _embeddedFileNameSearchPatterns;
	}
}
