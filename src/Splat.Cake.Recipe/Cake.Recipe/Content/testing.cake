///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

BuildParameters.Tasks.InstallReportGeneratorTask = Task("Install-ReportGenerator")
    .Does(() => RequireTool(ReportGeneratorTool, () => {
    }));

BuildParameters.Tasks.InstallReportUnitTask = Task("Install-ReportUnit")
    .IsDependentOn("Install-ReportGenerator")
    .Does(() => RequireTool(ReportUnitTool, () => {
    }));

BuildParameters.Tasks.InstallOpenCoverTask = Task("Install-Coverlet")
    .Does(() => RequireTool(CoverletTool, () => {
    }));

BuildParameters.Tasks.TestxUnitTask = Task("Test-xUnit")
    .IsDependentOn("Install-Coverlet")
    .WithCriteria(() => DirectoryExists(BuildParameters.TestDirectoryPath))
    .Does(() => RequireTool(XUnitTool, () => {
        EnsureDirectoryExists(BuildParameters.TestDirectoryPath);

        foreach (var projectName in BuildParameters.WhitelistTestPackages)
        {
            var packageName = System.IO.Path.GetFileNameWithoutExtension(projectName);
            BuildProject(projectName, true);
                
            foreach (var testFramework in BuildParameters.TestFrameworks)
            {
                Information($"Performing coverage tests on {packageName} on framework {testFramework}");

                var testFile = $"{System.IO.Path.GetDirectoryName(projectName)}/bin/{BuildParameters.Configuration}/{testFramework}/{packageName}.dll";

                StartProcess(Context.Tools.Resolve("coverlet*").ToString(), new ProcessSettings {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Arguments = new ProcessArgumentBuilder()
                        .AppendQuoted(testFile)
                        .AppendSwitch("--include", $"[{BuildParameters.Title}*]*")
                        .AppendSwitch("--exclude", "[*.Tests*]*")
                        .AppendSwitch("--exclude", "[*]*Legacy*")
                        .AppendSwitch("--exclude", "[*]*ThisAssembly*")
                        .AppendSwitch("--exclude-by-file", "*ApprovalTests*")
                        .AppendSwitchQuoted("--output", BuildParameters.TestDirectoryPath.Combine($"testcoverage-{packageName}-{testFramework}.xml").ToString())
                        .AppendSwitch("--format", "cobertura")
                        .AppendSwitch("--target", "dotnet")
                        .AppendSwitchQuoted("--targetargs", $"test {projectName} --no-build -c {BuildParameters.Configuration} --logger:trx;LogFileName=testresults-{packageName}-{testFramework}.trx -r {MakeAbsolute(BuildParameters.TestDirectoryPath)}")
                    });

                Information($"Finished coverage testing {packageName}");
            }
        }
       
        // TODO: Need to think about how to bring this out in a generic way for all Test Frameworks
        // ReportUnit(BuildParameters.Paths.Directories.xUnitTestResults, BuildParameters.Paths.Directories.xUnitTestResults, new ReportUnitSettings());

        // TODO: Need to think about how to bring this out in a generic way for all Test Frameworks
        ReportGenerator(
            GetFiles($"{BuildParameters.TestDirectoryPath}**/testcoverage-*.xml"),
            $"{BuildParameters.TestDirectoryPath}**/report",
            new ReportGeneratorSettings 
            {
                ReportTypes = new[] { ReportGeneratorReportType.Cobertura, ReportGeneratorReportType.Html },
            });
    })
);

BuildParameters.Tasks.IntegrationTestTask = Task("Run-Integration-Tests")
    .WithCriteria(() => BuildParameters.ShouldRunIntegrationTests)
    .IsDependentOn("Default")
    .Does(() => 
    {
            CakeExecuteScript(BuildParameters.IntegrationTestScriptPath,
                new CakeSettings 
                {
                    Arguments = new Dictionary<string, string>
                    {
                        { "verbosity", Context.Log.Verbosity.ToString("F") }
                    }
                });
    });

BuildParameters.Tasks.TestTask = Task("Test");
