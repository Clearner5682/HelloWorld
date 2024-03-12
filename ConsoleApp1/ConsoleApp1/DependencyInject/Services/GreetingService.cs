using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.DependencyInject.Services
{
    public class GreetingService : IGreetingService
    {
        public virtual void Greeting()
        {
            Console.WriteLine($"Greeting...");
        }
    }
}
