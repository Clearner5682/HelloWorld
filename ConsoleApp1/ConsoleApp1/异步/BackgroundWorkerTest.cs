using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    public static class BackgroundWorkerTest
    {
        public static void Test()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += ProcessData;
            worker.ProgressChanged += ShowProgress;
            worker.RunWorkerCompleted += WorkCompleted;
            worker.RunWorkerAsync("Task1");
        }

        private static void WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("数据处理完成，BackgroundWorker退出");
            var worker = sender as BackgroundWorker;
            if (worker != null)
            {
                worker.Dispose();
            }
        }

        // 处理数据
        public static void ProcessData(object sender,DoWorkEventArgs args)
        {
            var taskName = args.Argument.ToString();
            var worker = sender as BackgroundWorker;
            Console.WriteLine("处理数据中...");
            for(int i = 0; i < 99; i++)
            {
                Thread.Sleep(500);
                worker.ReportProgress(i);
            }
        }

        // 展示进度
        public static int lastPercent = 0;
        public static void ShowProgress(object sender,ProgressChangedEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            int percent = e.ProgressPercentage;
            string taskName = "Task1";
            if (percent == 0)
            {
                Console.Write($"{taskName}:");
                return;
            }
            for(int i = 0; i < percent - lastPercent; i++)
            {
                Console.Write("<");
            }
            lastPercent = percent;
        }
    }
}
