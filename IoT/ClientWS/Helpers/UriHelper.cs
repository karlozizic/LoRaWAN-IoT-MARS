namespace ClientWS.Helpers;

public static class UriHelper
{
    public static Uri? InitializeUri(string envVar, string? customUri = null)
    {
        if (Uri.TryCreate(customUri, UriKind.Absolute, out var uri))
        {
            return new Uri(customUri);
        }

        return new Uri(EnvironmentalVariableHelper.FetchEnvVar(envVar)); 
    }
}