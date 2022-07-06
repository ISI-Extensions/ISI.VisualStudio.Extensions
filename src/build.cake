//dotnet tool install Cake.Tool -g
#addin nuget:?package=Cake.FileHelpers
#tool nuget:?package=7-Zip.CommandLine
#addin nuget:?package=Cake.7zip
#addin nuget:?package=ISI.Cake.AddIn&loaddependencies=true

//mklink /D Secrets S:\
var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
var settings = GetSettings(settingsFullName);

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solutionFile = File("./ISI.VisualStudio.Extensions.sln");
var solution = ParseSolution(solutionFile);
var rootProjectFile = File("./ISI.VisualStudio.Extensions/ISI.VisualStudio.Extensions.csproj");
var rootAssemblyVersionKey = "ISI.VisualStudio.Extensions";
var artifactName = "ISI.VisualStudio.Extensions";
var artifactFileStoreUuid = new System.Guid("e9533a8c-0577-4883-a955-16f988cff620");
var artifactVersionFileStoreUuid = new System.Guid("25c1e176-6862-4f2d-acfa-28f247870039");

var buildDateTime = DateTime.UtcNow;
var buildDateTimeStamp = GetDateTimeStamp(buildDateTime);
var buildRevision = GetBuildRevision(buildDateTime);
Information("BuildRevision: {0}", buildRevision);

var assemblyVersions = GetAssemblyVersionFiles(solution, rootAssemblyVersionKey, buildRevision);

var buildArtifactVsixFile = File(string.Format("../Publish/{0}.{1}.vsix", artifactName, buildDateTimeStamp));

Task("Clean")
	.Does(() =>
	{
		foreach(var projectPath in solution.Projects.Select(p => p.Path.GetDirectory()))
		{
			Information("Cleaning {0}", projectPath);
			CleanDirectories(projectPath + "/**/bin/" + configuration);
			CleanDirectories(projectPath + "/**/obj/" + configuration);
		}

		Information("Cleaning Projects ...");
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

		var vsixmanifestFile = File("./ISI.VisualStudio.Extensions/source.extension.vsixmanifest");

		var xPath = "//vsx:PackageManifest/vsx:Metadata/vsx:Identity/@Version";
		
		var xmlPeekSettings = new XmlPeekSettings();
		xmlPeekSettings.Namespaces.Add("vsx", "http://schemas.microsoft.com/developer/vsx-schema/2011");

		var xmlPokeSettings = new XmlPokeSettings();
		xmlPokeSettings.Namespaces.Add("vsx", "http://schemas.microsoft.com/developer/vsx-schema/2011");


		var vsixmanifestValue = XmlPeek(vsixmanifestFile, xPath, xmlPeekSettings);
		XmlPoke(vsixmanifestFile, xPath, assemblyVersions[rootAssemblyVersionKey].AssemblyVersion, xmlPokeSettings);

		try
		{
			MSBuild(solutionFile, configurator => configurator
				.SetConfiguration(configuration)
				.SetPlatformTarget(PlatformTarget.MSIL)
				.SetVerbosity(Verbosity.Quiet)
				.WithTarget("Rebuild"));

			var vsixFile = File("./ISI.VisualStudio.Extensions/bin/" + configuration + "/ISI.VisualStudio.Extensions.vsix");

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
			XmlPoke(vsixmanifestFile, xPath, vsixmanifestValue, xmlPokeSettings);

			ResetAssemblyVersionFiles(assemblyVersions);
		}
	});

