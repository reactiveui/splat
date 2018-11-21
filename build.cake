// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//////////////////////////////////////////////////////////////////////
// ADDINS
//////////////////////////////////////////////////////////////////////

#addin "nuget:?package=Cake.FileHelpers&version=3.1.0"
#addin "nuget:?package=Cake.Powershell&version=0.4.7"

//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

#tool "nuget:?package=vswhere&version=2.5.2"

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

// Should MSBuild treat any errors as warnings?
var treatWarningsAsErrors = false;

// Build configuration
var local = BuildSystem.IsLocalBuild;
var isPullRequest = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER"));
var isRepository = StringComparer.OrdinalIgnoreCase.Equals("reactiveui/spalt", TFBuild.Environment.Repository.RepoName);

var msBuildPath = VSWhereLatest().CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");

var informationalVersion = EnvironmentVariable("GitAssemblyInformationalVersion");

// Artifacts
var artifactDirectory = "./artifacts/";
var packageWhitelist = new[] { "splat" };

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

    CreateDirectory(artifactDirectory);
});

Teardown(context =>
{
    // Executed AFTER the last task.
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Build")
    .Does (() =>
{
    Action<string,string> build = (solution, name) =>
    {
        Information("Building {0} using {1}", solution, msBuildPath);

        MSBuild(solution, new MSBuildSettings() {
                ToolPath = msBuildPath,
                ArgumentCustomization = args => args.Append("/m /restore")
            }
            .WithTarget("build;pack") 
            .WithProperty("PackageOutputPath",  MakeAbsolute(Directory(artifactDirectory)).ToString().Quote())
            .WithProperty("TreatWarningsAsErrors", treatWarningsAsErrors.ToString())
            .SetConfiguration("Release")                        
            .SetVerbosity(Verbosity.Minimal)
            .SetNodeReuse(false));
    };

    foreach(var package in packageWhitelist)
    {
        build("./src/" + package + "/" + package + ".csproj", package);
    }
});

Task("SignPackages")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    StartPowershellFile("./SignPackages.ps1", args =>
    {
    });
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
    .Does (() =>
{
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
