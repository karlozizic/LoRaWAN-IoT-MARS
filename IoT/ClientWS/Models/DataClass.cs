namespace ClientWS.Models;

public class DataClass
{
    public string cmd { get; set; }
    public int seqno { get; set; }
    public string EUI { get; set; }
    public long ts { get; set; }
    public int fcnt { get; set; }
    public int port { get; set; }
    public long freq { get; set; }
    public int rssi { get; set; }
    public int? snr { get; set; }
    public int toa { get; set; }
    public string dr { get; set; }
    public bool ack { get; set; }
    public int bat { get; set; }
    public bool? offline { get; set; }
    public string data { get; set; }
    public GatewayModel? gws { get; set; }
}

public class GatewayModel
{
    public List<GatewayInfo> gws { get; set; }
}

public class GatewayInfo
{
    public int rssi { get; set; }
    public int snr { get; set; }
    public long ts { get; set; }
    public DateTime time { get; set; }
    public string gweui { get; set; }
    public int ant { get; set; }
    public double lat { get; set; }
    public double lon { get; set; }
}