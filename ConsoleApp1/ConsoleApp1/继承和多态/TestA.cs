using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.继承和多态
{
    public class TestA
    {
        protected string Name { get; set; }
        public TestA() 
        {
            this.Name = "TestA";
        }

        public virtual void Print()
        {
            Console.WriteLine(this.Name);
        }
    }
}
