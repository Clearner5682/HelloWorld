using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Harmony.Demos
{
    public class Patch_AsyncMethod
    {
        public static void Test()
        {
            var harmony = new HarmonyLib.Harmony("async_method_patcher");

            // 修改参数
            harmony.Patch(
                original: typeof(HelloWorld).GetMethod("AddAsync"),
                postfix: new HarmonyMethod(typeof(Patch_AsyncMethod).GetMethod("GetArgsInPostfix"))
            );
        }

        public static void GetArgsInPostfix(ref int x, ref int y, dynamic __result)
        {
            Console.WriteLine($"Args In Postfix: x={x}, y={y}");

            if (__result.Status == TaskStatus.Faulted)
            {
                Console.WriteLine("Exceptions were thrown");
            }
        }
    }
}
