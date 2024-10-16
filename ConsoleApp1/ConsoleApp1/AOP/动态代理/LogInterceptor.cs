using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.AOP.动态代理
{
    public class LogInterceptor : IInterceptor<LogAttribute>
    {
        public void Before()
        {
            Console.WriteLine("Log Before");
        }

        public void After()
        {
            Console.WriteLine("Log After");
        }
    }
}
