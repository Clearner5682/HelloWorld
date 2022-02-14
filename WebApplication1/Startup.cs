using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Middlewares;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var identityServerUrl = Configuration.GetValue<string>("IdentityServerUrl");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();

            services.AddResponseCompression(options=> { 
                
            });
            services.AddResponseCaching();

            #region JWT身份认证

            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configOptions=> {
            //        configOptions.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            // 3+2
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWT-SignKey"))),
            //            ValidateIssuer=false,// 如果这里为true，则令牌的Issuer必须和下面指定的Issuer一致，否则认证失败
            //            ValidIssuer ="testIssuer",
            //            ValidateAudience=false,// 如果这里为true，则令牌的Audience必须和下面指定的Audience一致，否则认证失败
            //            ValidAudience ="testAudience",


            //            RequireExpirationTime=true,
            //            ValidateLifetime=true,// 令牌的有效期会有一段摇摆期，默认值为300秒，也就是在原有效期过期5分钟之内令牌仍然是有效的
            //            ClockSkew=TimeSpan.FromSeconds(0)// 没有摇摆期
            //        };
            //    });

            #endregion

            #region Cookie身份认证
            
            services.AddAuthentication(configOptions=> {
                configOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;// 设置了DefaultScheme，这样DefaultSignInScheme，DefaultChallengeScheme，DefalutForbidScheme都会使用该Scheme
                //configOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(configOptions => {
                    configOptions.Cookie.Name = "MyCookieAuthentication";
                    configOptions.Cookie.HttpOnly = true;
                    //configOptions.Cookie.Domain = "";
                    configOptions.ExpireTimeSpan = TimeSpan.FromSeconds(60);
                    //configOptions.ReturnUrlParameter = "abc";
                    //configOptions.LoginPath = new PathString("/Account/Login");
                    //configOptions.AccessDeniedPath = new PathString("/Account/Denied");
                    //configOptions.LogoutPath = new PathString("/Account/Logout");
                    configOptions.Cookie.Path = "/";
                    configOptions.SlidingExpiration = true;// 滚动刷新过期时间
                    configOptions.Events = new CookieAuthenticationEvents
                    {
                        //身份认证成功之后执行的事件
                        OnValidatePrincipal = CustomCookieAuthenticationEvents.OnValidateAsync,
                        //未登录跳转事件（默认的情况下会跳转，这对前后端分离不太友好，因此这里重写该事件）
                        OnRedirectToLogin=CustomCookieAuthenticationEvents.OnRedirectToLogin,
                        //无权限（认证成功，授权失败）跳转事件（同上）
                        OnRedirectToAccessDenied = CustomCookieAuthenticationEvents.OnRedirectToAccessDenied,
                        //登录成功之后执行
                        OnSigningIn = CustomCookieAuthenticationEvents.OnSigningIn,
                    };
                });


            #endregion

            EngineContext.Init(services.BuildServiceProvider());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            #region 压缩Http响应的中间件

            //app.UseResponseCompression();

            // 如果只想对特定请求压缩Http响应，可以使用UseWhen，在满足特定条件的情况下才将压缩Http响应的中间件加入到管道中
            app.UseWhen(context => {
                return context.Request.Path == "/api/compression/test1"||context.Request.Path=="/api/compression/test2";
            }, applicationBuilder => {
                applicationBuilder.UseResponseCompression();
            });

            #endregion

            #region 缓存Http响应的中间件

            //使用了该中间件，缓存就变成了服务器端缓存
            app.UseResponseCaching();

            #endregion

            app.Use(async (context, next) => {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/index.html");
                    //await context.Response.SendFileAsync("index.html");
                    return;
                }

                await next.Invoke();
            });

            app.UseAuthentication();

            //app.UseMiddleware<JWTMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
