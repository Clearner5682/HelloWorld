using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp1.事件和委托
{
    public static class DelegateTest
    {
        public static event Action TestEvent;
        public static void Test()
        {
            TestEvent += () =>
            {
                Console.WriteLine("事件开始执行...");
                Thread.Sleep(3000);
                Console.WriteLine("事件执行完毕");
            };
            TestEvent += () =>
            {
                Console.WriteLine("事件开始执行...");
                Thread.Sleep(3000);
                Console.WriteLine("事件执行完毕");
            };
            TestEvent();

            Console.WriteLine("主线程继续执行");
        }
    }
}
