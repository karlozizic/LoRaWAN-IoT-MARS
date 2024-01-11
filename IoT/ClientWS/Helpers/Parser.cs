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
    private const byte TYPE_TEMP = 0x01;
    private const byte TYPE_HUMIDITY = 0x02;
    private const byte TYPE_LIGHT = 0x04;
    private const byte TYPE_MOTION = 0x05;
    private const byte TYPE_VDD = 0x07;
    private const byte TYPE_OCCUPANCY = 0x11;
    private const byte TYPE_GRIDEYE = 0x13;
    private const byte TYPE_DEBUG = 0x3D;
    private const byte TYPE_SETTINGS = 0x3E;
    
    public static ELSYSERSEye_Data ParseData(WebSocketReceiveResult result, byte[] buffer)
    {
        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var deserializedData = JsonSerializer.Deserialize<PayloadModel>(message);

        var deviceEui = deserializedData?.DeviceEUI;
        var payloadData = deserializedData?.PayloadData;
        if (string.IsNullOrEmpty(deviceEui) || string.IsNullOrEmpty(payloadData) ||
            deserializedData?.Command == "gw") //only rx messages are parsed
            throw new PayloadDataException("gateway message or invalid payload data");

        //differentiate which payload to parse based on EUI as there are more devices in LoRaWAN network
        var deviceType = DeviceDifferentiator.GetDeviceTypeBasedOnEUI(deviceEui);
        if (deviceType != DeviceType.ELSYS_ERS_EYE)
        {
            throw new InvalidDeviceException("Invalid payload data - Unknown device type");
        }
        
        var data = DecodeSensorPayload(HexToBytes(payloadData));
        Console.WriteLine($"Parsed data: Temperature({data.Temperature}) Humidity({data.Humidity}) Light({data.Light}) Motion({data.Motion}) BatteryVoltage({data.BatteryVoltage}) Occupancy({data.Occupancy}) DebugInformation({data.DebugInformation}) SensorSettings({data.SensorSettings})");
        return data;
    }
    //converts hex string to byte array
    private static List<byte> HexToBytes(string hex)
    {
        var bytes = new List<byte>();
        for (var c = 0; c < hex.Length; c += 2)
            bytes.Add(Convert.ToByte(hex.Substring(c, 2), 16));
        return bytes;
    }
    //converts 16 bit binary number to decimal
    private static int Bin16Dec(int bin)
    {
        var num = bin & 0xFFFF;
        if ((0x8000 & num) != 0)
            num = -(0x010000 - num);
        return num;
    }

    private static ELSYSERSEye_Data DecodeSensorPayload(List<byte> data)
    {
        var obj = new ELSYSERSEye_Data(); 
        for (var i = 0; i < data.Count; i++)
        {
            switch (data[i])
            {
                case TYPE_TEMP:
                    // Big-endian - shifts the first byte 8 bits to the left and ORs it with the second byte
                    var temp = (data[i + 1] << 8) | (data[i + 2]);
                    temp = Bin16Dec(temp);
                    // Divide by 10 to get the actual temperature - defined in documentation
                    obj.Temperature = temp / 10.0; 
                    i += 2;
                    break;
                case TYPE_HUMIDITY:
                    // one byte 
                    var rh = data[i + 1];
                    obj.Humidity = rh;
                    i += 1;
                    break;
                case TYPE_LIGHT:
                    obj.Light = (data[i + 1] << 8) | (data[i + 2]); 
                    i += 2;
                    break;
                case TYPE_MOTION:
                    obj.Motion = data[i + 1];
                    i += 1;
                    break;
                case TYPE_VDD:
                    obj.BatteryVoltage = (data[i + 1] << 8) | (data[i + 2]);
                    i += 2;
                    break;
                case TYPE_OCCUPANCY:
                    obj.Occupancy = data[i + 1];
                    i += 1;
                    break;
                case TYPE_GRIDEYE:
                    // reference value
                    var refValue = data[i + 1];
                    i++;
                    var grideyeValues = new List<double>();
                    // loop iterates over 8x8 grid
                    for (var j = 0; j < 64; j++)
                    {
                        // / 10.0 for temperature 
                        grideyeValues.Add(refValue + (data[i + j] / 10.0));
                    }
                    obj.GridEyeOccupancy = grideyeValues;
                    i += 64;
                    break;
                //not interested in these values
                case TYPE_DEBUG:
                    i += 4;
                    break;
                case TYPE_SETTINGS:
                    //exit the loop
                    i = data.Count;
                    break;
                default:
                    i = data.Count;
                    break;
            }
        }
        return obj;
    }

}