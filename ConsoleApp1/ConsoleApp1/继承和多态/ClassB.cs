using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.继承和多态
{
    public class ClassB : AbstractClassBase
    {
        public override void SayHello()
        {
            Console.WriteLine("Hello from B");
        }
    }
}