Task("Sign")
	.IsDependentOn("Build")
	.Does(() =>
	{
		if (settings.CodeSigning.DoCodeSigning && configuration.Equals("Release"))
		{
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

				SignAssemblies(new ISI.Cake.Addin.CodeSigning.SignAssembliesRequest()
				{
					AssemblyPaths = signableFiles,
					RemoteCodeSigningServiceUri = GetNullableUri(settings.CodeSigning.RemoteCodeSigningServiceUrl),
					RemoteCodeSigningServicePassword = settings.CodeSigning.RemoteCodeSigningServicePassword,
					CodeSigningCertificateTokenCertificateFileName = settings.CodeSigning.Token.CertificateFileName,
					CodeSigningCertificateTokenCryptographicProvider = settings.CodeSigning.Token.CryptographicProvider,
					CodeSigningCertificateTokenContainerName = settings.CodeSigning.Token.ContainerName,
					CodeSigningCertificateTokenPassword = settings.CodeSigning.Token.Password,
					TimeStampUri = GetNullableUri(settings.CodeSigning.TimeStampUrl),
					TimeStampDigestAlgorithm = SignToolDigestAlgorithm.Sha256,
					CertificatePath = GetNullableFile(settings.CodeSigning.CertificateFileName),
					CertificatePassword = settings.CodeSigning.CertificatePassword,
					CertificateFingerprint = settings.CodeSigning.CertificateFingerprint,
					DigestAlgorithm = SignToolDigestAlgorithm.Sha256,
				});

				SevenZip(m => m
					.InAddMode()
					.WithArchive(buildArtifactVsixFile)
					.WithArchiveType(SwitchArchiveType.Zip)
					.WithFiles(new FilePath(tempDirectory.FullName + "/" + artifactName + "/*")));

				DeleteFile(buildArtifactZipFile);

				SignVsixes(new ISI.Cake.Addin.CodeSigning.SignVsixesRequest()
				{
					VsixPaths = new FilePathCollection(new [] { buildArtifactVsixFile.Path }),
					RemoteCodeSigningServiceUri = GetNullableUri(settings.CodeSigning.RemoteCodeSigningServiceUrl),
					RemoteCodeSigningServicePassword = settings.CodeSigning.RemoteCodeSigningServicePassword,
					CodeSigningCertificateTokenCertificateFileName = settings.CodeSigning.Token.CertificateFileName,
					CodeSigningCertificateTokenCryptographicProvider = settings.CodeSigning.Token.CryptographicProvider,
					CodeSigningCertificateTokenContainerName = settings.CodeSigning.Token.ContainerName,
					CodeSigningCertificateTokenPassword = settings.CodeSigning.Token.Password,
					TimeStampUri = GetNullableUri(settings.CodeSigning.TimeStampUrl),
					TimeStampDigestAlgorithm = SignToolDigestAlgorithm.Sha256,
					CertificatePath = GetNullableFile(settings.CodeSigning.CertificateFileName),
					CertificatePassword = settings.CodeSigning.CertificatePassword,
					CertificateFingerprint = settings.CodeSigning.CertificateFingerprint,
					DigestAlgorithm = SignToolDigestAlgorithm.Sha256,
				});
			}
		}
	});

Task("Publish")
	.IsDependentOn("Sign")
	.Does(() =>
	{
		var authenticationToken = GetAuthenticationToken(new ISI.Cake.Addin.Scm.GetAuthenticationTokenRequest()
		{
			ScmManagementUrl = settings.Scm.WebServiceUrl,
			UserName = settings.ActiveDirectory.UserName,
			Password = settings.ActiveDirectory.Password,
		}).AuthenticationToken;

		UploadArtifact(new ISI.Cake.Addin.BuildArtifacts.UploadArtifactRequest()
		{
			BuildArtifactManagementUrl = settings.Scm.WebServiceUrl,
			AuthenticationToken = authenticationToken,
			SourceFileName = buildArtifactVsixFile.Path.FullPath,
			ArtifactName = artifactName,
			DateTimeStamp = buildDateTimeStamp,
		});

		SetArtifactEnvironmentDateTimeStampVersion(new ISI.Cake.Addin.BuildArtifacts.SetArtifactEnvironmentDateTimeStampVersionRequest()
		{
			BuildArtifactManagementUrl = settings.Scm.WebServiceUrl,
			AuthenticationToken = authenticationToken,
			ArtifactName = artifactName,
			Environment = "Build",
			DateTimeStampVersion = string.Format("{0}|{1}", buildDateTimeStamp, assemblyVersions[rootAssemblyVersionKey].AssemblyVersion),
		});

		var simpleVsixFile = File(string.Format("../Publish/{0}.vsix", artifactName));
		if(FileExists(simpleVsixFile))
		{
			DeleteFile(simpleVsixFile);
		}
		CopyFile(buildArtifactVsixFile, simpleVsixFile);

	});

