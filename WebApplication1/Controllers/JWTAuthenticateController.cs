using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTAuthenticateController : ControllerBase
    {
        private readonly ILogger<JWTAuthenticateController> logger;
        private readonly IConfiguration configuration;

        public JWTAuthenticateController(ILogger<JWTAuthenticateController>logger,IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(LoginInfo loginInfo)
        {
            this.logger.LogInformation($"用户名：{loginInfo.UserName},密码：{loginInfo.Password}");
            this.logger.LogInformation("登录成功");

            var token = GetToken(loginInfo.UserName);

            return Ok(token);
        }

        private string GetToken(string username)
        {
            IList<Claim> claims = new List<Claim>() {
                new Claim(type:"Role",value:"sysadmin"),
                new Claim("Id",Guid.NewGuid().ToString()),
                new Claim("Name",username),
            };
            DateTime now = DateTime.Now;
            DateTime notBefore = now;
            DateTime expire = now.AddMinutes(1);

            string signKey = this.configuration.GetValue<string>("JWT-SignKey");
            SecurityKey securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signKey));// 用来加密的Key
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);// 加密的算法

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken("testIssuer1", "testAudience1", claims, notBefore, expire, signingCredentials);

            string token1 = jwtSecurityToken.ToString();// Header和Payload的序列化结果
            string token2 = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);// 完整的JWT-Token

            return token2;
        }

        [Route("GetKeyInfo")]
        [HttpGet]
        [Authorize]
        public IActionResult GetKeyInfo()
        {
            return Ok("this is the key info");
        }
    }
}
