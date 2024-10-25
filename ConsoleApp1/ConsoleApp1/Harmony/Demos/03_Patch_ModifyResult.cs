using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Harmony
{
    public class Patch_ModifyResult
    {
        public static void Test()
        {
            var harmony = new HarmonyLib.Harmony("modify_result_patcher");
            harmony.Patch(
                original: typeof(HelloWorld).GetMethod("Add"),
                postfix: new HarmonyLib.HarmonyMethod(typeof(Patch_ModifyResult).GetMethod("ChangeResult"))
            );
        }

        public static void ChangeResult(ref int __result)
        {
            // __instance 是原始方法的实例
            // 拿到原始方法的实例之后，可以调用原始方法的其他方法
            //HelloWorld instance = __instance as HelloWorld;
            //instance.SayHello();
            // 修改返回值
            __result += 100;
            Console.WriteLine($"Result Changed to {__result}");
        }
    }
}
