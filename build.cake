//////////////////////////////////////////////////////////////////////
// ADDINS
//////////////////////////////////////////////////////////////////////

#addin "Cake.FileHelpers"

//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

#tool GitVersion.CommandLine
#tool GitLink

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
if (string.IsNullOrWhiteSpace(target))
{
    target = "Default";
}

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// should MSBuild & GitLink treat any errors as warnings.
var treatWarningsAsErrors = false;

// Get whether or not this is a local build.
var local = BuildSystem.IsLocalBuild;
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();

//var isRunningOnBitrise = Bitrise.IsRunningOnBitrise;
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;

var isRepository = StringComparer.OrdinalIgnoreCase.Equals("paulcbetts/splat", AppVeyor.Environment.Repository.Name);

// Parse release notes.
var releaseNotes = ParseReleaseNotes("RELEASENOTES.md");

// Get version.
var version = releaseNotes.Version.ToString();
var epoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
var gitSha = GitVersion().Sha;

var semVersion = local ? string.Format("{0}.{1}", version, epoch) : string.Format("{0}.{1}", version, epoch);

// Define directories.
var artifactDirectory = "./artifacts/";

// Define global marcos.
Action Abort = () => { throw new Exception("a non-recoverable fatal error occurred."); };

Action<string> RestorePackages = (solution) =>
{
    NuGetRestore(solution);
};

Action<string, string> Package = (nuspec, basePath) =>
{
    CreateDirectory(artifactDirectory);

    Information("Packaging {0} using {1} as the BasePath.", nuspec, basePath);

    NuGetPack(nuspec, new NuGetPackSettings {
        Authors                  = new [] { "Paul Betts" },
        Owners                   = new [] { "paulcbetts" },

        ProjectUrl               = new Uri("https://github.com/paulcbetts/splat/"),
        IconUrl                  = new Uri("http://f.cl.ly/items/1307401C3x2g3F2p2Z36/Logo.png"),
        LicenseUrl               = new Uri("https://github.com/paulcbetts/splat/blob/master/COPYING"),
        Copyright                = "Copyright (c) Akavache Contributors",
        RequireLicenseAcceptance = false,

        Version                  = semVersion,
        Tags                     = new [] {  "portable"  },
        ReleaseNotes             = new List<string>(releaseNotes.Notes),

        Symbols                  = true,
        Verbosity                = NuGetVerbosity.Detailed,
        OutputDirectory          = artifactDirectory,
        BasePath                 = basePath,
    });
};

Action<string> SourceLink = (solutionFileName) =>
{
    GitLink("./", new GitLinkSettings() {
        RepositoryUrl = "https://github.com/paulcbetts/splat",
        SolutionFileName = solutionFileName,
        
        // nb: I would love to set this to `treatErrorsAsWarnings` which defaults to `false` but GitLink trips over Akavache.Tests :/
        // Handling project 'Akavache.Tests'
        //   No pdb file found for 'Akavache.Tests', is project built in 'Release' mode with pdb files enabled? Expected file is 'C:\Dropbox\OSS\akavache\Akavache\src\Akavache.Tests\Akavache.Tests.pdb'
        ErrorsAsWarnings = true, 
    });
};


///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup(context =>
{
    Information("Building version {0} of Splat.", semVersion);
});

Teardown(context =>
{
    // Executed AFTER the last task.
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("RestorePackages")
    .IsDependentOn("UpdateAssemblyInfo")
    .Does (() =>
{
    Action<string> build = (filename) =>
    {
        var solution = filename;

        // UWP (project.json) needs to be restored before it will build.
        RestorePackages(solution);

        Information("Building {0}", solution);

        MSBuild(solution, new MSBuildSettings()
            .SetConfiguration("Release")
            .WithProperty("NoWarn", "1591") // ignore missing XML doc warnings
            .WithProperty("TreatWarningsAsErrors", treatWarningsAsErrors.ToString())
            .SetVerbosity(Verbosity.Minimal)
            .SetNodeReuse(false));

        SourceLink(solution);
    };

    build("src/Splat.sln");
});

Task("UpdateAppVeyorBuildNumber")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(semVersion);
});

Task("UpdateAssemblyInfo")
    .IsDependentOn("UpdateAppVeyorBuildNumber")
    .Does (() =>
{
    var file = "src/Splat/GlobalAssemblyInfo.cs";

    CreateAssemblyInfo(file, new AssemblyInfoSettings {
        Product = "Splat",
        Version = version,
        FileVersion = version,
        InformationalVersion = semVersion,
        Copyright = "Copyright (c) Paul Betts"
    });
});

Task("RestorePackages").Does (() =>
{
    RestorePackages("src/Splat.sln");
});

Task("RunUnitTests")
    .IsDependentOn("Build")
    .Does(() =>
{
    // XUnit2("./src/Splat.Tests/bin/x64/Release/Splat.Tests.dll", new XUnit2Settings {
    //     OutputDirectory = artifactDirectory,
    //     XmlReportV1 = false,
    //     NoAppDomain = true
    // });
});

Task("Package")
    .IsDependentOn("Build")
    .IsDependentOn("RunUnitTests")
    .Does (() =>
{
    Package("src/Splat.nuspec", "src");
});

Task("Publish")
    .IsDependentOn("RunUnitTests")
    .IsDependentOn("Package")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
    .Does (() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("NUGET_APIKEY");
    if (string.IsNullOrEmpty(apiKey))
    {
        throw new Exception("The NUGET_APIKEY environment variable is not defined.");
    }

    var source = EnvironmentVariable("NUGET_SOURCE");
    if (string.IsNullOrEmpty(source))
    {
        throw new Exception("The NUGET_SOURCE environment variable is not defined.");
    }

    // only push whitelisted packages.
    foreach(var package in new[] { "Splat" })
    {
        // only push the package which was created during this build run.
        var packagePath = artifactDirectory + File(string.Concat(package, ".", semVersion, ".nupkg"));
        var symbolsPath = artifactDirectory + File(string.Concat(package, ".", semVersion, ".symbols.nupkg"));

        // Push the package.
        NuGetPush(packagePath, new NuGetPushSettings {
            Source = source,
            ApiKey = apiKey
        });

        // Push the symbols
        NuGetPush(symbolsPath, new NuGetPushSettings {
           Source = source,
           ApiKey = apiKey
        });

    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Publish")
    .Does (() =>
{
});


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
