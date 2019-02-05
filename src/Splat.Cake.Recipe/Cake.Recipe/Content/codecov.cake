///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

BuildParameters.Tasks.UploadCodecovReportTask = Task("Upload-Codecov-Report")
    .WithCriteria(() => FileExists(BuildParameters.CodeCoverageOutputFile))
    .WithCriteria(() => BuildParameters.CanPublishToCodecov)
    .Does(() => RequireTool(CodecovTool, () => {
        var testCoverageOutputFile = BuildParameters.CodeCoverageOutputFile;
        var token = BuildParameters.Codecov.RepoToken;

        Information("Upload {0} to Codecov server", testCoverageOutputFile);
        
        // Upload a coverage report.
        Codecov(testCoverageOutputFile.ToString(), token);
    })
).OnError (exception =>
{
    Error(exception.Message);
    Information("Upload-Codecov-Report Task failed, but continuing with next Task...");
    publishingError = true;
});
