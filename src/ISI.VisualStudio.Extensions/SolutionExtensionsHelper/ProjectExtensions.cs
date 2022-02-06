using System;
using System.Linq;
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class SolutionExtensionsHelper
	{
		private static readonly object _projectExtensionsLock = new object();
		private static System.Collections.Generic.HashSet<string> _projectExtensions = null;
		public System.Collections.Generic.HashSet<string> ProjectExtensions
		{
			get
			{
				if (_projectExtensions == null)
				{
					lock (_projectExtensionsLock)
					{
						if (_projectExtensions == null)
						{
							var projectExtensions = new System.Collections.Generic.HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

							using (var rootKey = Microsoft.VisualStudio.Shell.VSRegistry.RegistryRoot(Microsoft.VisualStudio.Shell.Interop.__VsLocalRegistryType.RegType_Configuration))
							{
								if (rootKey == null)
								{
									return null;
								}

								using (var projectsKey = rootKey.OpenSubKey("Projects"))
								{
									foreach (var subKeyName in projectsKey.GetSubKeyNames())
									{
										using (var projectKey = projectsKey.OpenSubKey(subKeyName))
										{
											var projectExtension = projectKey.GetValue("DefaultProjectExtension", string.Empty, Microsoft.Win32.RegistryValueOptions.None) as string;

											if (!string.IsNullOrEmpty(projectExtension))
											{
												if (!projectExtension.StartsWith("."))
												{
													projectExtension = string.Format(".{0}", projectExtension);
												}

												projectExtensions.Add(projectExtension);
											}
										}
									}
								}
							}

							_projectExtensions = projectExtensions;
						}
					}
				}

				return _projectExtensions;
			}
		}
	}
}
