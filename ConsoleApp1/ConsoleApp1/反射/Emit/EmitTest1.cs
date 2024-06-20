using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;

namespace ConsoleApp1.反射.Emit
{
    // 通过Emit生成方法
    public class EmitTest1
    {
        public static void Test()
        {
            DynamicMethod method = new DynamicMethod("Add", typeof(int), new Type[] { typeof(int),typeof(int) });
            ILGenerator ilGenerator = method.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Add);
            ilGenerator.Emit(OpCodes.Ret);

            Delegate del = method.CreateDelegate(typeof(Func<int,int,int>));
            object result = del.DynamicInvoke(100, 200);

            Console.WriteLine("Add Result:"+result);

            ParameterExpression paramX = Expression.Parameter(typeof(int));
            ParameterExpression paramY = Expression.Parameter(typeof(int));
            BinaryExpression add = Expression.Add(paramX, paramY);
            LambdaExpression lambda = Expression.Lambda(add, paramX, paramY);
            Delegate del1 = lambda.Compile();
            object result1 = del1.DynamicInvoke(200, 300);
            
            Console.WriteLine("Add Result1:"+result1);
        }

        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
