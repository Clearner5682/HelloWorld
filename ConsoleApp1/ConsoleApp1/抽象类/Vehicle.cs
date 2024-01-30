using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.抽象类
{
    public abstract class Vehicle
    {
        public int MaxSpeed { get; protected set; } = 100;
        public virtual int Capacity { get; set; } = 1;

        public virtual void Run(Vehicle vehicle)
        {
            Console.WriteLine($"{this.GetType().FullName} run...,Speed:{this.MaxSpeed},Capacity:{this.Capacity}");
        }
    }
}
