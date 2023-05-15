namespace testAPI.Models
{
    public class ServoPositionModel
    {
        public ServoPositionModel(string label,  List<int> data)
        {
            this.label = label;
            this.data = data;
        }
        public ServoPositionModel()
        {
        }

        public string label { get; set; }
        public List<int> data { get; set; }

    }
}
