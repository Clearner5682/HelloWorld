using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using Newtonsoft.Json;

namespace ConsoleApp1.反射.Emit
{
    // 通过Emit设置字段
    public class EmitTest3
    {
        public static void Test()
        {
            // 定义程序集、模块、类
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DannyHong.Test.Emit"), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DannyHong.Test.Emit");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("UserField", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);

            // 定义字段
            FieldBuilder tokenPrefix = typeBuilder.DefineField("TokenPrefix", typeof(string), FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly);
            FieldBuilder id = typeBuilder.DefineField("id", typeof(string), FieldAttributes.Public | FieldAttributes.InitOnly);
            FieldBuilder userName = typeBuilder.DefineField("userName", typeof(string), FieldAttributes.Public);
            FieldBuilder passwordHash = typeBuilder.DefineField("passwordHash", typeof(string), FieldAttributes.Private);

            // 定义静态构造函数
            ConstructorBuilder staticCtorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Static | MethodAttributes.Private | MethodAttributes.SpecialName | MethodAttributes.HideBySig, CallingConventions.Standard, Type.EmptyTypes);
            ILGenerator staticCtorIL = staticCtorBuilder.GetILGenerator();
            staticCtorIL.Emit(OpCodes.Ldstr, "Bearer");// Ldstr指令将字符串加载到堆栈上
            staticCtorIL.Emit(OpCodes.Stsfld, tokenPrefix);// Stsfld将值存储到静态字段中(store static field)
            staticCtorIL.Emit(OpCodes.Ret);
            // 定义实例构造函数
            ConstructorBuilder ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, CallingConventions.Standard, Type.EmptyTypes);
            ILGenerator ctorIL = ctorBuilder.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);// 实例方法的Ldarg_0表示this
            ctorIL.Emit(OpCodes.Ldstr, "123456");// Ldstr指令将字符串加载到堆栈上
            ctorIL.Emit(OpCodes.Stfld, passwordHash);// Stfld将值存储到字段中(store field)
            ctorIL.Emit(OpCodes.Ret);

            Type type = typeBuilder.CreateType();
            dynamic obj = Activator.CreateInstance(type);
            FieldInfo field_TokenPrefix = type.GetField("TokenPrefix",BindingFlags.Public|BindingFlags.Static);
            FieldInfo field_passwordHash = type.GetField("passwordHash", BindingFlags.NonPublic | BindingFlags.Instance);
            Console.WriteLine($"TokenPrefix:{field_passwordHash.GetValue(obj)},passwordHash:{field_passwordHash.GetValue(obj)}");
        }
    }

    // 使用Emit创建如下类：
    // 了解Emit处理字段的方法
    //public class UserField
    //{
    //    public static readonly string TokenPrefix = "Bearer";//静态字段的初始化其实就是在静态构造函数中赋值，我们看到的其实是语法糖
    //    public readonly string id;
    //    public string userName;
    //    private string passwordHash="123456";//实例字段的初始化其实是在构造函数中赋值，我们看到的其实是语法糖

    //    static UserField()
    //    {
    //        TokenPrefix = "Bearer";
    //    }

    //    public UserField()
    //    {
    //        passwordHash = "123456";
    //    }
    //}
}
