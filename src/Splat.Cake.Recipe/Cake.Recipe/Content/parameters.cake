public static class BuildParameters
{
    public static string Target { get; private set; }
    public static string Configuration { get; private set; }
    public static Cake.Core.Configuration.ICakeConfiguration CakeConfiguration { get; private set; }
    public static bool IsLocalBuild { get; private set; }
    public static bool IsRunningOnUnix { get; private set; }
    public static bool IsRunningOnWindows { get; private set; }
    public static bool IsPullRequest { get; private set; }

    public static bool TreatWarningsAsErrors { get; set; }

    public static CodecovCredentials Codecov { get; private set; }

    public static string Title { get; private set; }

    public static ICollection<string> WhitelistPackages { get; private set; }
    public static ICollection<string> WhitelistTestPackages { get; private set; }

    public static DirectoryPath SourceDirectory { get; private set; }

    public static DirectoryPath ArtifactsDirectory { get; private set; }
    public static DirectoryPath TestDirectoryPath { get; private set; }
    public static DirectoryPath PackagesDirectoryPath { get; private set; }
    public static DirectoryPath BinariesDirectoryPath { get; private set; }

    public static FilePath IntegrationTestScriptPath { get; private set; }

    public static ICollection<DirectoryPath> ToClean { get; private set; }

    public static ICollection<string> TestFrameworks { get; private set; }

    public static FilePath CodeCoverageOutputFile { get; private set; }

    public static bool ShouldRunCodecov { get; private set; }
    public static bool ShouldRunIntegrationTests { get; private set; }
    
    public static bool IsBuildServer { get; private set; }

    public static BuildTasks Tasks { get; set; }

    public static string CakeVersion { get; set; }

    public static bool CanPublishToCodecov
    {
        get
        {
            return ShouldRunCodecov &&
                (!string.IsNullOrEmpty(BuildParameters.Codecov.RepoToken) ||
                BuildParameters.IsRunningOnAppVeyor);
        }
    }

    static BuildParameters()
    {
        Tasks = new BuildTasks();
    }

    public static void SetParameters(
        ICakeContext context,
        BuildSystem buildSystem,
        string title,
        DirectoryPath artifactsDirectory,
        DirectoryPath sourceDirectory,
        DirectoryPath testDirectoryPath = null,
        DirectoryPath packagesDirectoryPath = null,
        DirectoryPath binariesDirectoryPath = null,
        string integrationTestScriptPath = null,
        ICollection<string> whitelistPackages = null,
        ICollection<string> whitelistTestPackages = null,
        ICollection<string> testFrameworks = null,
        FilePath codeCoverageOutputFile = null,
        bool shouldRunIntegrationTests = false,
        bool shouldRunCodecov = false)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (artifactsDirectory == null)
        {
            throw new ArgumentNullException(nameof(artifactsDirectory));
        }

        if (sourceDirectory == null)
        {
            throw new ArgumentNullException(nameof(sourceDirectory));
        }

        Title = title;
        WhitelistPackages = whitelistPackages;
        WhitelistTestPackages = whitelistTestPackages;
        TestFrameworks = testFrameworks;

        SourceDirectory = context.MakeAbsolute(sourceDirectory);
        ArtifactsDirectory = context.MakeAbsolute(artifactsDirectory);
        TestDirectoryPath = testDirectoryPath ?? (ArtifactsDirectory.Combine("tests"));
        PackagesDirectoryPath = packagesDirectoryPath ?? (ArtifactsDirectory.Combine("packages"));
        BinariesDirectoryPath = binariesDirectoryPath ?? (ArtifactsDirectory.Combine("binaries"));
        ToClean = new List() { ArtifactsDirectory, TestDirectoryPath, PackagesDirectoryPath, BinariesDirectoryPath };

        CodeCoverageOutputFile = codeCoverageOutputFile ?? TestDirectoryPath.Combine("codecoverage.xml");

        IntegrationTestScriptPath = integrationTestScriptPath;

        Target = context.Argument("target", "Default");
        Configuration = context.Argument("configuration", "Release");
        CakeConfiguration = context.GetConfiguration();

        IsBuildServer = buildSystem.TFBuild.IsRunningOnTFS;
        IsLocalBuild = buildSystem.IsLocalBuild;
        IsRunningOnUnix = context.IsRunningOnUnix();
        IsRunningOnWindows = context.IsRunningOnWindows();
        IsPullRequest = BuildProvider.PullRequest.IsPullRequest;

        CakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

        ShouldRunCodecov = shouldRunCodecov;
        ShouldRunIntegrationTests = (((!IsLocalBuild && !IsPullRequest) &&                                        
                                        context.FileExists(context.MakeAbsolute(BuildParameters.IntegrationTestScriptPath))) ||
                                        shouldRunIntegrationTests);
    }

    public static void PrintParameters(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        context.Information("Printing Build Parameters...");
        context.Information("Title: {0}", Title);
        context.Information("IsLocalBuild: {0}", IsLocalBuild);
        context.Information("IsPullRequest: {0}", IsPullRequest);
        context.Information("IsTagged: {0}", IsTagged);
        context.Information("TreatWarningsAsErrors: {0}", TreatWarningsAsErrors);
        context.Information("IsRunningOnUnix: {0}", IsRunningOnUnix);
        context.Information("IsRunningOnWindows: {0}", IsRunningOnWindows);
    }
}
