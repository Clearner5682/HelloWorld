using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApplication1.Models;
using Utils;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    [Route("api/Test")]
    [ApiController]
    public class TestController:ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private static string outAppKey = ConfigurationManager.AppSettings["outAppKey"];

        public TestController()
        {
            this.httpContextAccessor = EngineContext.Current.Resolve<IHttpContextAccessor>();
            var headers = this.httpContextAccessor.HttpContext.Request.Headers;
        }

        [HttpGet,Route("Test1")]
        public IActionResult Test1()
        {
            var obj = new { ErrorCode = 0, Message = "", Data = "Hello world" };
            var str = JsonConvert.SerializeObject(obj);

            return Ok(obj);
        }

        [HttpPost,Route("Test2")]
        public IActionResult Test2(UserInfo userInfo)
        {
            var headers = this.Request.Headers;
            foreach(var header in headers)
            {
                Console.WriteLine(header.Key + "=>" + header.Value);
            }

            return Ok(userInfo);
        }

        [HttpPost,Route("Upload")]
        public IActionResult Upload(string name)
        {
            IFormFileCollection formCollection = this.Request.Form.Files;
            var file = formCollection[0];
            string fileName = file.FileName;
            long fileSize = file.Length;

            return Ok(fileName+"=>"+fileSize);
        }

        [HttpGet]
        public IActionResult GetIpAddress()
        {
            string returnStr = $"RemoteIpAddress:{this.HttpContext.Connection.RemoteIpAddress.ToString()}:{this.HttpContext.Connection.RemotePort}\r\n";
            returnStr += $"LocalIpAddress:{this.HttpContext.Connection.LocalIpAddress.ToString()}:{this.HttpContext.Connection.LocalPort}";

            return Ok(returnStr);
        }
    }
}
