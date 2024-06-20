using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.继承和多态
{
    public class TestB:TestA
    {
        public TestB()
        {
            this.Name = "TestB";
        }

        public override void Print()
        {
            Console.WriteLine(GetMessage());
        }

        protected virtual string GetMessage()
        {
            return $"Message from {this.Name}";
        }
    }
}
