namespace ClientWS.Models;

public class OxoButtonData
{
    //Button clicked number (0...9)
    public int ButtonClickedNumber { get; set; }
    //0 or 1 
    public bool HeartbeatTimeoutOccured { get; set; }
    //0...6
    public int AccelerometerInterruptEvent { get; set; }
    //Image code H byte
    public byte ImageCodeH { get; set; }
    //Image code L byte
    public byte ImageCodeL { get; set; }
    //Battery level (0...100) %
    public int BatteryLevel { get; set; }
    //signed temperature in celsius
    public int Temperature { get; set; }
    //signed 28 byte values 
    public int AccelerometerValueX { get; set; }
    public int AccelerometerValueY { get; set; }
    public int AccelerometerValueZ { get; set; }

}