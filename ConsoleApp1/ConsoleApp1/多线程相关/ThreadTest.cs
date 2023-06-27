using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    public class ThreadTest
    {
        // initialState为false,即初始状态没有Set信号，等待的线程仍然等待
        // initialState为true，即初始状态已经有了Set信号，等待的线程直接开始执行
        public static AutoResetEvent AutoResetEvent = new AutoResetEvent(false);
        // ManualResetEvent和AutoResetEvent的区别是
        // 1=>AutoResetEvent每次Set只能唤起一个WaitOne的线程执行，然后就会自动执行Reset。当下次再Set时就会再唤起一个WaitOne的线程
        // 2=>ManualResetEvent每次Set会唤起所有WaitOne的线程执行，并且它不会自动Reset。如果不手动Reset,其他正在WaitOne的线程都会执行
        // 3=>Manual和Auto的根本区别就是一个在Set之后要手动执行Reset来清除信号，一个在Set之后会自动执行Reset来清除信号
        public static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        public void Test11()
        {
            Thread t100 = new Thread(() => {
                while (true)
                {
                    var input = Console.ReadKey();
                    if (input.Key == ConsoleKey.D1)
                    {
                        AutoResetEvent.Set();//每调用一次Set方法，就可以让一个等待的线程执行（如果多个线程都在等待，则从等待的队列中唤醒一个执行）
                    }
                    Thread.Sleep(1000);
                }
            });
            t100.Start();

            AutoResetEvent.WaitOne();

            Console.WriteLine("Test11");
        }

        public void Test22()
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                int num = 0;
                while (num < 100)
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}工作线程执行...{num}");
                    num++;
                    Thread.Sleep(1000);
                }
            });

            Console.WriteLine("Test22");
        }

        public static void TestJoin()
        {
            Thread.CurrentThread.Name = "main";
            Thread t = new Thread(new ThreadStart(Cook));
            t.Name = "Cook";
            t.Start();

            //Join的使用
            t.Join();//阻塞当前线程，直到调用Join的线程执行完毕

            Thread t2 = new Thread(Eat);
            t2.Name = "Eat";
            t2.Start();
        }

        public static void TestAutoResetEvent()
        {
            Thread t1 = new Thread(() => {
                Console.WriteLine("厨师开始做饭...");
                Thread.Sleep(3000);
                Console.WriteLine("厨师做完饭了");
                AutoResetEvent.Set();//厨师做完饭了，通知顾客可以开吃了
            });
            t1.Start();
            Thread t2 = new Thread(() => {
                AutoResetEvent.WaitOne();//顾客等待开吃
                Console.WriteLine($"顾客2开吃");
            });
            t2.Start();
            Thread t3 = new Thread(() => {
                AutoResetEvent.WaitOne();//顾客等待开吃
                Console.WriteLine($"顾客3开吃");
            });
            t3.Start();
            Thread t4 = new Thread(() => {
                AutoResetEvent.WaitOne();//顾客等待开吃
                Console.WriteLine($"顾客4开吃");
            });
            t4.Start();
            Thread t100 = new Thread(()=> {
                while (true)
                {
                    var input = Console.ReadKey();
                    if (input.Key == ConsoleKey.D1)
                    {
                        AutoResetEvent.Set();//每调用一次Set方法，就可以让一个等待的线程执行（如果多个线程都在等待，则从等待的队列中唤醒一个执行）
                    }
                    Thread.Sleep(1000);
                }
            });
            t100.Start();
        }

        public static void TestManualResetEvent()
        {
            Thread t1 = new Thread(() => {
                Console.WriteLine("厨师开始做饭...");
                Thread.Sleep(3000);
                Console.WriteLine("厨师做完饭了");
                ManualResetEvent.Set();
            });
            t1.Start();
            Thread t2 = new Thread(()=> {
                ManualResetEvent.WaitOne();
                Console.WriteLine("顾客2开吃");
                //通过效果可以看到，ManualResetEvent如果不手动调用Reset，则无论有多少个WaitOne都会直接执行
                //ManualResetEvent.Reset();
                ManualResetEvent.WaitOne();
                Console.WriteLine("顾客2喝茶");
            });
            t2.Start();
            Thread t3 = new Thread(() =>
            {
                ManualResetEvent.WaitOne();
                Console.WriteLine("顾客3开吃");
                //通过效果可以看到，ManualResetEvent如果不手动调用Reset，则无论有多少个WaitOne都会直接执行
                //ManualResetEvent.Reset();
                ManualResetEvent.WaitOne();
                Console.WriteLine("顾客3喝茶");
            });
            t3.Start();
        }

        public static void Cook()
        {
            Console.WriteLine($"厨师开始做饭...");
            Thread.Sleep(2000);
            Console.WriteLine($"厨师做完饭了...");
        }

        public static void Eat()
        {
            Console.WriteLine($"顾客开始吃饭");
        }

        public static void TestThreadPool()
        {
            for(int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(state => {
                    Console.WriteLine($"开始执行，ThreadId:{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"执行完毕，ThreadId:{Thread.CurrentThread.ManagedThreadId}");
                });
            }
        }
    }
}
