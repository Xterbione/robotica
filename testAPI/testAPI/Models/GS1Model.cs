using Newtonsoft.Json;

namespace testAPI.Models
{
    public class GS1Model
    {
        //public List<string> d { get; set; }

        public static List<string> getJson()
        {
            List<string> bla = new List<string>();// { "Something" };
            var temp = JsonConvert.DeserializeObject<string>(File.ReadAllText(@"C:\Users\Public\file.txt"));

            foreach (var x in temp)
            {
                string y = Convert.ToString(x);
                bla.Append(y);
            }

            return bla;
        }
    }
}