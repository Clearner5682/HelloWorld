using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.AOP.动态代理
{
    public interface IInterceptor<T>:IInterceptor where T : class
    {
    }

    public interface IInterceptor
    {
        void Before();
        void After();
    }
}
