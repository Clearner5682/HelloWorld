using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.AOP
{
    public class StaticProxyTest
    {
        public static void Test()
        {
            IBusinessClass proxyInstance = StaticProxy.GetProxy<IBusinessClass, BusinessClass>();
            proxyInstance.Work();
        }
    }
}
