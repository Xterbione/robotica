using System.Globalization;

namespace testAPI.Models
{
    public class GS1
    {
        static string currentDate = DateTime.Today.ToString("yyyy-MM-dd");

        //public const string Ex0 = "[\"Apple\",\"Banana\",\"Orange\",\"Grape\"]";
        //public const string Ex1 = "{\"GS1\": [{\"Data\": \"7240Afnemercode4301I7041\"}]}";
        //public const string Ex1_1 = "[{\"7240Afnemercode4301I7041\"}]";
        //public const string Ex2 = "{\"GS1\": [{\"Data\": \"7240Afnemercode4301I7030_3DO\"}, {\"Data\": \"7240Afnemercode4301I70184CN\"}, {\"Data\": \"7240Afnemercode4301I7017\"}]}";
        //public const string Ex2_2 = "{\"GS1\": [{\"Data\": \"7240Afnemercode4301I7030_3DO\"}, {\"Data\": \"7240Afnemercode4301I70184CN\"}, {\"Data\": \"7240Afnemercode4301I7017\"}]}";
        //public const string Ex3 = "{\"GS1\": [{\"Data\": \"7240Afnemercode4301I7050\"}, {\"Data\": \"7240Afnemercode4301I7041\"}, {\"Data\": \"7240Afnemercode4301I7032\"}]}";
        //public const string Ex4 = "{\"GS1\": [{\"Data\": \"7240Afnemercode4301I6860\"}, {\"Data\": \"7240Afnemercode4301E0001\"}, {\"Data\": \"7240Afnemercode4301I6803\"}, {\"Data\": \"7240Afnemercode4301E0006\"}, {\"Data\": \"7240Afnemercode4301E6318\"}, {\"Data\": \"7240Afnemercode4301E0525\"}]}";

        public static List<string> getFile()
        {
            Console.WriteLine(DateTime.Today.ToString("yyyy-MM-dd"));

            string path = @"C:\Users\Public\" + currentDate + ".txt";

            List<string> strings = new List<string>(File.ReadLines(path).TakeLast(5));

            /*if (strings.Last() == null)
            {
                string.remove()
            }*/

            return strings;
        }
    }
}