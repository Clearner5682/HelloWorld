using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public static class ServiceExtension
    {
        public static void Method1<T>(this T t)
        {
            Console.WriteLine(typeof(T)+"|" + t.ToString());
        }
    }
}
