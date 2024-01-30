using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ConsoleApp1.抽象类
{
    public static class AbstractClassTest
    {
        public static void Test()
        {
            Car car=new Car(300,5);
            //car.MaxSpeed = 300;
            car.Run(car);
            Type carType=car.GetType();
            PropertyInfo propertyInfo = carType.GetProperty("MaxSpeed");
            propertyInfo.SetValue(car, 300);
            ConstructorInfo[] constructors = carType.GetConstructors( BindingFlags.Instance | BindingFlags.NonPublic);
            Car carNew = constructors[0].Invoke(null) as Car;


            Bus bus=new Bus();
            bus.Run(bus);
        }
    }
}
