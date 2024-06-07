#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;

namespace ISI.VisualStudio.Extensions
{
	public partial class LaunchSettings_Helper
	{
		public void CheckProjectPortReservations(Community.VisualStudio.Toolkit.SolutionItem solutionItem)
		{
			foreach (var child in solutionItem.Children)
			{
				switch (child)
				{
					case Community.VisualStudio.Toolkit.PhysicalFile physicalFile:
						break;
					case Community.VisualStudio.Toolkit.PhysicalFolder physicalFolder:
						break;
					case Community.VisualStudio.Toolkit.Project project:
						CheckProjectPortReservations(project);
						break;
					case Community.VisualStudio.Toolkit.Solution solution:
						break;
					case Community.VisualStudio.Toolkit.SolutionFolder solutionFolder:
						CheckProjectPortReservations(solutionFolder);
						break;
					case Community.VisualStudio.Toolkit.VirtualFolder virtualFolder:
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(child));
				}
			}
		}

		public void CheckProjectPortReservations(Community.VisualStudio.Toolkit.Project project)
		{
			var projectName = System.IO.Path.GetFileNameWithoutExtension(project.FullPath);

			var launchSettingsJsonFullName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(project.FullPath), "Properties", "launchSettings.json");

			if (System.IO.File.Exists(launchSettingsJsonFullName))
			{
				var launchSettingsJsonNode = System.Text.Json.Nodes.JsonNode.Parse(System.IO.File.ReadAllText(launchSettingsJsonFullName));

				var profilesJsonNode = launchSettingsJsonNode["profiles"];

				if (profilesJsonNode != null)
				{
					var launchSettingJsonNode = profilesJsonNode[projectName];

					if (launchSettingJsonNode != null)
					{
						var applicationUrlJsonNode = launchSettingJsonNode["applicationUrl"];

						if (applicationUrlJsonNode != null)
						{
							var applicationUris = applicationUrlJsonNode.GetValue<string>().Split(new[] { ';' }).ToNullCheckedArray(applicationUrl => new UriBuilder(applicationUrl));

							if (applicationUris.Any())
							{
								var trySetPortReservationsResponse = ISI.Extensions.PortReservations.TrySetPortReservations(projectName, applicationUris.ToNullCheckedArray(applicationUri => applicationUri.Port));

								if (!trySetPortReservationsResponse.Success)
								{
									var messageBox = new Community.VisualStudio.Toolkit.MessageBox();

									var message = $"Port: {trySetPortReservationsResponse.UsedPorts.First()} is already reserved for another project, would you like it changed to another port ?";
									if (trySetPortReservationsResponse.UsedPorts.Length != 1)
									{
										message = $"Ports: {string.Join(", ", trySetPortReservationsResponse.UsedPorts)} are already reserved for another project, would you like them changed to other ports ?";
									}

									if (messageBox.Show(message, icon: Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_QUERY, buttons: Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_YESNO, defaultButton: Microsoft.VisualStudio.Shell.Interop.OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST) == Microsoft.VisualStudio.VSConstants.MessageBoxResult.IDYES)
									{
										foreach (var applicationUri in applicationUris.Where(applicationUri => trySetPortReservationsResponse.UsedPorts.Contains(applicationUri.Port)))
										{
											var port = ISI.Extensions.PortReservations.GetNewPortReservation(projectName);

											applicationUri.Port = port;
										}

										launchSettingJsonNode["applicationUrl"] = string.Join(";", applicationUris.Select(applicationUri => applicationUri.Uri.ToString()));

										System.IO.File.WriteAllText(launchSettingsJsonFullName, launchSettingsJsonNode.ToJsonString(new System.Text.Json.JsonSerializerOptions()
										{
											WriteIndented = true,
										}));
									}
								}
							}
						}
					}
				}
			}
		}
	}
}