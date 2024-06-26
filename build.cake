//dotnet tool install Cake.Tool -g
#addin nuget:?package=Cake.FileHelpers
#tool nuget:?package=7-Zip.CommandLine
#addin nuget:?package=Cake.7zip
#addin nuget:?package=System.Security.Cryptography.Pkcs&Version=6.0.0
#addin nuget:?package=ISI.Cake.AddIn&loaddependencies=true

//mklink /D Secrets S:\
var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
var settings = GetSettings(settingsFullName);

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solutionFile = File("./src/ISI.VisualStudio.Extensions.sln");
var solution = ParseSolution(solutionFile);
var rootProjectFile = File("./src/ISI.VisualStudio.Extensions/ISI.VisualStudio.Extensions.csproj");
var rootAssemblyVersionKey = "ISI.VisualStudio.Extensions";
var artifactName = "ISI.VisualStudio.Extensions";
var artifactFileStoreUuid = new System.Guid("e9533a8c-0577-4883-a955-16f988cff620");
var artifactVersionFileStoreUuid = new System.Guid("25c1e176-6862-4f2d-acfa-28f247870039");

var buildDateTime = DateTime.UtcNow;
var buildDateTimeStamp = GetDateTimeStamp(buildDateTime);
var buildRevision = GetBuildRevision(buildDateTime);

var assemblyVersions = GetAssemblyVersionFiles(rootAssemblyVersionKey, buildRevision);
var assemblyVersion = assemblyVersions[rootAssemblyVersionKey].AssemblyVersion;

var buildDateTimeStampVersion = new ISI.Extensions.Scm.DateTimeStampVersion(buildDateTimeStamp, assemblyVersions[rootAssemblyVersionKey].AssemblyVersion);

Information("BuildDateTimeStampVersion: {0}", buildDateTimeStampVersion);

var buildArtifactVsixFile = File(string.Format("./Publish/{0}.{1}.vsix", artifactName, buildDateTimeStamp));

Task("Clean")
	.Does(() =>
	{
		Information("Cleaning Projects ...");

		foreach(var projectPath in new HashSet<string>(solution.Projects.Select(p => p.Path.GetDirectory().ToString())))
		{
			Information("Cleaning {0}", projectPath);
			CleanDirectories(projectPath + "/**/bin/" + configuration);
			CleanDirectories(projectPath + "/**/obj/" + configuration);
		}
	});

Task("NugetPackageRestore")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		Information("Restoring Nuget Packages ...");
		NuGetRestore(solutionFile);
	});

Task("Build")
	.IsDependentOn("NugetPackageRestore")
	.Does(() => 
	{
		SetAssemblyVersionFiles(assemblyVersions);

		var vsixManifestFile = File("./src/ISI.VisualStudio.Extensions/source.extension.vsixmanifest");

		var xPath = "//vsx:PackageManifest/vsx:Metadata/vsx:Identity/@Version";
		
		var xmlPeekSettings = new XmlPeekSettings();
		xmlPeekSettings.Namespaces.Add("vsx", "http://schemas.microsoft.com/developer/vsx-schema/2011");

		var xmlPokeSettings = new XmlPokeSettings();
		xmlPokeSettings.Namespaces.Add("vsx", "http://schemas.microsoft.com/developer/vsx-schema/2011");


		var vsixmanifestValue = XmlPeek(vsixManifestFile, xPath, xmlPeekSettings);
		XmlPoke(vsixManifestFile, xPath, assemblyVersions[rootAssemblyVersionKey].AssemblyVersion, xmlPokeSettings);

		try
		{
			MSBuild(solutionFile, configurator => configurator
				.SetConfiguration(configuration)
				.SetPlatformTarget(PlatformTarget.MSIL)
				.SetVerbosity(Verbosity.Quiet)
				.WithTarget("Rebuild"));

			var vsixFile = File("./src/ISI.VisualStudio.Extensions/bin/" + configuration + "/ISI.VisualStudio.Extensions.vsix");

			var publishDirectory = buildArtifactVsixFile.Path.GetDirectory().FullPath;
			if (!System.IO.Directory.Exists(publishDirectory))
			{
				System.IO.Directory.CreateDirectory(publishDirectory);
			}

			CopyFile(vsixFile, buildArtifactVsixFile);

			System.IO.File.WriteAllText(System.IO.Path.Combine(publishDirectory, string.Format("{0}.Current.DateTimeStamp.Version.txt", artifactName)), string.Format("{0}|{1}", buildDateTimeStamp, assemblyVersions[rootAssemblyVersionKey].AssemblyVersion));
		}
		finally
		{
			XmlPoke(vsixManifestFile, xPath, vsixmanifestValue, xmlPokeSettings);

			ResetAssemblyVersionFiles(assemblyVersions);
		}
	});

