namespace ClientWS.Models;

public class MARSPayload
{
    public List<DataItem> data { get; set; }
}

public class DataItem
{
    public int counterNodeId { get; set; }
    public List<Value> values { get; set; }
}

public class Value
{
    public string timestamp { get; set; }
    public double value { get; set; }
}



