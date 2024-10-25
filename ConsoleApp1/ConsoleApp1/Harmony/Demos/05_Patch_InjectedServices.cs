using ConsoleApp1.DependencyInject;
using ConsoleApp1.DependencyInject.Services;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Harmony
{
    public class Patch_InjectedServices
    {
        public static void Test()
        {
            var harmony = new HarmonyLib.Harmony("injected_services_patcher");

            var emailSender = ServiceLocator.Instance.GetService<IEmailSender>();
            if(emailSender!= null)
            {
                // 说明容器中注入了IEmailSender的实例
                Type emailSenderType = emailSender.GetType();
                harmony.Patch(
                    original: emailSenderType.GetMethod("SendEmail"),
                    postfix: new HarmonyMethod(typeof(Patch_InjectedServices).GetMethod("AfterSendEmail"))
                );
            }
        }

        public static void AfterSendEmail()
        {
            Console.WriteLine("After Send Email");
        }
    }
}
