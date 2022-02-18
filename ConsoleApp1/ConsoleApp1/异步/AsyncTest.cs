using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class AsyncTest
    {
        // 异步编程模型(Asynchronous Programming Model)
        public static void TestAPM()
        {
            Console.WriteLine($"主线程：{Thread.CurrentThread.ManagedThreadId}");
            Func<int,int> computeDel = input =>
            {
                Console.WriteLine($"计算线程：{Thread.CurrentThread.ManagedThreadId}，计算中");
                Thread.Sleep(3000);

                return input * input;
            };

            // 注意这种异步的实现方式在.netcore中已经不支持了
            computeDel.BeginInvoke(100, asyncResult => {
                
                var result = computeDel.EndInvoke(asyncResult);
                Console.WriteLine($"回调方法的线程：{Thread.CurrentThread.ManagedThreadId}，计算结果：{result}");

            }, null);

            Console.WriteLine($"主线程:{Thread.CurrentThread.ManagedThreadId}，继续响应");
        }

        // 基于事件的异步模式(Event-based Asynchronous Pattern)
        public static void TestEAP()
        {
            Console.WriteLine($"主线程：{Thread.CurrentThread.ManagedThreadId}");
            
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, arg) =>
            {
                Console.WriteLine($"计算线程：{Thread.CurrentThread.ManagedThreadId}，计算中");
                Thread.Sleep(3000);

                Console.WriteLine($"计算结果：{(int)arg.Argument * (int)arg.Argument}");
            };
            worker.RunWorkerAsync(100);

            Console.WriteLine($"主线程:{Thread.CurrentThread.ManagedThreadId}，继续响应");
        }

        // 基于任务的异步模式（Task-based Asynchronous Pattern）
        public static async void TestTAP()
        {
            Console.WriteLine($"主线程：{Thread.CurrentThread.ManagedThreadId}");

            Func<int,int> computeDel = input =>
            {
                Console.WriteLine($"计算线程：{Thread.CurrentThread.ManagedThreadId}，计算中");
                Thread.Sleep(3000);

                var result = input * input;
                Console.WriteLine($"计算结果：{result}");

                return result;
            };

            var computeResult = Task.Run<int>(() => {
                return Task.FromResult(computeDel(100));
            });

            Console.WriteLine($"线程:{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
