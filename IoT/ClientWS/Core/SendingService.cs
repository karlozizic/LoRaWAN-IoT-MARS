using System.Text.Json;
using ClientWS.Exceptions;
using ClientWS.Helpers;
using ClientWS.Models;

namespace ClientWS.Core;

public class SendingService
{
    private static Uri? _marsUri;
    private static string? _username;
    private static string? _password;
    private static string? _grantType;
    private static readonly HttpClient _httpClient = new();
    private static string? _accessToken;
    
    public SendingService()
    {
        _marsUri = UriHelper.InitializeUri("MARS_URI", null);
        _username = EnvironmentalVariableHelper.FetchEnvVar("USERNAME");
        _password = EnvironmentalVariableHelper.FetchEnvVar("PASSWORD");
        _grantType = EnvironmentalVariableHelper.FetchEnvVar("GRANT_TYPE");
    }

    public async Task SetAccessToken()
    {
        Dictionary<string, string> formData = new()
        {
            {"username", _username},
            {"password", _password},
            {"grant_type", _grantType}
        };
        
        var content = new FormUrlEncodedContent(formData);
        
        var response = await _httpClient.PostAsync(_marsUri, content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
            _accessToken = tokenResponse?.access_token;
        }
        else
        {
            throw new AccessTokenException("Could not get access token");
        }
    }
    
    public async Task SendData(ELSYSERSEye_Data data)
    {
        Console.WriteLine("testing out");
    }
    
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}