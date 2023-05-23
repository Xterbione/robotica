using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using testAPI.Models;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private string[] colors = new string[] {
            "#FF0000", // Red
            "#00FF00", // Green
            "#0000FF", // Blue
            "#FFFF00", // Yellow
            "#FF00FF", // Magenta
            "#00FFFF", // Cyan
            "#800000", // Maroon
            "#008000", // Olive
            "#000080", // Navy
            "#808000", // Olive Drab
            "#800080", // Purple
            "#008080", // Teal
            "#FFA500", // Orange
            "#FFC0CB", // Pink
            "#ADD8E6", // Light Blue
            "#F0E68C"  // Khaki
        };
        private Random random = new Random();
        public ApiController()
        {
            //not much here yet
        }
        [HttpGet("/getobjectsensor")]
        public JsonResult Getobjsens()
        {   var frequentie = random.Next(0, 4000);
            var category = "";
            switch (frequentie)
            {
                case > 1999:
                    category = "3 Highband[2000Hz+]";
                    break;
                case > 199:
                    category = "2 Midband[200 - 2000Hz]";
                    break;
                default:
                    category = "1 Lowband[0 - 200Hz]";
                    break;
            }
            var result = new ObjectSensorModel();
            result.Weight = random.Next(0,50);
            result.Temperature = random.Next(20, 30);
            result.QRValue = GS1.getJson(GS1.Ex1_1);//new List<string>() { "Something", "Another", "This", "That" };
            result.frequentie = frequentie;
            result.CurrentObject = "Oxicodon";
            result.category = category;
            return new JsonResult(Ok(result));
        }

        [HttpGet("/getservostats")]
        public JsonResult Getservos()
        {
            var result = new ServoDataBundleModel();
            for (int i = 0; i < 6; i++)
            {
                result.data.Add(random.Next(0, 100));
            }
            result.labels = new List<string> { "wheel fl", "wheel fr", "wheel rl", "wheel rr", "grip", "arm positioner" };
            return new JsonResult(Ok(result));
        }

        [HttpGet("/getservostatsgraph")]
        public JsonResult Getservosgraph()
        {
            var result = new ServoGraphInitModel();

            for (int i = 0; i < 19; i++)
            {
                DateTime fiveMinutesAgo = DateTime.Now.AddSeconds(-(19-i));
                result.Labels.Add(fiveMinutesAgo.ToString());
            }
            result.Labels.Add("current");
            var record = new ServoGraphSingleRecordModel("wheel fl", colors[0].ToString(),new List<List<int>>());
            result.records.Add(record);
            record = new ServoGraphSingleRecordModel("wheel fr", colors[1].ToString(), new List<List<int>>());
            result.records.Add(record);
            record = new ServoGraphSingleRecordModel("wheel rl", colors[2].ToString(), new List<List<int>>());
            result.records.Add(record);
            record = new ServoGraphSingleRecordModel("wheel rr", colors[3].ToString(), new List<List<int>>());
            result.records.Add(record);
            record = new ServoGraphSingleRecordModel("grip", colors[4].ToString(), new List<List<int>>());
            result.records.Add(record);
            record = new ServoGraphSingleRecordModel("arm positioner", colors[5].ToString(), new List<List<int>>());
            result.records.Add(record);
            foreach (var item in result.records)
            {
                for (int i = 0; i < 20; i++)
                {
                    item.data.Add(new List<int>() { random.Next() });
                }
                
            }
            return new JsonResult(Ok(result));
        }



        [HttpGet("/webcamframe")]
        public String GetWebcamFrame()
        {


            //Convert the image to PNG format


            //using (MemoryStream stream = new MemoryStream())
            //{
            //    lock (capturelock)
            //    {
            //        try
            //        {
            //            Capture an image from the webcam

            //            capture.Read(frame);
            //            Cv2.ImEncode(".jpg", frame, out pngBytes);
            //            capturelock = true;
            //        }
            //        catch (Exception)
            //        {

            //        }
            //    }

            //}

            //Serve the PNG image as an HTTP response
            //return File(pngBytes, "image/png");
            return "";
        }




        [HttpGet("/getservopos")]
        public JsonResult GetServoPos()
        {
            var result = new List<ServoPositionModel>();
            var record = new ServoPositionModel("wheel fl", new List<int>());
            result.Add(record);
            record = new ServoPositionModel("wheel fr",  new List<int>());
            result.Add(record);
            record = new ServoPositionModel("wheel rl",  new List<int>());
            result.Add(record);
            record = new ServoPositionModel("wheel rr",  new List<int>());
            result.Add(record);
            record = new ServoPositionModel("grip", new List<int>());
            result.Add(record);
            record = new ServoPositionModel("arm positioner", new List<int>());
            result.Add(record);
            foreach (var item in result)
            {
                    item.data.Add(random.Next(0,359) );

            }
            return new JsonResult(Ok(result));
        }


        [HttpGet("/hwstats")]
        public JsonResult GetHWStats()
        {

            var result = SystemInfo.GetSystemInfo();
            return new JsonResult(Ok(result));
        }
        [HttpGet("/servicestatus")]
        public JsonResult GetServiceStatus()
        {

            var result = SystemInfo.GetServiceStatus();
            return new JsonResult(Ok(result));
        }
    }
}
