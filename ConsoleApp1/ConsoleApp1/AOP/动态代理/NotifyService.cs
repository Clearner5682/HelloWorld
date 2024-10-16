using ConsoleApp1.AOP.动态代理;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.AOP
{
    [Log]
    public class NotifyService:INotifyService
    {
        public virtual void Notify()
        {
            Console.WriteLine($"Notify...");
        }
    }
}
