// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//////////////////////////////////////////////////////////////////////
// ADDINS
//////////////////////////////////////////////////////////////////////

#addin "nuget:?package=Cake.FileHelpers&version=3.1.0"
#addin "nuget:?package=Cake.Codecov&version=0.5.0"
//#addin "nuget:?package=Cake.Coverlet&version=2.2.0-g8e2ea2e891"

//////////////////////////////////////////////////////////////////////
// MODULES
//////////////////////////////////////////////////////////////////////

#module nuget:?package=Cake.DotNetTool.Module&version=0.1.0

//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

#tool "nuget:?package=vswhere&version=2.5.9"
#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=Codecov&version=1.1.0"

//////////////////////////////////////////////////////////////////////
// DOTNET TOOLS
//////////////////////////////////////////////////////////////////////

#tool "dotnet:?package=SignClient&version=1.0.82"
// #tool "dotnet:?package=coverlet.console&version=1.4.0"
#tool "dotnet:?package=nbgv&version=2.3.38"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
if (string.IsNullOrWhiteSpace(target))
{
    target = "Default";
}

var configuration = Argument("configuration", "Release");
if (string.IsNullOrWhiteSpace(configuration))
{
    configuration = "Release";
}

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Should MSBuild treat any errors as warnings?
var treatWarningsAsErrors = false;

// Build configuration
var local = BuildSystem.IsLocalBuild;
var isPullRequest = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER"));
var isRepository = StringComparer.OrdinalIgnoreCase.Equals("reactiveui/spalt", TFBuild.Environment.Repository.RepoName);

var msBuildPath = VSWhereLatest().CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");

var informationalVersion = EnvironmentVariable("GitAssemblyInformationalVersion");

// OpenCover file location
var testCoverageOutputFile = MakeAbsolute(File(testsArtifactDirectory + "OpenCover.xml"));

// Artifacts
var artifactDirectory = "./artifacts/";
var testsArtifactDirectory = artifactDirectory + "tests/";
var binariesArtifactDirectory = artifactDirectory + "binaries/";
var packagesArtifactDirectory = artifactDirectory + "packages/";

// Whitelisted Packages
var packageWhitelist = new[] 
{ 
    "Splat",
};

var packageTestWhitelist = new[]
{
    "Splat.Tests", 
};

var testFrameworks = new[] { "netcoreapp2.1", "net472" };


// Define global marcos.
Action Abort = () => { throw new Exception("a non-recoverable fatal error occurred."); };

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup(context =>
{
    if (!IsRunningOnWindows())
    {
        throw new NotImplementedException("Splat will only build on Windows (w/Xamarin installed) because it's not possible to target UWP, WPF and Windows Forms from UNIX.");
    }

    Information("Building version {0} of Splat.", informationalVersion);

    CleanDirectories(artifactDirectory);
    CreateDirectory(testsArtifactDirectory);
    CreateDirectory(binariesArtifactDirectory);
    CreateDirectory(packagesArtifactDirectory);

    StartProcess(Context.Tools.Resolve("nbgv.*").ToString(), "cloud");
});

Teardown(context =>
{
    // Executed AFTER the last task.
});

