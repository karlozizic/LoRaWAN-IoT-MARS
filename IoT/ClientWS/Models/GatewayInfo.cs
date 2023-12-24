using System.Text.Json.Serialization;

namespace ClientWS.Models;

public class GatewayInfo
{
    [JsonPropertyName("rssi")]
    public int Rssi { get; set; }

    [JsonPropertyName("snr")]
    public double Snr { get; set; }

    [JsonPropertyName("ts")]
    public long Timestamp { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("gweui")]
    public string Gweui { get; set; }

    [JsonPropertyName("ant")]
    public int Antenna { get; set; }

    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
}