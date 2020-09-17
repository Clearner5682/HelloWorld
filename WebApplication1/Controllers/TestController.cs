using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/Test")]
    public class TestController
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private static string outAppKey = ConfigurationManager.AppSettings["outAppKey"];

        public TestController()
        {
            this.httpContextAccessor = EngineContext.Current.Resolve<IHttpContextAccessor>();
            var headers = this.httpContextAccessor.HttpContext.Request.Headers;
        }

        [HttpGet,Route("Test")]
        public string Test()
        {
            return outAppKey;
        }
    }
}
