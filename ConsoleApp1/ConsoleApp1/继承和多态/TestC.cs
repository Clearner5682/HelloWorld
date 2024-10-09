using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.继承和多态
{
    public class TestC:TestB
    {
        public TestC()
        {
            this.Name = "TestC";
        }

        public override void Print()
        {
            Console.WriteLine("I am TestC");
            base.Print();
        }

        //protected override string GetMessage()
        //{
        //    return $"Message from C";
        //}
    }
}
