using ConsoleApp1.DependencyInject;
using ConsoleApp1.DependencyInject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Harmony
{
    public class HarmonyTest
    {
        public static void Test()
        {
            Patch_HelloWorld.Patch();
            Patch_ModifyArgs.Test();
            Patch_ModifyResult.Test();
            Patch_AccessOriginalInstance.Test();
            Patch_InjectedServices.Test();
            Patch_ExternalDlls.Test();

            var instance = new HelloWorld("danny_hong");

            instance.SayHello();
            var result = instance.Add(100, 55);

            ServiceLocator.Instance.GetService<IEmailSender>().SendEmail();

            var list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
        }
    }
}
