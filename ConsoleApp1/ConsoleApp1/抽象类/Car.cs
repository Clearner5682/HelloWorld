using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.抽象类
{
    public class Car:Vehicle
    {
        public new int MaxSpeed { get; protected set; } = 200;

        public override int Capacity { get; set; }

        //public override void Run()
        //{
        //    Console.WriteLine($"Car run...,Speed:{this.MaxSpeed},Capacity:{this.Capacity}");
        //}

        protected Car()
        {

        }

        public Car(int maxSpeed, int capacity)
        {
            MaxSpeed = maxSpeed;
            Capacity = capacity;
        }   
    }
}
