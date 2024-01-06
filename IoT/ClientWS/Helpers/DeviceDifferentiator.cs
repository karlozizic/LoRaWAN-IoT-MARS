using ClientWS.Enums;

namespace ClientWS.Helpers;

public static class DeviceDifferentiator
{
    
    public static DeviceType GetDeviceTypeBasedOnEUI(string eui)
    {
        return eui switch
        {
            "A81758FFFE04D146" => DeviceType.ELSYS_ERS_EYE, 
            _ => DeviceType.Unknown
        };
    }
    
}