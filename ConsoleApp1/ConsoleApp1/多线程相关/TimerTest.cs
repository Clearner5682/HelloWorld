using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public static class TimerTest
    {
        public static void Test1()
        {
            System.Threading.Timer timer = new System.Threading.Timer(state=> {
                Console.WriteLine($"Timer定时执行：{state}");
            },"arg",5000,1000);

            Console.ReadKey();

            timer.Dispose();
        }

        public static void Test2()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (sender, e) =>
            {
                Console.WriteLine($"Timer定时执行：{e.SignalTime}");
            };
            timer.Start();

            Console.ReadKey();

            timer.Stop();
            timer.Dispose();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
