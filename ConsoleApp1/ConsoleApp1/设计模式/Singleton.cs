using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    public class LazySingleton
    {
        private LazySingleton()
        {

        }

        private static object _lockObj = new object();
        private static LazySingleton _instance;
        public static LazySingleton Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                lock (_lockObj)
                {
                    if (_instance != null)
                    {
                        return _instance;
                    }

                    _instance = new LazySingleton();

                    return _instance;
                }

                #region 如果不加锁，就会导致多个线程获取多个实例，例如下面的代码

                //if (_instance != null)
                //{
                //    return _instance;
                //}

                //_instance = new LazySingleton();

                //return _instance;

                #endregion
            }
        }

        public void SayHello()
        {
            Console.WriteLine($"Hello My HashCode is {this.GetHashCode()} ,ThreadId is {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    public class HungrySingleton
    {
        private HungrySingleton()
        {

        }

        public static HungrySingleton Instance { get; }
        static HungrySingleton()
        {
            Instance = new HungrySingleton();
        }

        public void SayHello()
        {
            Console.WriteLine($"Hello My HashCode is {this.GetHashCode()} ,ThreadId is {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    public class SingletonTest
    {
        public static void Test()
        {
            for(var i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(state=> {
                    LazySingleton.Instance.SayHello();
                });
            }
        }
    }
}