Task("Sign")
	.IsDependentOn("Build")
	.Does(() =>
	{
		if (settings.CodeSigning.DoCodeSigning && configuration.Equals("Release"))
		{
			//if(settings.TryGetValue("LocalTestCodeSigningRemoteCodeSigningServiceUrl", out var codeSigningRemoteCodeSigningServiceUrl))
			//{
			//	settings.CodeSigning.RemoteCodeSigningServiceUrl = codeSigningRemoteCodeSigningServiceUrl;
			//}

			using(var tempDirectory = GetNewTempDirectory())
			{
				var buildArtifactZipFile = File(string.Format("{0}/{1}.{2}.zip", tempDirectory.FullName, artifactName, buildDateTimeStamp));

				MoveFile(buildArtifactVsixFile, buildArtifactZipFile);

				SevenZip(m => m
				 .InExtractMode()
				 .WithArchive(buildArtifactZipFile)
				 .WithArchiveType(SwitchArchiveType.Zip)
				 .WithOutputDirectory(Directory(tempDirectory.FullName + "/" + artifactName)));

				var signableFiles = GetFiles(tempDirectory.FullName + "/**/ISI.VisualStudio.Extensions.dll");

				SignAssemblies(new ISI.Cake.Addin.CodeSigning.SignAssembliesUsingSettingsRequest()
				{
					AssemblyPaths = signableFiles,
					Settings = settings,
				});

				SevenZip(m => m
					.InAddMode()
					.WithArchive(buildArtifactVsixFile)
					.WithArchiveType(SwitchArchiveType.Zip)
					.WithFiles(new FilePath(tempDirectory.FullName + "/" + artifactName + "/*")));

				DeleteFile(buildArtifactZipFile);

				SignVsixes(new ISI.Cake.Addin.CodeSigning.SignVsixesUsingSettingsRequest()
				{
					VsixPaths = new FilePathCollection(new [] { buildArtifactVsixFile.Path }),
					Settings = settings,
				});
			}
		}
	});

Task("Publish")
	.IsDependentOn("Sign")
	.Does(() =>
	{
		var buildArtifactsApiKey = GetBuildArtifactsApiKey(new ISI.Cake.Addin.BuildArtifacts.GetBuildArtifactsApiKeyUsingSettingsActiveDirectoryRequest()
		{
			Settings = settings,
		}).BuildArtifactsApiKey;


		var simpleVsixFile = File(string.Format("./Publish/{0}.vsix", artifactName));
		if(FileExists(simpleVsixFile))
		{
			DeleteFile(simpleVsixFile);
		}
		CopyFile(buildArtifactVsixFile, simpleVsixFile);

		var vsExtensionsApiKey = GetVsExtensionsAuthenticationToken(new ISI.Cake.Addin.VsExtensions.GetVsExtensionsAuthenticationTokenRequest()
		{
			VsExtensionsApiUri = GetNullableUri(settings.VsExtensions.ApiUrl),
			UserName = settings.ActiveDirectory.GetDomainUserName(),
			Password = settings.ActiveDirectory.Password,
		}).AuthenticationToken;

		UploadVsExtension(new ISI.Cake.Addin.VsExtensions.UploadVsExtensionRequest()
		{
			VsExtensionsApiUri = GetNullableUri(settings.VsExtensions.ApiUrl),
			VsExtensionsApiKey = vsExtensionsApiKey,
			VsExtensionPath = simpleVsixFile,
		});
		
		UploadBuildArtifact(new ISI.Cake.Addin.BuildArtifacts.UploadBuildArtifactRequest()
		{
			BuildArtifactsApiUri = GetNullableUri(settings.BuildArtifacts.ApiUrl),
			BuildArtifactsApiKey = buildArtifactsApiKey,
			SourceFileName = simpleVsixFile.Path.FullPath,
			BuildArtifactName = artifactName,
			DateTimeStampVersion = buildDateTimeStampVersion,
		});

		SetBuildArtifactEnvironmentDateTimeStampVersion(new ISI.Cake.Addin.BuildArtifacts.SetBuildArtifactEnvironmentDateTimeStampVersionRequest()
		{
			BuildArtifactsApiUri = GetNullableUri(settings.BuildArtifacts.ApiUrl),
			BuildArtifactsApiKey = buildArtifactsApiKey,
			BuildArtifactName = artifactName,
			Environment = "Build",
			DateTimeStampVersion = buildDateTimeStampVersion,
		});
	});

