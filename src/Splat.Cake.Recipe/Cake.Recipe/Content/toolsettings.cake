public static class ToolSettings
{
    public static ICollection<string> TestCoverageFilter { get; private set; }
    public static ICollection<string> TestCoverageExcludeFilter { get; private set; }
    public static ICollection<string> TestCoverageExcludeByAttribute { get; private set; }
    public static ICollection<string> TestCoverageExcludeByFile { get; private set; }
    public static string TestCoverageOutputFormat { get; private set; }
    public static PlatformTarget BuildPlatformTarget { get; private set; }
    public static MSBuildToolVersion BuildMSBuildToolVersion { get; private set; }
    public static int MaxCpuCount { get; private set; }
    public static DirectoryPath OutputDirectory { get; private set; }

    public static void SetToolSettings(
        ICakeContext context,
        ICollection<string> testCoverageFilter = null,
        ICollection<string> testCoverageExcludeFilter = null,
        ICollection<string> testCoverageExcludeByAttribute = null,
        ICollection<string> testCoverageExcludeByFile = null,
        string testCoverageOutputFormat = null,
        PlatformTarget? buildPlatformTarget = null,
        int? maxCpuCount = null,
        DirectoryPath outputDirectory = null)
    {
        context.Information("Setting up tools...");

        TestCoverageFilter = testCoverageFilter ?? new [] { string.Format("[{0}*]*", BuildParameters.Title) };
        TestCoverageExcludeFilter = testCoverageExcludeFilter ?? new [] { string.Format("[{0}*Tests*]*", BuildParameters.Title) };
        TestCoverageExcludeByAttribute = testCoverageExcludeByAttribute ?? new[] { "*.ExcludeFromCodeCoverage*" };
        TestCoverageExcludeByFile = testCoverageExcludeByFile ?? new[] { "*/*Designer.cs", "*/*.g.cs", "*/*.g.i.cs" };
        TestCoverageOutputFormat = testCoverageOutputFormat ?? "cobertura";
        BuildPlatformTarget = buildPlatformTarget ?? PlatformTarget.MSIL;
        BuildMSBuildToolVersion = buildMSBuildToolVersion;
        MaxCpuCount = maxCpuCount ?? 1;
        OutputDirectory = outputDirectory;
    }
}