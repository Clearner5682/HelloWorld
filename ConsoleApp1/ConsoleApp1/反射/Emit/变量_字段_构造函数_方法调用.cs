using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace ConsoleApp1.反射.Emit
{
    //public class UserInfo
    //{
    //    private string _name;
    //    public UserInfo(string name)
    //    {
    //        Console.WriteLine($"Name:{name}");
    //        _name = name;
    //    }

    //    public void SayHello()
    //    {
    //        Console.WriteLine($"Hello: {_name}");
    //    }

    //    public static void Run(string name)
    //    {
    //        string str = $"{name} Run";
    //        Console.WriteLine(name);
    //    }
    //}

    public static class EmitTest5
    {
        // 使用Emit生成上面的UserInfo类
        // 包含构造函数、字段、实例方法、静态方法
        public static void Test()
        {
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Emit.Test"),AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("Emit.Test.Identity");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("UserInfo",TypeAttributes.Public|TypeAttributes.AutoClass|TypeAttributes.BeforeFieldInit);
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_name", typeof(string), FieldAttributes.Private);
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public,CallingConventions.Standard,new Type[] { typeof(string) });
            ILGenerator ilGenerator = constructorBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);// this指针，静态方法中表示第一个参数，非静态方法中表示this
            ilGenerator.Emit(OpCodes.Ldstr,"Name: {0}");
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine",new Type[] { typeof(string),typeof(object) }));
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Stfld,fieldBuilder);
            ilGenerator.Emit(OpCodes.Ret);
            MethodBuilder instanceMethodBuilder = typeBuilder.DefineMethod("SayHello", MethodAttributes.Public,null,Type.EmptyTypes);
            ilGenerator = instanceMethodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldstr,"Hello: {0}");
            ilGenerator.Emit(OpCodes.Ldarg_0);// this指针，加上下面的Ldfld指令，就是this._name
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Call, typeof(string).GetMethod("Format", new Type[] { typeof(string),typeof(object) }));
            ilGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            ilGenerator.Emit(OpCodes.Ret);
            MethodBuilder staticMethodBuilder = typeBuilder.DefineMethod("Run", MethodAttributes.Public|MethodAttributes.Static,null,new Type[] { typeof(string) });
            ilGenerator = staticMethodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldstr,"{0} Run");
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Call, typeof(string).GetMethod("Format", new Type[] { typeof(string),typeof(object) }));
            ilGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            ilGenerator.Emit(OpCodes.Ret);

            Type type = typeBuilder.CreateType();
            //var userInfo = Activator.CreateInstance(type,"Danny Hong");
            //type.GetMethod("SayHello").Invoke(userInfo,null);
            dynamic userInfo = Activator.CreateInstance(type, "Danny Hong");
            userInfo.SayHello();
            type.GetMethod("Run").Invoke(null,new object[] { "Danny Hong" });
        }
    }
}
