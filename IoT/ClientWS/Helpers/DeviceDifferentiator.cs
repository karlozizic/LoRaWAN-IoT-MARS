using ClientWS.Enums;

namespace ClientWS.Helpers;

public static class DeviceDifferentiator
{
    public static DeviceType GetDeviceTypeBasedOnPayloadLength(int payloadLength)
    {
        return payloadLength switch
        {
            26 => DeviceType.OXOButton,
            46 => DeviceType.EAGLE1500,
            158 => DeviceType.ELSYS,
            _ => DeviceType.Unknown
        };
    }
    
    public static DeviceType GetDeviceTypeBasedOnEUI(string eui)
    {
        return eui switch
        {
            "A81758FFFE04D146" => DeviceType.OXOButton,
            "A81758FFFE04D4AE" => DeviceType.EAGLE1500,
            "A81758FFFE04A758" => DeviceType.ELSYS,
            _ => DeviceType.Unknown
        };
    }
    
}