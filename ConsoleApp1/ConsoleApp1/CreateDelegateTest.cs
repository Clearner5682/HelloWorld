using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    public class CreateDelegateTest
    {
        public static void Test()
        {
            Caculator caculator = new Caculator();
            Func<int> func1 = caculator.Add;
            Func<int> func2 = new Func<int>(caculator.Add);

            Type type = caculator.GetType();
            MethodInfo methodInfo = type.GetMethod("Add");
            //Delegate.CreateDelegate(typeof(Func<int>), methodInfo);//用于根据静态方法动态创建委托
            Func<int> func3 = (Func<int>)methodInfo.CreateDelegate(typeof(Func<int>), caculator);
            var result = func3();
        }
    }

    public class Caculator
    {
        public int Add()
        {
            return 100;
        }
    }
}