//////////////////////////////////////////////////////////////////////
// HELPER METHODS
//////////////////////////////////////////////////////////////////////
Action<string, string, bool> Build = (solution, packageOutputPath, doNotOptimise) =>
{
    Information("Building {0} using {1}", solution, msBuildPath);

    var msBuildSettings = new MSBuildSettings() {
            ToolPath = msBuildPath,
            ArgumentCustomization = args => args.Append("/m /NoWarn:VSX1000"),
            NodeReuse = false,
            Restore = true
        }
        .WithProperty("TreatWarningsAsErrors", treatWarningsAsErrors.ToString())
        .SetConfiguration(configuration)     
        .WithTarget("build;pack")                   
        .SetVerbosity(Verbosity.Minimal);

    if (!string.IsNullOrWhiteSpace(packageOutputPath))
    {
        msBuildSettings = msBuildSettings.WithProperty("PackageOutputPath",  MakeAbsolute(Directory(packageOutputPath)).ToString().Quote());
    }

    if (doNotOptimise)
    {
        msBuildSettings = msBuildSettings.WithProperty("Optimize",  "False");
    }

    MSBuild(solution, msBuildSettings);
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Build")
    .Does (() =>
{

    // Clean the directories since we'll need to re-generate the debug type.
    CleanDirectories($"./src/**/obj/{configuration}");
    CleanDirectories($"./src/**/bin/{configuration}");

    foreach(var packageName in packageWhitelist)
    {
        Build($"./src/{packageName}/{packageName}.csproj", packagesArtifactDirectory, false);
    }

    CopyFiles(GetFiles($"./src/**/bin/{configuration}/**/*"), Directory(binariesArtifactDirectory), true);
});

Task("RunUnitTests")
    .Does(() =>
{
    // Clean the directories since we'll need to re-generate the debug type.
    CleanDirectories($"./src/**/obj/{configuration}");
    CleanDirectories($"./src/**/bin/{configuration}");

    // var coverletSettings = new CoverletSettings {
    //         CollectCoverage = true,
    //         CoverletOutputFormat = CoverletOutputFormat.opencover,
    //         CoverletOutputDirectory = Directory(testsArtifactDirectory),
    //         MergeWithFile = testCoverageOutputFile,
    //         CoverletOutputName = "opencover.xml"
    //     }
    //     .WithInclusion("[Splat*]*")
    //     .WithFilter("[*.Tests*]*")
    //     .WithFilter("[*.Tests*]*")
    //     //.ExcludeByAttribute("*.ExcludeFromCodeCoverage*")
    //     .WithFileExclusion("*/*Designer.cs")
    //     .WithFileExclusion("*/*.g.cs")
    //     .WithFileExclusion("*/*.g.i.cs")
    //     .WithFileExclusion("*ApprovalTests*");

    foreach (var packageName in packageTestWhitelist)
    {
        var projectName = $"./src/{packageName}/{packageName}.csproj";
        Build(projectName, null, true);

        foreach (var testFramework in testFrameworks)
        {
            var testSettings = new DotNetCoreTestSettings {
                NoBuild = true,
                Framework = testFramework,
                Configuration = configuration,
                ResultsDirectory = testsArtifactDirectory,
                Logger = $"trx;LogFileName=testresults-{testFramework}.trx",
                // TestAdapterPath = GetDirectories("./tools/xunit.runner.console*/**/net472").FirstOrDefault(),        
            };

            // var testFile = $"./src/{packageName}/bin/{configuration}/{testFramework}/{packageName}.dll";
            // Information($"Generate Coverlet information for {testFile} for {testFramework}");
            // Coverlet(testFile, projectName, testSettings, coverletSettings);

            DotNetCoreTest(projectName, testSettings);
        }
    }

//    ReportGenerator(testCoverageOutputFile, testsArtifactDirectory + "Report/");
// })
// .ReportError(exception =>
// {
//     var apiApprovals = GetFiles("./**/ApiApprovalTests.*");
//     CopyFiles(apiApprovals, artifactDirectory);
});

Task("SignPackages")
    .IsDependentOn("Build")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    if(EnvironmentVariable("SIGNCLIENT_SECRET") == null)
    {
        throw new Exception("Client Secret not found, not signing packages.");
    }

    var nupkgs = GetFiles(packagesArtifactDirectory + "*.nupkg");
    foreach(FilePath nupkg in nupkgs)
    {
        var packageName = nupkg.GetFilenameWithoutExtension();
        Information($"Submitting {packageName} for signing");

        StartProcess(Context.Tools.Resolve("SignClient.*").ToString(), new ProcessSettings {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            Arguments = new ProcessArgumentBuilder()
                .Append("sign")
                .AppendSwitch("-c", "./SignPackages.json")
                .AppendSwitch("-i", nupkg.FullPath)
                .AppendSwitch("-r", EnvironmentVariable("SIGNCLIENT_USER"))
                .AppendSwitch("-s", EnvironmentVariable("SIGNCLIENT_SECRET"))
                .AppendSwitch("-n", "ReactiveUI")
                .AppendSwitch("-d", "ReactiveUI")
                .AppendSwitch("-u", "https://reactiveui.net")
            });

        Information($"Finished signing {packageName}");
    }
    
    Information("Sign-package complete");
});

Task("Package")
    .IsDependentOn("Build")
    .IsDependentOn("SignPackages")
    .Does (() =>
{
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Package")
    .IsDependentOn("RunUnitTests")
    .Does (() =>
{
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
