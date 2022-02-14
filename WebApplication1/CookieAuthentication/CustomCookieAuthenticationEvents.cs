using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace WebApplication1
{
    public class CustomCookieAuthenticationEvents
    {
        public static Task OnValidateAsync(CookieValidatePrincipalContext context)
        {
            var logger =(ILogger<CustomCookieAuthenticationEvents>)context.HttpContext.RequestServices.GetService(typeof(ILogger<CustomCookieAuthenticationEvents>));
            
            var principal = context.Principal;
            var identity = principal.Identity;

            if (identity.IsAuthenticated)
            {
                logger.LogInformation($"Cookie身份认证成功了，用户信息：{string.Join(",", principal.Claims.Select(o => o.Type + ":" + o.Value))}");
            }
            else
            {
                logger.LogInformation("Cookie身份认证失败了");
            }

            return Task.CompletedTask;
        }

        public static async Task OnRedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            // 只要是身份认证失败了，就会进这里
            var logger = (ILogger<CustomCookieAuthenticationEvents>)context.HttpContext.RequestServices.GetService(typeof(ILogger<CustomCookieAuthenticationEvents>));
            logger.LogInformation("这个事件默认情况下是会跳转登录页的，现在重写为返回401和未登录（登录过期、Token错误）的提示");

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.Headers.Add("Content-Type", "json");

            await context.HttpContext.Response.WriteAsync("{'Message':'未登录'}");
        }

        public static Task OnRedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            var logger = (ILogger<CustomCookieAuthenticationEvents>)context.HttpContext.RequestServices.GetService(typeof(ILogger<CustomCookieAuthenticationEvents>));
            logger.LogInformation("这个事件默认情况下是会跳转访问被拒绝的页面的，现在重写为返回403");

            return Task.CompletedTask;
        }

        public static Task OnSigningIn(CookieSigningInContext context)
        {
            var logger = (ILogger<CustomCookieAuthenticationEvents>)context.HttpContext.RequestServices.GetService(typeof(ILogger<CustomCookieAuthenticationEvents>));
            logger.LogInformation("HttpContext.SignInAsync执行成功了就表示登录完成了");

            return Task.CompletedTask;
        }
    }
}
