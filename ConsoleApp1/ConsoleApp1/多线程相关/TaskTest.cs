using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.多线程相关
{
    public class TaskTest
    {
        public static void Test()
        {
            try
            {
                bool isSuccess = false;
                bool isCompleted = Task.Run(() =>
                {
                    int count = 4;
                    for (int i = 0; i < count; i++)
                    {
                        Console.WriteLine($"Task is running...{i + 1}");
                        Thread.Sleep(1000);
                    }
                    isSuccess = true;
                    //throw new Exception("An error occured");
                }).Wait(3000);

                Console.WriteLine($"IsSuccess: {isSuccess}");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
