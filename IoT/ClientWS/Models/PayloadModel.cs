using System.Text.Json.Serialization;

namespace ClientWS.Models;

public class PayloadModel
{
    [JsonPropertyName("cmd")]
    public string Command { get; set; }

    [JsonPropertyName("seqno")]
    public int SequenceNumber { get; set; }

    [JsonPropertyName("EUI")]
    public string DeviceEUI { get; set; }

    [JsonPropertyName("ts")]
    public long Timestamp { get; set; }

    [JsonPropertyName("fcnt")]
    public int FrameCounter { get; set; }

    [JsonPropertyName("port")]
    public int PortNumber { get; set; }

    [JsonPropertyName("freq")]
    public long Frequency { get; set; }

    [JsonPropertyName("rssi")]
    public int RSSI { get; set; }

    [JsonPropertyName("snr")]
    public double SignalToNoiseRatio { get; set; }

    [JsonPropertyName("toa")]
    public int TimeOnAir { get; set; }

    [JsonPropertyName("dr")]
    public string DataRate { get; set; }

    [JsonPropertyName("ack")]
    public bool Acknowledgment { get; set; }

    [JsonPropertyName("bat")]
    public int Battery { get; set; }

    [JsonPropertyName("offline")]
    public bool IsOffline { get; set; }

    [JsonPropertyName("data")]
    public string PayloadData { get; set; }
    
    [JsonPropertyName("gws")]
    public List<GatewayInfo>? Gateways { get; set; }
}