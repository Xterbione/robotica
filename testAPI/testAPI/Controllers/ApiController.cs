using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using testAPI.Models;

namespace testAPI.Controllers
{



    [Route("/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// access to predefined color types
        /// </summary>
        /// 
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
          
        }

        /// <summary>
        /// gets all the hardware information
        /// </summary>
        /// <returns>JsonResult</returns>

        [HttpGet("/hwstats")]
        public JsonResult GetHWStats()
        {

            var result = SystemInfo.GetSystemInfo();
            return new JsonResult(Ok(result));
        }


        [HttpGet("/Nodes")]
        public JsonResult GetNodeList()
        {

            var result = SystemInfo.GetNodeList();
            return new JsonResult(Ok(result));
        }

        /// <summary>
        /// gets all linux services and wether they are enabled or not
        /// </summary>
        /// <returns></returns>
        [HttpGet("/servicestatus")]
        public JsonResult GetServiceStatus()
        {

            var result = SystemInfo.GetServiceStatus();
            return new JsonResult(Ok(result));
        }

    }
}
