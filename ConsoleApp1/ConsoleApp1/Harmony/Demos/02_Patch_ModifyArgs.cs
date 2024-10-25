using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Harmony
{
    public class Patch_ModifyArgs
    {
        public static void Test()
        {
            var harmony = new HarmonyLib.Harmony("modify_args_patcher");

            // 修改参数
            harmony.Patch(
                original: typeof(HelloWorld).GetMethod("Add"),
                prefix: new HarmonyMethod(typeof(Patch_ModifyArgs).GetMethod("ChangeArgs"))
            );

            // 修改结果，跳过原始方法
            //harmony.Patch(
            //    original: typeof(HelloWorld).GetMethod("Add"),
            //    prefix: new HarmonyMethod(typeof(Patch_ModifyArgs).GetMethod("ChangeResultAndSkippingOriginal"))
            //);
        }

        // 注意Patch的方法必须是public static
        // 注意参数的类型和数量要和原始方法一致
        public static void ChangeArgs(ref int x, ref int y)
        {
            Console.WriteLine($"Original Args: x={x}, y={y}");
            x += 10;
            y += 20;
            Console.WriteLine($"Changed Args: x={x}, y={y}");
        }

        // __result 是原始方法的返回值，ref 表示可以修改
        // return false表示不执行原始方法
        public static bool ChangeResultAndSkippingOriginal(ref int __result)
        {
            __result = 9999;

            Console.WriteLine($"Result Changed to {__result}, Skipping Original");

            return false;
        }
    }
}
