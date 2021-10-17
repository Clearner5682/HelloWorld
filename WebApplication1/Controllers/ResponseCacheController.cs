using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/cache")]
    [ApiController]
    public class ResponseCacheController : ControllerBase
    {

        #region Http缓存之客户端缓存
        //客户端缓存的意思就是缓存存在于客户端的硬盘中
        //浏览器发送Http请求之前会先在浏览器的缓存中找缓存是否存在，缓存是否过期
        //如果缓存存在且没有过期，则直接返回缓存的结果(cache from disk)，而不会发送Http请求到服务器端


        [HttpGet,Route("Test1")]
        [ResponseCache(Duration =60,Location =ResponseCacheLocation.Any)]
        //同下面的Test2方法
        //使用ResponseCache特性即可实现客户端缓存，相当于往响应头中写入Cache-Control:public max-age=60
        //这样使用比较优雅，因为不需要在Action中写与逻辑无关的内容
        public IActionResult Test1()
        {
            return Ok(new { ErrorCode=0,Message="",Data="Test1"});
        }

        [HttpGet,Route("Test2")]
        public IActionResult Test2()
        {
            //在响应头中写入Cache-Control
            //过期时间为相对时间，比如60秒
            Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
            {
                MaxAge=TimeSpan.FromSeconds(60),
                Private=true
            };

            return Ok(new { ErrorCode = 0, Message = "", Data = "Test2" });
        }

        [HttpGet,Route("Test3")]
        public IActionResult Test3()
        {
            var now = DateTime.Now;
            
            //在响应头中写入Last-Modified和Expire
            //Expire为绝对过期时间，比如2021-09-29 12:00:00.000
            Response.GetTypedHeaders().LastModified = new DateTimeOffset(now);
            Response.GetTypedHeaders().Expires = new DateTimeOffset(now.AddSeconds(60));

            return Ok(new { ErrorCode = 0, Message = "", Data = "Test3" });
        }

        #endregion

        #region Http缓存之服务器端缓存

        //加上了ResponseCaching中间件之后，使用该特性就会使客户端缓存变成服务器端缓存
        [HttpGet,Route("Test4")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public IActionResult Test4()
        {
            return Ok(new { ErrorCode = 0, Message = "", Data = "Test4" });
        }

        #endregion
    }
}
