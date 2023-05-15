namespace testAPI.Models
{
    public class ServoGraphSingleRecordModel
    {
        public ServoGraphSingleRecordModel(string label, string backgroundcolor, List<List<int>> data)
        {
            this.label = label;
            this.backgroundcolor = backgroundcolor;
            this.data = data;
        }
        public ServoGraphSingleRecordModel()
        {
        }

        public string label { get; set; }
        public string backgroundcolor { get; set; } = "#000000";
        public List<List<int>> data { get; set; }

    }
}
