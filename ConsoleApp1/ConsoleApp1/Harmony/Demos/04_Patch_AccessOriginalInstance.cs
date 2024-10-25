using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Harmony
{
    public class Patch_AccessOriginalInstance
    {
        public static void Test()
        {
            var harmony = new HarmonyLib.Harmony("access_original_instance_patcher");
            harmony.Patch(
                original: typeof(HelloWorld).GetMethod("Add"),
                postfix: new HarmonyLib.HarmonyMethod(typeof(Patch_AccessOriginalInstance).GetMethod("AccessOriginalInstance"))
            );
        }

        public static void AccessOriginalInstance(ref object __instance)
        {
            // __instance 是原始方法的实例
            // 拿到原始方法的实例之后，可以调用原始方法的其他方法
            Type type = __instance.GetType();
            FieldInfo fieldName = type.GetField("_name",BindingFlags.Instance | BindingFlags.NonPublic);
            fieldName.SetValue(__instance, "李银河");
            MethodInfo methodSayHello = type.GetMethod("PrivateMethod",BindingFlags.Instance | BindingFlags.NonPublic);
            methodSayHello.Invoke(__instance, null);
        }
    }
}
