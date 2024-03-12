using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.AOP
{
    public class BusinessClassProxy : IBusinessClass
    {
        public void Work()
        {
            Console.WriteLine($"Before work");

            BusinessClass instance = new BusinessClass();
            instance.Work();

            Console.WriteLine($"After work");
        }
    }
}
