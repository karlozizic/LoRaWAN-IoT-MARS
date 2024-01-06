using ClientWS.Exceptions;

namespace ClientWS.Helpers;

public static class EnvironmentalVariableHelper
{
    public static string FetchEnvVar(string envVar)
    {
        string envVarString = Environment.GetEnvironmentVariable(envVar);
        
        if (!string.IsNullOrEmpty(envVarString))
        {
            return envVarString;
        }
        
        throw new EnvironmentalVarException("Environmental variable not found");
    }
}