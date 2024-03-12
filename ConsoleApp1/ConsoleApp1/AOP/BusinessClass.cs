using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.AOP
{
    public class BusinessClass : IBusinessClass
    {
        public void Work()
        {
            Console.WriteLine($"Working...");
        }
    }
}
