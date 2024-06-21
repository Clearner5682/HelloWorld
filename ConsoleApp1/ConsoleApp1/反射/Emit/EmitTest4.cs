using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.反射.Emit
{
    // 通过Emit设置属性
    public class EmitTest4
    {
        public static void Test()
        {
            // 创建程序集
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DannyHong.Test.Emit"), AssemblyBuilderAccess.Run);
            // 创建模块
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DannyHong.Test.Emit");
            // 创建类型
            TypeBuilder typeBuilder = moduleBuilder.DefineType("Blog", TypeAttributes.Public|TypeAttributes.AutoClass|TypeAttributes.BeforeFieldInit);
            
            // 创建字段
            FieldBuilder titleField = typeBuilder.DefineField("_title", typeof(string), FieldAttributes.Private);
            FieldBuilder contentField = typeBuilder.DefineField("_content", typeof(string), FieldAttributes.Private);
            // 创建属性
            PropertyBuilder title = typeBuilder.DefineProperty("Title", PropertyAttributes.None, typeof(string), Type.EmptyTypes);
            MethodBuilder titleGet = typeBuilder.DefineMethod("get_Title", MethodAttributes.Public|MethodAttributes.SpecialName|MethodAttributes.HideBySig, typeof(string), Type.EmptyTypes);
            ILGenerator titleGetIL = titleGet.GetILGenerator();
            titleGetIL.Emit(OpCodes.Ldarg_0);
            titleGetIL.Emit(OpCodes.Ldfld, titleField);
            titleGetIL.Emit(OpCodes.Ret);
            MethodBuilder titleSet = typeBuilder.DefineMethod("set_Title", MethodAttributes.Public|MethodAttributes.SpecialName|MethodAttributes.HideBySig, null, new Type[] { typeof(string) });
            ILGenerator titleSetIL= titleSet.GetILGenerator();
            titleSetIL.Emit(OpCodes.Ldarg_0);
            titleSetIL.Emit(OpCodes.Ldarg_1);
            titleSetIL.Emit(OpCodes.Stfld, titleField);
            titleSetIL.Emit(OpCodes.Ret);
            title.SetGetMethod(titleGet);
            title.SetSetMethod(titleSet);

            PropertyBuilder content = typeBuilder.DefineProperty("Content", PropertyAttributes.None, typeof(string), Type.EmptyTypes);
            MethodBuilder contentGet = typeBuilder.DefineMethod("get_Content", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(string), Type.EmptyTypes);
            ILGenerator contentGetIL = contentGet.GetILGenerator();
            contentGetIL.Emit(OpCodes.Ldarg_0);
            contentGetIL.Emit(OpCodes.Ldfld, contentField);
            contentGetIL.Emit(OpCodes.Ret);
            MethodBuilder contentSet = typeBuilder.DefineMethod("set_Content", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { typeof(string) });
            ILGenerator contentSetIL = contentSet.GetILGenerator();
            contentSetIL.Emit(OpCodes.Ldarg_0);
            contentSetIL.Emit(OpCodes.Ldarg_1);
            contentSetIL.Emit(OpCodes.Stfld, contentField);
            contentSetIL.Emit(OpCodes.Ret);
            content.SetGetMethod(contentGet);
            content.SetSetMethod(contentSet);

            Type type = typeBuilder.CreateType();
            dynamic blog = Activator.CreateInstance(type);
            blog.Title = "This is the Title";
            blog.Content = "This is the Content";
            var test = blog.Title;
        }
    }

    // 用Emit创建下面的类型
    //public class Blog
    //{
    //    public string Title { get; set; }
    //    public string Content { get; set; }
    //}
}
