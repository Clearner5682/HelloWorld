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
using ConsoleApp1.继承和多态;
using ConsoleApp1.多线程相关;
using ConsoleApp1.反射.Emit;

namespace ConsoleApp1
{
    partial class Program
    {
        static void Main(string[] args)
        {
            DependencyInjectTest.Test();

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("You pressed CTRL+C");
                e.Cancel = false;
            };

            EmitTest2.Test();

            Type carType = EmitTypeList.GetType("Car");
            IVehicle car = (IVehicle)Activator.CreateInstance(carType,"Benz");
            car.Run();

            HelloFrom("Source Generator");

            Console.ReadKey();
        }

        static partial void HelloFrom(string name);
    }
}
