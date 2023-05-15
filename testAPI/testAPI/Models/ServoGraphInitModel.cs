namespace testAPI.Models
{
    public class ServoGraphInitModel
    {
       public List<string> Labels { get; set; } = new List<string>();
       public List<ServoGraphSingleRecordModel> records { get; set; } = new List<ServoGraphSingleRecordModel>();
    }
}
