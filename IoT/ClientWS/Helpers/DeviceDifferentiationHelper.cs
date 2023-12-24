using ClientWS.Enums;

namespace ClientWS.Helpers;

public static class DeviceDifferentiationHelper
{
    public static DeviceType GetDeviceType(string payloadData)
    {
        return payloadData.Length switch
        {
            32 => DeviceType.EAGLE1500,
            42 => DeviceType.OXOButton,
            64 => DeviceType.ELSYS,
            _ => DeviceType.Unknown
        };
    }
    
}