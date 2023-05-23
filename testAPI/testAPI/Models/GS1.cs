using Newtonsoft.Json;

namespace testAPI.Models
{
    public class GS1
    {
        public string Data { get; set; }

        public const string Ex0 = "[\"Apple\",\"Banana\",\"Orange\",\"Grape\"]";
        public const string Ex1 = "{\"GS1\": [{\"Data\": \"7240Afnemercode4301I7041\"}]}";
        public const string Ex1_1 = "[{\"7240Afnemercode4301I7041\"}]";
        public const string Ex2 = "{\"GS1\": [{\"Data\": \"7240Afnemercode4301I7030_3DO\"}, {\"Data\": \"7240Afnemercode4301I70184CN\"}, {\"Data\": \"7240Afnemercode4301I7017\"}]}";
        public const string Ex2_2 = "{\"GS1\": [{\"Data\": \"7240Afnemercode4301I7030_3DO\"}, {\"Data\": \"7240Afnemercode4301I70184CN\"}, {\"Data\": \"7240Afnemercode4301I7017\"}]}";
        public const string Ex3 = "{\"GS1\": [{\"Data\": \"7240Afnemercode4301I7050\"}, {\"Data\": \"7240Afnemercode4301I7041\"}, {\"Data\": \"7240Afnemercode4301I7032\"}]}";
        public const string Ex4 = "{\"GS1\": [{\"Data\": \"7240Afnemercode4301I6860\"}, {\"Data\": \"7240Afnemercode4301E0001\"}, {\"Data\": \"7240Afnemercode4301I6803\"}, {\"Data\": \"7240Afnemercode4301E0006\"}, {\"Data\": \"7240Afnemercode4301E6318\"}, {\"Data\": \"7240Afnemercode4301E0525\"}]}";

        static List<string> jsonList = new List<string>();

        public static List<string> getJson(string jsonString)
        {
            jsonList.Clear();
            try
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
                Console.WriteLine(jsonObject.ToString());

                // Check if the deserialized object is an array
                if (jsonObject is Newtonsoft.Json.Linq.JArray jsonArray)
                {
                    foreach (var item in jsonObject)
                    {
                        jsonList.Add(item.ToString());
                        Console.WriteLine(item.ToString());
                    }
                }
                else
                {
                    jsonList.Add(jsonObject.ToString());
                    Console.WriteLine(jsonObject.ToString());
                }
            }
            catch (JsonReaderException ex)
            {
                // Handle exception if the JSON string is invalid
                jsonList.Add("Invalid JSON");
                Console.WriteLine("Error: Invalid JSON string - " + ex.Message);
            }

            return jsonList;
        }
    }
}