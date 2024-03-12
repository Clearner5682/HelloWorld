using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConsoleApp1.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;
using ConsoleApp1.表达式树;
using ConsoleApp1.事件和委托;
using System.Threading;
using ConsoleApp1.反射;
using ConsoleApp1.抽象类;
using ConsoleApp1.AOP;
using Microsoft.Extensions.DependencyInjection;
using ConsoleApp1.DependencyInject.Services;
using ConsoleApp1.DependencyInject;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IGreetingService, GreetingService>();
            serviceCollection.AddSingleton<IGreetingService, ChineseGreetingService>();
            serviceCollection.AddSingleton<IGreetingService, EnglishGreetingService>();

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            ServiceLocator.Instance.SetServiceProvider(serviceProvider);

            var greetingService = serviceProvider.GetService<IGreetingService>();
            greetingService.Greeting();

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("You pressed CTRL+C");
                e.Cancel = false;
            };

            //ExpressionTreeTest.Test();
            //ExpressionTreeTest11.Test();
            //StaticProxyTest.Test();

            while (true)
            {
                Console.WriteLine("Running...");
                Thread.Sleep(1000);
            }

            Console.ReadKey();
        }

        private static long CreateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();

            return BitConverter.ToInt64(buffer, 0);
        }
    }

    public enum EnumTest
    {
        Test11,
        Test22
    }
}
