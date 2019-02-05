public static class Environment
{
    public static string CodecovRepoTokenVariable { get; private set; }
    public static string SignClientSecretVariable { get; private set; }

    public static void SetVariableNames(
        string codecovRepoTokenVariable = null,
        string signClientSecretVariable = null)
    {
        CodecovRepoTokenVariable = codecovRepoTokenVariable ?? "CODECOV_TOKEN";
        SignClientSecretVariable = signClientSecretVariable ?? "SIGNCLIENT_SECRET";
    }
}