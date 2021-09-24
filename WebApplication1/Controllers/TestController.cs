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
            return Ok(new { ErrorCode = 0, Message = "", Data = "Hello world" });
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

        #region 文件流的操作

        [HttpGet,Route("CopyFile")]
        public IActionResult CopyFile()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyToByFile("copy.txt", "save.txt");
            fileHelper.CopyToByFile("copy.jpg","save.jpg");

            return Ok();
        }

        [HttpGet, Route("CopyFileAsync")]
        public async Task<IActionResult> CopyFileAsync()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyToByFile("copy.txt", "save.txt");
            await fileHelper.CopyToByFileAsync("copy.jpg", "save.jpg");

            return Ok();
        }

        [HttpGet, Route("CopyBySelf")]
        public IActionResult CopyBySelf()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyBySelf("copy.txt", "save.txt");
            fileHelper.CopyBySelf("copy.jpg", "save.jpg");

            return Ok();
        }

        [HttpGet, Route("CopyByMemory")]
        public IActionResult CopyByMemory()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyByMemory("copy.txt", "save.txt");
            fileHelper.CopyByMemory("copy.jpg", "save.jpg");

            return Ok();
        }

        [HttpGet, Route("CopyByStream")]
        public IActionResult CopyByStream()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyByStream("copy.txt", "save.txt");
            fileHelper.CopyByStream("copy.jpg", "save.jpg");

            return Ok();
        }

        [HttpGet, Route("CopyByByte")]
        public IActionResult CopyByByte()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyByByte("copy.txt", "save.txt");
            fileHelper.CopyByByte("copy.jpg", "save.jpg");

            return Ok();
        }

        #endregion
    }
}
