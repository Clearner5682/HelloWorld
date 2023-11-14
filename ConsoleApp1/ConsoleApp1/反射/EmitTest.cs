using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Linq.Expressions;

namespace ConsoleApp1.反射
{
    public class EmitTest
    {
        public static void Test()
        {
            // 利用Emit生成方法，并调用
            DynamicMethod method = new DynamicMethod("SayHello", null, Type.EmptyTypes);

            // 生成IL代码
            ILGenerator ilGenerator=method.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ldstr, "Hello World by Emit");
            ilGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ret);

            // 根据DynamicMethod创建委托
            var sayHelloDel = method.CreateDelegate(typeof(Action)) as Action;
            sayHelloDel.Invoke();

            var value = Expression.Constant("Hello World by Expression");
            MethodCallExpression callConsole = Expression.Call(null, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),value);
            LambdaExpression lambda = Expression.Lambda(callConsole);
            Action action = lambda.Compile() as Action;
            action.Invoke();
        }
    }
}
