using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.反射
{
    public static class ReflectionTest
    {
        public static void Test()
        {
            // 创建两个类的实例
            var obj1 = new MyClass { Id = 1, Name = "John" };
            var obj2 = new MyClass { Id = 1, Name = "John" };

            // 获取类的类型
            var type = obj1.GetType();

            // 获取类的所有属性
            var properties = type.GetProperties();

            // 遍历所有属性，比较属性值是否相等
            foreach (var property in properties)
            {
                var value1 = property.GetValue(obj1);
                var value2 = property.GetValue(obj2);

                if (!value1.Equals(value2))
                {
                    Console.WriteLine($"{property.Name} is not equal.");
                }
            }
        }
    }

    class MyClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