Task("Production-Deploy")
	.Does(() => 
	{
		var authenticationToken = GetAuthenticationToken(new ISI.Cake.Addin.Scm.GetAuthenticationTokenRequest()
		{
			ScmManagementUrl = settings.Scm.WebServiceUrl,
			UserName = settings.ActiveDirectory.UserName,
			Password = settings.ActiveDirectory.Password,
		}).AuthenticationToken;

		var dateTimeStampVersion = GetBuildArtifactEnvironmentDateTimeStampVersion(new ISI.Cake.Addin.BuildArtifacts.GetBuildArtifactEnvironmentDateTimeStampVersionRequest()
		{
			BuildArtifactManagementUrl = settings.Scm.WebServiceUrl,
			AuthenticationToken = authenticationToken,
			ArtifactName = artifactName,
			Environment = "Build",
		}).DateTimeStampVersion;

		Information(string.Format("SoftwareVersion => {0}", dateTimeStampVersion.Version.ToString()));

		using(var tempDirectory = GetNewTempDirectory())
		{
			var artifactFileName = string.Format("{0}.{1}.vsix", artifactName, buildDateTimeStamp);
			var artifactFullName = System.IO.Path.Combine(tempDirectory.FullName, artifactFileName);

			DownloadArtifact(new ISI.Cake.Addin.BuildArtifacts.DownloadArtifactRequest()
			{
				BuildArtifactManagementUrl = settings.Scm.WebServiceUrl,
				AuthenticationToken = authenticationToken,
				ArtifactName = artifactName,
				DateTimeStamp = dateTimeStampVersion.DateTimeStamp,
				TargetFileName = artifactFullName,
			});

			var artifactVersionFullName = System.IO.Path.Combine(tempDirectory.FullName, string.Format("{0}.Current.DateTimeStamp.Version.txt", artifactName));
			FileWriteText(artifactVersionFullName, dateTimeStampVersion.Formatted());

			Information("Uploading Artifact");

			UploadFile(new ISI.Cake.Addin.FileStore.UploadFileRequest()
			{
				FileStoreUrl = settings.FileStore.Url,
				UserName = settings.FileStore.UserName,
				Password = settings.FileStore.Password,
				Version = buildDateTimeStamp,
				FileStoreUuid = artifactFileStoreUuid,
				FileName = artifactFullName,
			});

			UploadFile(new ISI.Cake.Addin.FileStore.UploadFileRequest()
			{
				FileStoreUrl = settings.FileStore.Url,
				UserName = settings.FileStore.UserName,
				Password = settings.FileStore.Password,
				Version = buildDateTimeStamp,
				FileStoreUuid = artifactVersionFileStoreUuid,
				FileName = artifactVersionFullName,
			});
		
			Information(string.Format("curl https://www.isi-net.com/file-store/download/{0:D}/{1}.Current.DateTimeStamp.Version.txt --output {1}.Current.DateTimeStamp.Version.txt", artifactVersionFileStoreUuid, artifactName));
			Information(string.Format("curl https://www.isi-net.com/file-store/download/{0:D}/{1}.vsix --output {1}.vsix", artifactFileStoreUuid, artifactName));

			SetArtifactEnvironmentDateTimeStampVersion(new ISI.Cake.Addin.BuildArtifacts.SetArtifactEnvironmentDateTimeStampVersionRequest()
			{
				BuildArtifactManagementUrl = settings.Scm.WebServiceUrl,
				AuthenticationToken = authenticationToken,
				ArtifactName = artifactName,
				Environment = "Production",
				DateTimeStampVersion = dateTimeStampVersion.Value,
			});
		}
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