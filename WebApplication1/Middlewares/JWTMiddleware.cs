using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Middlewares
{
    public class JWTMiddleware
    {
        private readonly ILogger<JWTMiddleware> logger;
        private readonly RequestDelegate next;
        private static int invokeCount = 0;

        public JWTMiddleware(ILogger<JWTMiddleware> logger,RequestDelegate next)
        {
            invokeCount++;
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            logger.LogInformation($"这是第{invokeCount}次调用该中间件");
            var principal = httpContext.User;
            if (principal.Identity.IsAuthenticated)
            {
                logger.LogInformation($"认证已成功，用户信息：{string.Join(",", principal.Claims.Select(o => o.Type + ":" + o.Value))}");
            }
            else
            {
                logger.LogInformation("认证失败");
            }

            string token = "";
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                token = authorizationHeader.Replace("Bearer ","");
            }

            // 如果某个Token被盗用了，现在需要禁用该Token（强制下线）
            // 可以在中间件中额外加一道验证逻辑，验证Token是否在黑名单，如果在黑名单则认证失败
            // 这样有个问题就是违反了JWT的初衷，服务器端原本是不用存储和维护令牌的状态的（原本是无状态的，现在变成有状态了）
            IList<string> disabledTokens = new List<string>
            {
                "aaaa.bbbb.cccc",
                "1111.2222.3333",
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJSb2xlIjoic3lzYWRtaW4iLCJJZCI6IjZlNDZiN2RmLTU5ZDgtNDFkZC04YjVlLWUwN2U2M2ZjMDQ1YSIsIk5hbWUiOiJob25neWFuIiwibmJmIjoxNjQ0ODIwMzA2LCJleHAiOjE2NDQ4MjAzNjYsImlzcyI6InRlc3RJc3N1ZXIxIiwiYXVkIjoidGVzdEF1ZGllbmNlMSJ9.7_ZuQvSUp0N546t3YIh7l_sOpsNyx6rtKr8as8AIQfI"
            };
            if (disabledTokens.Contains(token))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //httpContext.Response.ContentType = "json";
                //var result = new { Reason = 1, Message = "该令牌被禁用" };
                //var obj = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                //await httpContext.Response.WriteAsync(obj);

                //或者可以把认证失败的原因放到响应的头部
                httpContext.Response.Headers.Add("Reason", "Token Disabled");

                return;
            }


            await next(httpContext);
        }
    }
}
