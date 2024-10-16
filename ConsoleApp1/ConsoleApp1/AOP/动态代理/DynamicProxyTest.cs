using ConsoleApp1.AOP.动态代理;
using ConsoleApp1.DependencyInject;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.AOP
{
    public class DynamicProxyTest
    {
        public static void Test(IServiceCollection serviceCollection)
        {
            // <特性,拦截器>
            Dictionary<Type,Type> dicInterceptor = new Dictionary<Type, Type>();

            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
            {
                Type [] types = assembly.GetTypes();
                foreach(Type type in types)
                {
                    if(type.IsClass && type.GetInterfaces().Any(o=>typeof(IInterceptor).IsAssignableFrom(o)))
                    {
                        Type interfaceType = type.GetInterfaces().First(o => o.IsGenericType);
                        Type genericType = interfaceType.GetGenericArguments().First();
                        if (!dicInterceptor.ContainsKey(genericType))
                        {
                            dicInterceptor.Add(genericType, type);
                        }
                    }
                }
            });


            ServiceLocator.Instance.OnRegistered += (serviceType, implementType) =>
            {
                Dictionary<Type,Type> interceptorsForService = new Dictionary<Type, Type>();
                var attributes = implementType.GetCustomAttributes();
                foreach (var attribute in attributes)
                {
                    Type attributeType = attribute.GetType();
                    if (dicInterceptor.ContainsKey(attributeType))
                    {
                        // 包含拦截器特性
                        interceptorsForService.Add(attributeType,dicInterceptor[attributeType]);
                    }
                }

                if(interceptorsForService.Count>0)
                {
                    // 创建代理类，并注入到容器
                    Type proxyType = CreateProxyType(serviceType,implementType, interceptorsForService);
                    var instance = Activator.CreateInstance(proxyType);
                    serviceCollection.AddSingleton(serviceType, instance);
                }
            };
        }

        private static Type CreateProxyType(Type serviceType,Type implementType,Dictionary<Type,Type> interceptors)
        {
            // 创建代理类
            // 假设只有一个拦截器
            Type interceptorType = interceptors.First().Value;
            Type attributeType = interceptors.First().Key;

            string assemblyName = implementType.Name + ".Proxies";
            string typeName = implementType.Name + "Proxy";

            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);
            typeBuilder.SetParent(implementType);
            typeBuilder.AddInterfaceImplementation(serviceType);
            MethodInfo[] methodInfos = implementType.GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly);
            foreach(MethodInfo methodInfo in methodInfos)
            {
                if(methodInfo.IsVirtual)// 只有虚方法才能被重写
                {
                    MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.NewSlot, methodInfo.ReturnType, methodInfo.GetParameters().Select(o => o.ParameterType).ToArray());
                    ILGenerator ilGenerator = methodBuilder.GetILGenerator();
                    ilGenerator.Emit(OpCodes.Newobj, interceptorType.GetConstructor(Type.EmptyTypes));
                    ilGenerator.Emit(OpCodes.Dup);
                    ilGenerator.Emit(OpCodes.Call, interceptorType.GetMethod("Before"));
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Call, implementType.GetMethod(methodInfo.Name, Type.EmptyTypes));
                    ilGenerator.Emit(OpCodes.Call, interceptorType.GetMethod("After"));
                    ilGenerator.Emit(OpCodes.Ret);
                    typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
                }
            }

            Type proxyType = typeBuilder.CreateType();

            return proxyType;
        }
    }
}
