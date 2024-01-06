namespace ClientWS.Models;

public class MARSPayload
{
    public List<DataItem> Data { get; set; }
}

public class DataItem
{
    public int CounterNodeId { get; set; }
    public List<Value> Values { get; set; }
}

public class Value
{
    public DateTime Timestamp { get; set; }
    public double Val { get; set; }
}



