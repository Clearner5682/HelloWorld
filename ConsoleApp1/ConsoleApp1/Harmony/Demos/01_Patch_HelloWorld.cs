using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Harmony
{
    public class Patch_HelloWorld
    {
        public static void Test() 
        {
            var harmony = new HarmonyLib.Harmony("hello_world_patcher");

            // Patch 没有返回值的方法
            harmony.Patch(
                original: AccessTools.Method(typeof(HelloWorld),"SayHello"), 
                prefix: new HarmonyMethod(typeof(Patch_HelloWorld).GetMethod("BeforeSayHello")),
                postfix: new HarmonyMethod(SymbolExtensions.GetMethodInfo(() => AfterSayHello))
            );
        }

        // 注意Patch的方法必须是public static
        public static void BeforeSayHello()
        {
            Console.WriteLine("Before SayHello");
        }

        // 注意Patch的方法必须是public static
        public static void AfterSayHello()
        {
            Console.WriteLine("After SayHello");
        }
    }
}
