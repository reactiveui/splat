///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var publishingError = false;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    Information(Figlet(BuildParameters.Title));

    Information("Starting Setup...");
    
    Information($"Build {BuildParameters.Title} using version {BuildParameters.CakeVersion} of Cake.");
});

Teardown(context =>
{
    Information("Starting Teardown...");

    // Clear nupkg files from tools directory
    if(DirectoryExists(Context.Environment.WorkingDirectory.Combine("tools")))
    {
        Information("Deleting nupkg files...");
        var nupkgFiles = GetFiles(Context.Environment.WorkingDirectory.Combine("tools") + "/**/*.nupkg");
        DeleteFiles(nupkgFiles);
    }

    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

BuildParameters.Tasks.ShowInfoTask = Task("Show-Info")
    .Does(() =>
{
    Information("Target: {0}", BuildParameters.Target);
    Information("Configuration: {0}", BuildParameters.Configuration);
    Information("IsLocalBuild: {0}", BuildParameters.IsLocalBuild);
    Information("IsPullRequest: {0}", BuildParameters.IsPullRequest);

    Information("Artifacts DirectoryPath: {0}", MakeAbsolute(BuildParameters.ArtifactsDirectory));
    Information("TestDirectoryPath DirectoryPath: {0}", MakeAbsolute(BuildParameters.TestDirectoryPath));
    Information("PackagesDirectoryPath DirectoryPath: {0}", MakeAbsolute(BuildParameters.PackagesDirectoryPath));
    Information("BinariesDirectoryPath DirectoryPath: {0}", MakeAbsolute(BuildParameters.BinariesDirectoryPath));
});

BuildParameters.Tasks.CleanTask = Task("Clean")
    .IsDependentOn("Show-Info")
    .Does(() =>
{
    Information("Cleaning...");

    CleanDirectories(BuildParameters.ToClean);
});


BuildParameters.Tasks.BuildTask = Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() => {

        foreach (var project in BuildParameters.WhitelistPackages)
        {
            BuildProject(project, false);             
        }

        CopyBuildOutput();
    });

public void CopyBuildOutput()
{
    Information("Copying build output...");

    foreach (var project in BuildParameters.WhitelistPackages)
    {
        // There is quite a bit of duplication in this function, that really needs to be tidied Upload

        Information("Input BuildPlatformTarget: {0}", ToolSettings.BuildPlatformTarget.ToString());
        var platformTarget = ToolSettings.BuildPlatformTarget == PlatformTarget.MSIL ? "AnyCPU" : ToolSettings.BuildPlatformTarget.ToString();
        Information("Using BuildPlatformTarget: {0}", platformTarget);

        var parsedProject = ParseProject(project.Path, BuildParameters.Configuration, platformTarget);

        if(project.Path.FullPath.ToLower().Contains("wixproj"))
        {
            Warning("Skipping wix project");
            continue;
        }

        if(project.Path.FullPath.ToLower().Contains("shproj"))
        {
            Warning("Skipping shared project");
            continue;
        }

        if(parsedProject.OutputPath == null || parsedProject.RootNameSpace == null || parsedProject.OutputType == null)
        {
            Information("OutputPath: {0}", parsedProject.OutputPath);
            Information("RootNameSpace: {0}", parsedProject.RootNameSpace);
            Information("OutputType: {0}", parsedProject.OutputType);
            throw new Exception(string.Format("Unable to parse project file correctly: {0}", project.Path));
        }

        // If the project is an exe, then simply copy all of the contents to the correct output folder
        if(!parsedProject.IsLibrary())
        {
            Information("Project has an output type of exe: {0}", parsedProject.RootNameSpace);
            var outputFolder = BuildParameters.Paths.Directories.PublishedApplications.Combine(parsedProject.RootNameSpace);
            EnsureDirectoryExists(outputFolder);

            // If .NET SDK project, copy using dotnet publish for each target framework
            // Otherwise just copy
            if(parsedProject.IsVS2017ProjectFormat)
            {
                var msBuildSettings = new DotNetCoreMSBuildSettings()
                            .WithProperty("Version", BuildParameters.Version.SemVersion)
                            .WithProperty("AssemblyVersion", BuildParameters.Version.Version)
                            .WithProperty("FileVersion",  BuildParameters.Version.Version)
                            .WithProperty("AssemblyInformationalVersion", BuildParameters.Version.InformationalVersion);

                if(!IsRunningOnWindows())
                {
                    var frameworkPathOverride = new FilePath(typeof(object).Assembly.Location).GetDirectory().FullPath + "/";

                    // Use FrameworkPathOverride when not running on Windows.
                    Information("Publish will use FrameworkPathOverride={0} since not building on Windows.", frameworkPathOverride);
                    msBuildSettings.WithProperty("FrameworkPathOverride", frameworkPathOverride);
                }

                foreach(var targetFramework in parsedProject.NetCore.TargetFrameworks)
                {
                    DotNetCorePublish(project.Path.FullPath, new DotNetCorePublishSettings {
                        OutputDirectory = outputFolder.Combine(targetFramework),
                        Framework = targetFramework,
                        Configuration = BuildParameters.Configuration,
                        MSBuildSettings = msBuildSettings
                    });
                }
            }
            else
            {
                CopyFiles(GetFiles(parsedProject.OutputPath.FullPath + "/**/*"), outputFolder, true);
            }

            continue;
        }

        // Now we need to test for whether this is a unit test project.
        // If this is found, move the output to the unit test folder, otherwise, simply copy to normal output folder
        if(!BuildParameters.IsDotNetCoreBuild)
        {
            Information("Not a .Net Core Build");
        }
        else
        {
            Information("Is a .Net Core Build");
        }

        if(parsedProject.IsLibrary() && parsedProject.IsXUnitTestProject())
        {
            Information("Project has an output type of library and is an xUnit Test Project: {0}", parsedProject.RootNameSpace);
            var outputFolder = BuildParameters.Paths.Directories.PublishedxUnitTests.Combine(parsedProject.RootNameSpace);
            EnsureDirectoryExists(outputFolder);
            CopyFiles(GetFiles(parsedProject.OutputPath.FullPath + "/**/*"), outputFolder, true);
            continue;
        }
        else
        {
            Information("Project has an output type of library: {0}", parsedProject.RootNameSpace);

            // If .NET SDK project, copy for each output path
            // Otherwise just copy
            if(parsedProject.IsVS2017ProjectFormat)
            {
                foreach(var outputPath in parsedProject.OutputPaths)
                {
                    var outputFolder = BuildParameters.Paths.Directories.PublishedLibraries.Combine(parsedProject.RootNameSpace).Combine(outputPath.GetDirectoryName());
                    EnsureDirectoryExists(outputFolder);
                    Information(outputPath);
                    CopyFiles(GetFiles(outputPath + "/**/*"), outputFolder, true);
                }
            }
            else
            {
                var outputFolder = BuildParameters.Paths.Directories.PublishedLibraries.Combine(parsedProject.RootNameSpace);
                EnsureDirectoryExists(outputFolder);
                Information(parsedProject.OutputPath.FullPath);
                CopyFiles(GetFiles(parsedProject.OutputPath.FullPath + "/**/*"), outputFolder, true);
            }
            
            continue;
        }
    }
}

BuildParameters.Tasks.PackageTask = Task("Test")
    .IsDependentOn("Test-xUnit");

BuildParameters.Tasks.PackageTask = Task("Package");

BuildParameters.Tasks.DefaultTask = Task("Default")
    .IsDependentOn("Package");

BuildParameters.Tasks.UploadCoverageReportTask = Task("Upload-Coverage-Report")
  .IsDependentOn("Upload-Codecov-Report");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

public Builder Build
{
    get
    {
        return new Builder(target => RunTarget(target));
    }
}

public class Builder
{
    private Action<string> _action;

    public Builder(Action<string> action)
    {
        _action = action;
    }

    public void Run()
    {
        BuildParameters.IsDotNetCoreBuild = false;
        BuildParameters.IsNuGetBuild = false;

        SetupTasks();

        _action(BuildParameters.Target);
    }

    public void RunNuGet()
    {
        BuildParameters.Tasks.PackageTask.IsDependentOn("Create-NuGet-Package");
        BuildParameters.IsDotNetCoreBuild = false;
        BuildParameters.IsNuGetBuild = true;

        _action(BuildParameters.Target);
    }

    private static void SetupTasks()
    {
        BuildParameters.Tasks.CreateNuGetPackagesTask.IsDependentOn("Build");
        BuildParameters.Tasks.CreateChocolateyPackagesTask.IsDependentOn("Build");
        BuildParameters.Tasks.TestTask.IsDependentOn("Build");
        BuildParameters.Tasks.DupFinderTask.IsDependentOn("Build");
        BuildParameters.Tasks.InspectCodeTask.IsDependentOn("Build");
        BuildParameters.Tasks.PackageTask.IsDependentOn("Analyze");
        BuildParameters.Tasks.PackageTask.IsDependentOn("Test");
        BuildParameters.Tasks.UploadCodecovReportTask.IsDependentOn("Test");
        BuildParameters.Tasks.UploadCoverallsReportTask.IsDependentOn("Test");
        BuildParameters.Tasks.InstallReportGeneratorTask.IsDependentOn("Build");

        BuildParameters.Tasks.TestTask.IsDependentOn("Test");
    }
}
