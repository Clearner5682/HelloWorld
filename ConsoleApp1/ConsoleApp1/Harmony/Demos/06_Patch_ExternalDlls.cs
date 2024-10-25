using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Harmony
{
    public class Patch_ExternalDlls
    {
        public static void Test()
        {
            var harmony = new HarmonyLib.Harmony("list_patcher");
            harmony.Patch(
                original: AccessTools.Method(typeof(List<int>), "Add"),
                prefix: new HarmonyMethod(typeof(Patch_ExternalDlls).GetMethod("ChangeListAdd"))
            );
        }

        public static void ChangeListAdd(List<int> __instance, ref int item)
        {
            item += 100;
        }
    }
}
