public class CodecovCredentials
{
    public string RepoToken { get; private set; }

    public CodecovCredentials(string repoToken)
    {
        RepoToken = repoToken;
    }
}
