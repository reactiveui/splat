
Action<string, bool> BuildProject = (project, doNotOptimise) =>
{
    Information("Building {0}", System.IO.Path.GetFileNameWithoutExtension(project));
    var msBuildSettings = new MSBuildSettings() {
            ArgumentCustomization = args => args.Append("/NoWarn:VSX1000"),
            Restore = true
        }
        .SetPlatformTarget(ToolSettings.BuildPlatformTarget)
        .UseToolVersion(ToolSettings.BuildMSBuildToolVersion)
        .WithProperty("TreatWarningsAsErrors", BuildParameters.TreatWarningsAsErrors.ToString())
        .SetMaxCpuCount(ToolSettings.MaxCpuCount)
        .SetConfiguration(BuildParameters.Configuration)
        .WithTarget("build")                   
        .SetVerbosity(Verbosity.Minimal);

    if (doNotOptimise)
    {
        msBuildSettings = msBuildSettings.WithProperty("Optimize",  "False");
    }

    MSBuild(project, msBuildSettings);   
};