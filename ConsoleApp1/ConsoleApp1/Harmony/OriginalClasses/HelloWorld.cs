using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Harmony
{
    public class HelloWorld
    {
        private string _name;

        public HelloWorld(string name)
        {
            _name = name;
        }

        public void SayHello()
        {
            Console.WriteLine($"Hello World: {_name}");
        }

        public int Add(int x, int y)
        {
            int result = x + y;

            Console.WriteLine($"Added Result:{result}");

            return result;
        }

        public async Task<int> AddAsync(int x,int y)
        {
            await Task.CompletedTask;

            int result = x + y;

            Console.WriteLine($"Async Added Result:{result}");

            throw new Exception("Async Exception");

            return result;
        }

        private void PrivateMethod()
        {
            Console.WriteLine($"Private Method:{_name}");
        }
    }
}
