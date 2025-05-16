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
using WebApplication1.MessageQueue;
using Hangfire;
using WebApplication1.Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApplication1.DependencyInject.Services;
using WebApplication1.EFCore;

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

            #region JWT Authentication

            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configOptions=> {
            //        configOptions.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            // 3+2
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWT-SignKey"))),
            //            ValidateIssuer=false,
            //            ValidIssuer ="testIssuer",
            //            ValidateAudience=false,
            //            ValidAudience ="testAudience",


            //            RequireExpirationTime=true,
            //            ValidateLifetime=true,
            //            ClockSkew=TimeSpan.FromSeconds(0)
            //        };
            //    });

            #endregion

            #region Cookie Authentication
            
            services.AddAuthentication(configOptions=> {
                configOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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
                    configOptions.SlidingExpiration = true;
                    configOptions.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = CustomCookieAuthenticationEvents.OnValidateAsync,
                        OnRedirectToLogin=CustomCookieAuthenticationEvents.OnRedirectToLogin,
                        OnRedirectToAccessDenied = CustomCookieAuthenticationEvents.OnRedirectToAccessDenied,
                        OnSigningIn = CustomCookieAuthenticationEvents.OnSigningIn,
                    };
                });


            #endregion

            EngineContext.Init(services.BuildServiceProvider());

            #region RabbitMQ

            //services.AddSingleton<RabbitMqClient>();
            //services.Configure<RabbitMqEventBusOptions>(Configuration.GetSection("RabbitMQ"));
            //services.AddHostedService<MyNormalListener>();

            #endregion

            #region Hangfire
            
            //services.AddHangfire(configuration =>
            //{
            //    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            //    configuration.UseSimpleAssemblyNameTypeSerializer();
            //    configuration.UseRecommendedSerializerSettings();
            //    configuration.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"));
            //    configuration.UseActivator(new ContainerJobActivator(services.BuildServiceProvider()));
            //});
            //services.AddHangfireServer();

            #endregion

            services.AddTransient<EmailSender>();

            #region EFCore

            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TestDb20240721"), sqlServerOptions =>
                {
                    
                });
            });

            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "WebApplication1 API", 
                    Version = "v1",
                    Contact = new OpenApiContact { Name = "Your Name" }
                });
            });
            #endregion


            services.AddTransient<IJobEngine, JobEngine>();
            services.AddTransient<IActivityExecutor, ActivityExecutor>();
            ActivityExecutor.OnActivitySucceed += JobEngine.ActivitySucceedHandler;
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

            //app.UseHangfireDashboard();

            app.UseRouting();

            #region Swagger Middleware

            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 API V1");
                c.RoutePrefix = "swagger";
            });
            
            #endregion

            #region Http Response Compression

            //app.UseResponseCompression();

            app.UseWhen(context => {
                return context.Request.Path == "/api/compression/test1"||context.Request.Path=="/api/compression/test2";
            }, applicationBuilder => {
                applicationBuilder.UseResponseCompression();
            });

            #endregion

            #region Http Response Caching

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

                //endpoints.MapHangfireDashboard();
            });
        }
    }
}
