namespace testAPI.Models
{
    public class SystemInfoDataModel
    {
        public double TotalMemory { get; set; }
        public string ServiceStatus { get; set; }
        public double UsedMemory { get; set; }
        public double DiskFree { get; set; }
        public double TotalDisk { get; set; }
        public int CpuCores { get; set; }
        public bool ObjectDetection { get; set; }
        public bool TextToSpeech { get; set; }

        public string RuntimeMinutes {get; set;}
        public double CpuUsage { get; set; }
    }
}
