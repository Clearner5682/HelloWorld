using Common;
using ConsoleApp1.AOP;
using ConsoleApp1.DependencyInject.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Activity = Common.Activity;

namespace ConsoleApp1.DependencyInject
{
    public class DependencyInjectTest
    {
        public static List<Assembly> externalAssemblies { get; set; } = new List<Assembly>();

        public static void Test()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IGreetingService, GreetingService>();
            serviceCollection.AddSingleton<IGreetingService, ChineseGreetingService>();
            serviceCollection.AddSingleton<IGreetingService, EnglishGreetingService>();
            //serviceCollection.AddSingleton<INotifyService, NotifyService>();

            DynamicProxyTest.Test(serviceCollection);

            // 从外部程序集注入服务
            var externalLibPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomExternalLib");
            if (Directory.Exists(externalLibPath))
            {
                Directory.GetFiles(externalLibPath, "*.dll").ToList().ForEach(dll =>
                {
                    var assembly = Assembly.LoadFile(dll);
                    externalAssemblies.Add(assembly);
                    var types = assembly.GetTypes();
                    var activityTypes = types.Where(t => typeof(IActivity).IsAssignableFrom(t));
                    foreach (var type in types)
                    {
                        serviceCollection.AddSingleton(typeof(IActivity),type);
                    }
                });
            }

            ServiceLocator.Instance.Intercept(typeof(INotifyService), typeof(NotifyService));
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            ServiceLocator.Instance.SetServiceProvider(serviceProvider);
            

            var greetingService = serviceProvider.GetService<IGreetingService>();
            greetingService.Greeting();
            var greetingService1 = ServiceLocator.Instance.GetService<IGreetingService>();
            greetingService1.Greeting();
            var isEqual = greetingService.Equals(greetingService1);

            var myApprove = serviceProvider.GetService<IActivity>();
            myApprove.Execute();

            Type activityType = externalAssemblies.First().GetTypes().First(o => typeof(IActivity).IsAssignableFrom(o));
            IActivity instance = (IActivity)Activator.CreateInstance(activityType);
            instance.Execute();
        }
    }
}
