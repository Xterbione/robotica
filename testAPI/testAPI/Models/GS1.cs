using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace testAPI.Models
{
    public class GS1
    {
        static string currentDate = DateTime.Today.ToString("yyyy-MM-dd");

        public static JsonArray getFile()
        {
            Console.WriteLine(DateTime.Today.ToString("yyyy-MM-dd"));

            string path = @"C:\Users\Public\" + currentDate + ".txt";

            var jsonArray = new JsonArray();


            foreach (string line in File.ReadLines(path).TakeLast(5))
            {
                var jsonObject = JsonDocument.Parse(line).RootElement;

                // Add the JSON object to the array
                jsonArray.Add(jsonObject);
            }

            return jsonArray;
        }
    }
}