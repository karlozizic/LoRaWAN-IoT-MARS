using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using ClientWS.Enums;
using ClientWS.Models;

namespace ClientWS.Helpers;

//TODO: finish Parser 
public static class Parser
{
    public static string ParseData(WebSocketReceiveResult result, byte[] buffer)
    {
        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var deserializedData = JsonSerializer.Deserialize<PayloadModel>(message);
        
        var payloadData = deserializedData?.PayloadData;
        if (string.IsNullOrEmpty(payloadData))
            throw new Exception("No payload data");
        
        var deviceType = DeviceDifferentiationHelper.GetDeviceType(payloadData);
        if (deviceType == DeviceType.Unknown)
            throw new Exception("Payload data is not in correct format");

        if (deviceType == DeviceType.EAGLE1500)
            ParseEAGLE1500Data(payloadData);
        else if (deviceType == DeviceType.OXOButton)
            ParseEAGLE1500Data(payloadData);
        else if (deviceType == DeviceType.ELSYS) 
            ParseELSYSData(payloadData);
        
        return message; 
    }


    public static string ParseEAGLE1500Data(string data)
    {
        return "";
    }

    public static string ParseOXOButtonData(string data)
    {
        return "";
    }

    public static string ParseELSYSData(string data)
    {
        return "";
    }
}