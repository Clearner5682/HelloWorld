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

            #region JWT�����֤

            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configOptions=> {
            //        configOptions.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            // 3+2
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWT-SignKey"))),
            //            ValidateIssuer=false,// �������Ϊtrue�������Ƶ�Issuer���������ָ����Issuerһ�£�������֤ʧ��
            //            ValidIssuer ="testIssuer",
            //            ValidateAudience=false,// �������Ϊtrue�������Ƶ�Audience���������ָ����Audienceһ�£�������֤ʧ��
            //            ValidAudience ="testAudience",


            //            RequireExpirationTime=true,
            //            ValidateLifetime=true,// ���Ƶ���Ч�ڻ���һ��ҡ���ڣ�Ĭ��ֵΪ300�룬Ҳ������ԭ��Ч�ڹ���5����֮��������Ȼ����Ч��
            //            ClockSkew=TimeSpan.FromSeconds(0)// û��ҡ����
            //        };
            //    });

            #endregion

            #region Cookie�����֤
            
            services.AddAuthentication(configOptions=> {
                configOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;// ������DefaultScheme������DefaultSignInScheme��DefaultChallengeScheme��DefalutForbidScheme����ʹ�ø�Scheme
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
                    configOptions.SlidingExpiration = true;// ����ˢ�¹���ʱ��
                    configOptions.Events = new CookieAuthenticationEvents
                    {
                        //�����֤�ɹ�֮��ִ�е��¼�
                        OnValidatePrincipal = CustomCookieAuthenticationEvents.OnValidateAsync,
                        //δ��¼��ת�¼���Ĭ�ϵ�����»���ת�����ǰ��˷��벻̫�Ѻã����������д���¼���
                        OnRedirectToLogin=CustomCookieAuthenticationEvents.OnRedirectToLogin,
                        //��Ȩ�ޣ���֤�ɹ�����Ȩʧ�ܣ���ת�¼���ͬ�ϣ�
                        OnRedirectToAccessDenied = CustomCookieAuthenticationEvents.OnRedirectToAccessDenied,
                        //��¼�ɹ�֮��ִ��
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

            #region ѹ��Http��Ӧ���м��

            //app.UseResponseCompression();

            // ���ֻ����ض�����ѹ��Http��Ӧ������ʹ��UseWhen���������ض�����������²Ž�ѹ��Http��Ӧ���м�����뵽�ܵ���
            app.UseWhen(context => {
                return context.Request.Path == "/api/compression/test1"||context.Request.Path=="/api/compression/test2";
            }, applicationBuilder => {
                applicationBuilder.UseResponseCompression();
            });

            #endregion

            #region ����Http��Ӧ���м��

            //ʹ���˸��м��������ͱ���˷������˻���
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
