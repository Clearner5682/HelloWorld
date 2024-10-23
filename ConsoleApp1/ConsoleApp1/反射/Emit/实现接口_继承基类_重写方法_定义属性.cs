using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;

namespace ConsoleApp1.反射.Emit
{
    // 通过Emit生成程序集、模块、类、字段、属性、方法
    public class EmitTest2
    {
        public static void Test()
        {
            Type baseClassType = typeof(Vehicle);
            Type interfaceType = typeof(IVehicle);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DannyHong.Test.Emit"), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DannyHong.Test.Emit");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("Car", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);
            typeBuilder.SetParent(baseClassType);//设置基类
            typeBuilder.AddInterfaceImplementation(interfaceType);//实现接口
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_name", typeof(string), FieldAttributes.Private);//定义字段


            //定义构造函数
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(string) });
            ILGenerator ctorIL = constructorBuilder.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);// Ldarg_0在实例方法中表示this，在静态方法中表示第一个参数
            ctorIL.Emit(OpCodes.Ldarg_1);// Ldarg_1表示第一个参数
            ctorIL.Emit(OpCodes.Stfld,fieldBuilder);//设置字段的值
            ctorIL.Emit(OpCodes.Ret);//返回


            //定义属性
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty("Name", PropertyAttributes.None,CallingConventions.Standard, typeof(string),Type.EmptyTypes);
            //定义属性的get方法
            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_Name", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.HideBySig, typeof(string), Type.EmptyTypes);
            ILGenerator getIL= getMethodBuilder.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(getMethodBuilder,interfaceType.GetProperty("Name").GetGetMethod());//对接口方法的重写
            propertyBuilder.SetGetMethod(getMethodBuilder);
            //定义属性的set方法
            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_Name", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new Type[] { typeof(string) });
            ILGenerator setIL = setMethodBuilder.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, fieldBuilder);
            setIL.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(setMethodBuilder, interfaceType.GetProperty("Name").GetSetMethod());//对接口方法的重写
            propertyBuilder.SetSetMethod(setMethodBuilder);


            //定义方法
            MethodBuilder runMethodBuilder = typeBuilder.DefineMethod("Run", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual ,CallingConventions.Standard, null, Type.EmptyTypes);
            ILGenerator runIL = runMethodBuilder.GetILGenerator();
            runIL.Emit(OpCodes.Ldarg_0);
            //runIL.Emit(OpCodes.Ldfld,fieldBuilder);
            //或者
            //runIL.Emit(OpCodes.Callvirt, interfaceType.GetProperty("Name").GetGetMethod());
            //或者
            runIL.Emit(OpCodes.Call,propertyBuilder.GetMethod);
            runIL.Emit(OpCodes.Ldstr, " is running");
            runIL.Emit(OpCodes.Call, typeof(string).GetMethod("Concat",new Type[] { typeof(string),typeof(string) }));
            runIL.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine",new Type[] { typeof(string) }));
            runIL.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(runMethodBuilder,baseClassType.GetMethod("Run"));//对基类方法的重写


            //创建类
            Type type = typeBuilder.CreateType();

            EmitTypeList.AddType(type);


            //调用
            object obj = Activator.CreateInstance(type, "Porsche");
            (obj as IVehicle).Run();
        }
    }

    public interface IVehicle
    {
        string Name { get; set; }
        void Run();
    }

    public abstract class Vehicle : IVehicle
    {
        public abstract string Name { get; set; }
        public abstract void Run();

    }

    // 用Emit实现如下的类：
    public class Car : Vehicle, IVehicle
    {
        private string _name;
        public override string Name 
        { 
            get { return _name; } 
            set { _name = value; } 
        }

        public Car(string name)
        {
            this._name = name;
        }

        public override void Run()
        {
            Console.WriteLine($"{this.Name} is running");
        }
    }
}
