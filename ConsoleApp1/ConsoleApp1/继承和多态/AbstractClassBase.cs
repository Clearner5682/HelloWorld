using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.继承和多态
{
    public abstract class AbstractClassBase
    {
        // 抽象方法只能有申明，不能有实现
        // 子类必须重写抽象方法
        public abstract void SayHello();
        
        // 虚方法必须有实现
        // 子类可以重写，也可以不重写
        public virtual void SayGoodbye()
        {
            Console.WriteLine("Goodbye");
        }
    }
}
