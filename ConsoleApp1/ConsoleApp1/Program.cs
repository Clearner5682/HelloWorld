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

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("You pressed CTRL+C");
                e.Cancel = false;
            };

            ExpressionTreeTest.Test();

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
