using ConsoleApp1.AOP.动态代理;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.反射.Emit
{
    public class EmitTest6
    {
        public static void Test()
        {
            Log();
        }

        private static void GetList()
        {
            // 通过Emit生成下面的方法
            //public List<string> GetList()
            //{
            //    var list = new List<string>();
            //    list.Add("hello");
            //    list.Add("world");

            //    return list;
            //}

            DynamicMethod dynamicMethod = new DynamicMethod("GetList", typeof(List<string>), Type.EmptyTypes);
            ILGenerator il = dynamicMethod.GetILGenerator();
            Type genericListType = typeof(List<>).MakeGenericType(typeof(string));
            il.Emit(OpCodes.Newobj, typeof(List<string>).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldstr, "hello");
            il.Emit(OpCodes.Call, genericListType.GetMethod("Add"));
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldstr, "world");
            il.Emit(OpCodes.Call, genericListType.GetMethod("Add"));
            il.Emit(OpCodes.Ret);

            Delegate del = dynamicMethod.CreateDelegate(typeof(Func<List<string>>));
            var list = del.DynamicInvoke();
        }

        private static void Log()
        {
            DynamicMethod dynamicMethod = new DynamicMethod("Log", null, Type.EmptyTypes);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, typeof(LogInterceptor).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Call, typeof(LogInterceptor).GetMethod("Before"));
            il.Emit(OpCodes.Call, typeof(LogInterceptor).GetMethod("After"));
            il.Emit(OpCodes.Ret);

            Delegate del = dynamicMethod.CreateDelegate(typeof(Action));
            del.DynamicInvoke();
        }
    }
}
