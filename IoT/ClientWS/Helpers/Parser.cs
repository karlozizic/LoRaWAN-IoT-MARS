using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using ClientWS.Enums;
using ClientWS.Exceptions;
using ClientWS.Models;

namespace ClientWS.Helpers;

public static class Parser
{
    public static OxoButtonData ParseData(WebSocketReceiveResult result, byte[] buffer)
    {
        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var deserializedData = JsonSerializer.Deserialize<PayloadModel>(message);

        var deviceEui = deserializedData?.DeviceEUI;
        var payloadData = deserializedData?.PayloadData;
        if (string.IsNullOrEmpty(deviceEui) || string.IsNullOrEmpty(payloadData) || deserializedData?.Command == "gw") //only rx messages are parsed
            throw new PayloadDataException("Invalid payload data");

        //differentiate which payload to parse based on EUI as there are more devices in LoRaWAN network
        var deviceType = DeviceDifferentiator.GetDeviceTypeBasedOnEUI(deviceEui);
        if (deviceType != DeviceType.OXOButton)
        {
            throw new InvalidDeviceException("Invalid payload data - Unknown device type");
        }
        
        var data = ParseOxoButton(payloadData);
        Console.WriteLine($"Parsed data: Button Clicked number({data.ButtonClickedNumber}) HeartbeatTimeoutOccured({data.HeartbeatTimeoutOccured}) AccelerometerInterruptEvent({data.AccelerometerInterruptEvent}) ImageCodeH({data.ImageCodeH}) ImageCodeL({data.ImageCodeL}) BatteryLevel({data.BatteryLevel}) Temperature({data.Temperature}) AccelerometerValueX({data.AccelerometerValueX}) AccelerometerValueY({data.AccelerometerValueY}) AccelerometerValueZ({data.AccelerometerValueZ})");
        return data;
    }

    private static OxoButtonData ParseOxoButton(string data)
    {
        if (data.Length < 26)
        {
            throw new PayloadDataException("Invalid payload data format - Eagle1500");
        }
        
        var parsedData = new OxoButtonData
        {
            ButtonClickedNumber = Convert.ToInt32(data.Substring(0, 2), 16),
            HeartbeatTimeoutOccured = Convert.ToInt32(data.Substring(2, 2), 16) == 1,
            AccelerometerInterruptEvent = Convert.ToInt32(data.Substring(4, 2), 16),
            ImageCodeH = Convert.ToByte(data.Substring(6, 2), 16),
            ImageCodeL = Convert.ToByte(data.Substring(8, 2), 16),
            BatteryLevel = Convert.ToInt32(data.Substring(10, 2), 16),
            Temperature = Convert.ToInt32(data.Substring(12, 2), 16),
            AccelerometerValueX = Convert.ToInt32(data.Substring(14, 4), 16),
            AccelerometerValueY = Convert.ToInt32(data.Substring(18, 4), 16),
            AccelerometerValueZ = Convert.ToInt32(data.Substring(22, 4), 16)
        };
        
        return parsedData;
    }

}