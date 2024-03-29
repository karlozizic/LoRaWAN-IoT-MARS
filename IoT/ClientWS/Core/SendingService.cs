﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ClientWS.Exceptions;
using ClientWS.Helpers;
using ClientWS.Models;

namespace ClientWS.Core;

public class SendingService
{
    private static Uri? _marsUri;
    private static string _username;
    private static string _password;
    private static string _grantType;
    private static readonly HttpClient _httpClient = new();
    
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
        
        var response = await _httpClient.PostAsync(_marsUri + "Token", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse?.access_token);
        }
        else
        {
            throw new AccessTokenException("Could not get access token");
        }
    }
    
    public async Task SendDataToOneNode(Value value, int nodeId)
    {
        var requestUri = _marsUri + "api/public/postRawDataInput";  
        var body = new MARSPayload
        {
            data = new List<DataItem>
            {
                new DataItem
                {
                    counterNodeId = nodeId,
                    values = new List<Value>
                    {
                        value
                    }
                }
            }
        };

        var jsonBody = JsonSerializer.Serialize(body); 
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpClient.PostAsync(requestUri, content); 
            Console.WriteLine($"MARS Response:{response}");
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseBody}");
        }
        catch(Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }
    }
    
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}