namespace ClientWS.Models;

public class ELSYSERSEye_Data
{
    // Properties specific to each sensor type
    public double Temperature { get; set; } // -3276.5 °C → 3276.5 °C (Value of: 100 → 10.0 °C) Resolution: 0.1
    public double Humidity { get; set; } // 0 – 100 % Resolution: 0.1
    public int Light { get; set; } // 0 – 65535 Lux
    public int Motion { get; set; } // 0 – 255 (Number of motion counts)
    public int BatteryVoltage { get; set; } // 0 – 65535 mV
    public int Occupancy { get; set; } // 0 = Unoccupied / 1 = Pending(Entering or leaving) / 2 = Occupied
    public List<double> GridEyeOccupancy { get; set; } // 65 bytes -> 1 byte ref. 64 byte pixel temp 8x8 (reserved for future use)
    public uint DebugInformation { get; set; } // 4 bytes 
    public byte[] SensorSettings { get; set; } // data size n -> sent to server at startup 
}