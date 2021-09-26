using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/compression")]
    [ApiController]
    public class ResponseCompressionController : ControllerBase
    {
        [HttpGet, Route("Test1")]
        public IActionResult Test1()
        {
            return Ok(new { ErrorCode = 0, Message = "", Data = "Hello world" });
        }

        [HttpPost, Route("Test2")]
        public IActionResult Test2(UserInfo userInfo)
        {
            var headers = this.Request.Headers;
            foreach (var header in headers)
            {
                Console.WriteLine(header.Key + "=>" + header.Value);
            }

            return Ok(userInfo);
        }
    }
}
