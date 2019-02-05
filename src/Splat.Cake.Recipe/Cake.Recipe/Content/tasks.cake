public class BuildTasks
{
    public CakeTaskBuilder InspectCodeTask { get; set; }
    public CakeTaskBuilder AnalyzeTask { get; set; }
    public CakeTaskBuilder ShowInfoTask { get; set; }
    public CakeTaskBuilder CleanTask { get; set; }
    public CakeTaskBuilder DotNetCoreCleanTask { get; set; }
    public CakeTaskBuilder RestoreTask { get; set; }
    public CakeTaskBuilder DotNetCoreRestoreTask { get; set; }
    public CakeTaskBuilder BuildTask { get; set; }
    public CakeTaskBuilder DotNetCoreBuildTask { get; set; }
    public CakeTaskBuilder PackageTask { get; set; }
    public CakeTaskBuilder DefaultTask { get; set; }
    public CakeTaskBuilder ClearCacheTask { get; set; }
    public CakeTaskBuilder UploadCodecovReportTask { get; set; }
    public CakeTaskBuilder UploadCoverageReportTask { get; set; }
    public CakeTaskBuilder DotNetCorePackTask { get; set; }
    public CakeTaskBuilder CreateNuGetPackageTask { get; set; }
    public CakeTaskBuilder CreateNuGetPackagesTask { get; set; }
    public CakeTaskBuilder InstallReportGeneratorTask { get; set; }
    public CakeTaskBuilder InstallReportUnitTask { get; set; }
    public CakeTaskBuilder InstallOpenCoverTask { get; set; }
    public CakeTaskBuilder TestxUnitTask { get; set; }
    public CakeTaskBuilder IntegrationTestTask { get;set; }
    public CakeTaskBuilder TestTask { get; set; }
    public CakeTaskBuilder CleanDocumentationTask { get; set; }
}