Task("Production-Publish")
	.Does(() =>
	{
		var buildArtifactsApiKey = GetBuildArtifactsApiKey(new ISI.Cake.Addin.BuildArtifacts.GetBuildArtifactsApiKeyUsingSettingsActiveDirectoryRequest()
		{
			Settings = settings,
		}).BuildArtifactsApiKey;

		var dateTimeStampVersion = GetBuildArtifactEnvironmentDateTimeStampVersion(new ISI.Cake.Addin.BuildArtifacts.GetBuildArtifactEnvironmentDateTimeStampVersionRequest()
		{
			BuildArtifactsApiUri = GetNullableUri(settings.BuildArtifacts.ApiUrl),
			BuildArtifactsApiKey = buildArtifactsApiKey,
			BuildArtifactName = artifactName,
			Environment = "Build",
		}).DateTimeStampVersion;

		using(var tempDirectory = GetNewTempDirectory())
		{
			var simpleVsixFile = File(tempDirectory.FullName + "/" + string.Format("{0}.vsix", artifactName));

			DownloadBuildArtifact(new ISI.Cake.Addin.BuildArtifacts.DownloadBuildArtifactRequest()
			{
				BuildArtifactsApiUri = GetNullableUri(settings.BuildArtifacts.ApiUrl),
				BuildArtifactsApiKey = buildArtifactsApiKey,
				BuildArtifactName = artifactName,
				DateTimeStampVersion = dateTimeStampVersion,
				TargetFileName = simpleVsixFile.Path.FullPath,
			});

			PublishVsixExtension(new ISI.Cake.Addin.VsExtensions.PublishVsixExtensionUsingSettingsRequest()
			{
				Settings = settings,

				InternalName = "ISI-VisualStudio-Extensions",
				Version = dateTimeStampVersion.Version.ToString(),

				VsixFullName = simpleVsixFile.Path.FullPath,
				VsixManifestFullName = MakeAbsolute(File("./src/ISI.VisualStudio.Extensions/source.extension.vsixmanifest")).FullPath,

				ReadMeFullName = MakeAbsolute(File("./README.md")).FullPath,
				PriceCategory = ISI.Cake.Addin.VsExtensions.PublishVsixExtensionRequestPricingCategory.Free,
				Categories = new[]
				{
					"Coding",
					"Data",
					"Modeling",
				},
			});
		}

		SetBuildArtifactEnvironmentDateTimeStampVersion(new ISI.Cake.Addin.BuildArtifacts.SetBuildArtifactEnvironmentDateTimeStampVersionRequest()
		{
			BuildArtifactsApiUri = GetNullableUri(settings.BuildArtifacts.ApiUrl),
			BuildArtifactsApiKey = buildArtifactsApiKey,
			BuildArtifactName = artifactName,
			Environment = "Production",
			DateTimeStampVersion = buildDateTimeStampVersion,
		});
	});

Task("Default")
	.IsDependentOn("Publish")
	.Does(() => 
	{
		Information("No target provided. Starting default task");
	});

using(GetNugetLock())
{
	using(GetSolutionLock())
	{
		RunTarget(target);
	}
}