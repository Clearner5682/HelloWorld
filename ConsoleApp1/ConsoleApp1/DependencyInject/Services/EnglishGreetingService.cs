using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.DependencyInject.Services
{
    public class EnglishGreetingService:GreetingService,IGreetingService
    {
        public override void Greeting()
        {
            Console.WriteLine($"Hello...");
        }
    }
}
