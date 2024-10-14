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
    public static class EmitTest5
    {
        public static void Test()
        {
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Emit.Test"),AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("Emit.Test.Identity");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("UserInfo",TypeAttributes.Public|TypeAttributes.AutoClass|TypeAttributes.BeforeFieldInit);
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_name", typeof(string), FieldAttributes.Private);
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public,CallingConventions.Standard,new Type[] { typeof(string) });
            ILGenerator constructorIL = constructorBuilder.GetILGenerator();
            constructorIL.Emit(OpCodes.Ldarg_0);// this指针
            constructorIL.Emit(OpCodes.Ldstr,"Name:{0}");
            constructorIL.Emit(OpCodes.Ldarg_1);
            constructorIL.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine",new Type[] { typeof(string),typeof(object) }));
            constructorIL.Emit(OpCodes.Ldarg_1);
            constructorIL.Emit(OpCodes.Stfld,fieldBuilder);
            constructorIL.Emit(OpCodes.Ret);

            Type type = typeBuilder.CreateType();
            dynamic userInfo = Activator.CreateInstance(type,"Danny Hong");
        }
    }

    public class UserInfo
    {
        private string _name;
        public UserInfo(string name)
        {
            Console.WriteLine($"Name:{name}");            
            _name = name;
        }

        public void SayHello()
        {
            Console.WriteLine($"Hello {_name}");
        }
    }
}
