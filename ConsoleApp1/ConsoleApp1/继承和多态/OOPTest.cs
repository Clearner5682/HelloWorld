using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.继承和多态
{
    public class OOPTest
    {
        public static void Test()
        {
            TestC c = new TestC();
            c.Print();

            ClassB b = new ClassB();
            b.SayHello();
            b.SayGoodbye();
        }
    }
}
