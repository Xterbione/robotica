//using Newtonsoft.Json;

//namespace testAPI.Models
//{
//    public class GS1Model
//    {
//        public string GS1 { get; set; }

//        //public void setGS1(string setter){GS1 = setter;}

//        public static List<GS1Model> getJson()
//        {
//            List<GS1Model> bla = new List<GS1Model>();// { "Something" };
//            var temp = JsonConvert.DeserializeObject<List<GS1Model>>(File.ReadAllText(@"C:\Users\Public\file.txt"));

//            /*foreach (var x in temp)
//            {
//                GS1Model z = new GS1Model();
//                z.GS1 = Convert.ToString(x);
//                //string y = Convert.ToString(x);
//                //bla.Append(y);
//                bla.Add(z);
//            }*/

//            return bla;
//        }
//    }
//}