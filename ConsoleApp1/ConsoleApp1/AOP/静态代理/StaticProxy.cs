using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp1.AOP
{
    public class StaticProxy
    {
        public static TInterface GetProxy<TInterface,TImplement>() where TImplement : class, new() where TInterface : class
        {
            string proxyClassName = typeof(TImplement).Name + "Proxy";
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type proxyType = null;
            foreach(Assembly assembly in assemblies)
            {
                var types = assembly.GetTypes();
                if (types.Any(x => x.Name == proxyClassName))
                {
                    proxyType = types.First(o=>o.Name==proxyClassName);
                    break;
                }
            }
            object proxyInstance = proxyType.Assembly.CreateInstance(proxyType.FullName);

            return (TInterface)proxyInstance;
        }
    }
}
