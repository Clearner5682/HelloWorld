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

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            System.AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                Console.WriteLine("Hello world");
            };
            //TimerTest.Test2();
            //LinqTest.Test();
            //ExpressionTreeTest.Test();
            for(int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    new ThreadTest().Test22();
                }));
                thread.Start();
            }

            Console.ReadKey();
        }

        private static long CreateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();

            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
