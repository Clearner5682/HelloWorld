using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConsoleApp1.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args != null && args.Length > 0)
            //{
            //    EventLogHelper.Information(args[0]);
            //}
            //EventLogHelper.Information("ConsoleApp1 Execute");

            CreateDelegateTest.Test();
        }
    }

    
}
