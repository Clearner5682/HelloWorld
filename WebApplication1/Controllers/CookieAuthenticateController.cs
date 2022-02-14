using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CookieAuthenticateController : ControllerBase
    {
        private readonly ILogger<CookieAuthenticateController> logger;

        public CookieAuthenticateController(ILogger<CookieAuthenticateController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginInfo loginInfo)
        {
            this.logger.LogInformation($"用户名：{loginInfo.UserName},密码：{loginInfo.Password}");
            this.logger.LogInformation("登录成功");

            IList<Claim> claims = new List<Claim>() {
                new Claim(type:"Role",value:"sysadmin"),
                new Claim("Id",Guid.NewGuid().ToString()),
                new Claim("Name",loginInfo.UserName),
            };
            DateTime now = DateTime.Now;

            // 注意这里必须指定身份证的认证方式是Cookies
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            foreach(var claim in claims)
            {
                identity.AddClaim(claim);
            }
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(principal);

            return Ok();
        }

        [Route("GetKeyInfo")]
        [HttpGet]
        [Authorize(Roles ="admin")]
        public IActionResult GetKeyInfo()
        {
            return Ok("this is the key info");
        }

        [Route("GetPublicInfo")]
        [HttpGet]
        public IActionResult GetPublicInfo()
        {
            return Ok("this is the public info");
        }
    }
}
