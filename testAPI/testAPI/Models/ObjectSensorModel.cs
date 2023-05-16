namespace testAPI.Models
{
    public class ObjectSensorModel
    {
        public string CurrentObject { get; set; }
        public List <GS1Model> QRValue { get; set; }
        public int Weight { get; set; }
        public int Temperature { get; set; }
        public string category { get; set; }
        public int frequentie { get; set; }
     }
}
