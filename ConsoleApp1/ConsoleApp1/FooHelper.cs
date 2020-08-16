using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class FooHelper
    {
        public int Foo(int index)
        {
            if (index < 0)
            {
                throw new Exception("序号不能为负数");
            }
            if (index == 0)
            {
                return 0;
            }
            if (index == 1 || index == 2)
            {
                return 1;
            }

            return Foo(index - 2) + Foo(index - 1);
        }

        // 1  1  2  3  5
        int index = 0;
        public int Foo2(int current, int next)
        {
            if (index < 0)
            {
                throw new Exception("序号不能为负数");
            }
            if (index == 0)
            {
                current = 0;
                next = 1;
            }
            else if (index == 1)
            {
                current = 1;
                next = 1;
            }
            else if (index == 2)
            {
                current = 1;
                next = 2;
            }
            else
            {
                var temp = current;
                current = next;
                next = next + temp;
            }
            if (index == 30)
            {
                return current;
            }
            index++;

            return Foo2(current, next);
        }
    }
}
