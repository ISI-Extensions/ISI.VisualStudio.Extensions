//dotnet tool install Cake.Tool -g
#addin nuget:?package=Cake.FileHelpers&version=4.0.1
#addin nuget:?package=ISI.Cake.AddIn&loaddependencies=true

//mklink /D Secrets S:\
var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
var settings = GetSettings(settingsFullName);

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solutionPath = File("./ISI.VisualStudio.Extensions.sln");
var solution = ParseSolution(solutionPath);

var assemblyVersionFile = File("./ISI.VisualStudio.Extensions.Version.cs");

var buildDateTime = DateTime.UtcNow;
var buildDateTimeStamp = GetDateTimeStamp(buildDateTime);
var buildRevision = GetBuildRevision(buildDateTime);
var assemblyVersion = GetAssemblyVersion(ParseAssemblyInfo(assemblyVersionFile).AssemblyVersion, buildRevision);
Information("AssemblyVersion: {0}", assemblyVersion);

Task("Clean")
	.Does(() =>
	{
		foreach(var projectPath in solution.Projects.Select(p => p.Path.GetDirectory()))
		{
			Information("Cleaning {0}", projectPath);
			CleanDirectories(projectPath + "/**/bin/" + configuration);
			CleanDirectories(projectPath + "/**/obj/" + configuration);
		}

		CleanDirectories(nugetPackOutputDirectory);

		Information("Cleaning Projects ...");
	});

Task("NugetPackageRestore")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		Information("Restoring Nuget Packages ...");
		NuGetRestore(solutionPath);
	});

Task("Build")
	.IsDependentOn("NugetPackageRestore")
	.Does(() => 
	{
		CreateAssemblyInfo(assemblyVersionFile, new AssemblyInfoSettings()
		{
			Version = assemblyVersion,
		});

		MSBuild(solutionPath, configurator => configurator
			.SetConfiguration(configuration)
			.SetVerbosity(Verbosity.Quiet)
			.SetMSBuildPlatform(MSBuildPlatform.Automatic)
			.SetPlatformTarget(PlatformTarget.MSIL)
			.WithTarget("Build"));

		CreateAssemblyInfo(assemblyVersionFile, new AssemblyInfoSettings()
		{
			Version = GetAssemblyVersion(assemblyVersion, "*"),
		});
	});

Task("Sign")
	.IsDependentOn("Build")
	.Does(() =>
	{
		if (settings.CodeSigning.DoCodeSigning && configuration.Equals("Release"))
		{
			
		}
	});

Task("Publish")
	.IsDependentOn("Sign")
	.Does(() =>
	{		
		
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