using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConsoleApp1.表达式树
{
    public static class ExpressionTreeTest11
    {
        public static void Test()
        {
            //① -a
            //② a+b*2
            //③ Math.Sin(x) + Math.Cos(y)
            //④ new StringBuilder("Hello")
            //⑤ new int[]{a,b,a+b}
            //⑥ a[i-1]*i
            //⑦ a.Length>b|b>=0  a为String类型
            //⑧ new System.Drawing.Point(a,b)
            //⑨ new UserInfo("Danny Hong").SayHello();
            Test1(100);
            Test2(50, 100);
            Test3(30, 60);
            Test4();
            Test5(10, 20);
            Test6(new int[] { 10, 20, 30 }, 2);
            Test7("abcdef", 8);
            Test8(40, 50);
            Test9();
        }

        private static void Test1(object arg)
        {
            ParameterExpression a = Expression.Parameter(typeof(int), "a");
            ConstantExpression constant1 = Expression.Constant(-1);
            BinaryExpression multiply = Expression.Multiply(constant1, a);
            LambdaExpression lambda = Expression.Lambda(multiply, a);
            Delegate del = lambda.Compile();
            Console.WriteLine($"Expression1:{lambda.ToString()}");
            Console.WriteLine($"Result1:{del.DynamicInvoke(arg)}");
        }

        private static void Test2(object arg1,object arg2)
        {
            ParameterExpression a = Expression.Parameter(typeof(int), "a");
            ParameterExpression b = Expression.Parameter(typeof(int), "b");
            ConstantExpression constant2 = Expression.Constant(2);
            BinaryExpression multiply = Expression.Multiply(constant2, b);
            BinaryExpression add = Expression.Add(a, multiply);
            LambdaExpression lambda = Expression.Lambda(add, a,b);
            Delegate del = lambda.Compile();
            Console.WriteLine($"Expression2:{lambda.ToString()}");
            Console.WriteLine($"Result2:{del.DynamicInvoke(arg1,arg2)}");
        }

        private static void Test3(object arg1, object arg2)
        {
            ParameterExpression x = Expression.Parameter(typeof(double), "x");
            ParameterExpression y = Expression.Parameter(typeof(double), "y");
            MethodCallExpression sin = Expression.Call(typeof(Math).GetMethod("Sin"),x);
            MethodCallExpression cos = Expression.Call(typeof(Math).GetMethod("Cos"), y);
            BinaryExpression add = Expression.Add(sin, cos);
            LambdaExpression lambda = Expression.Lambda(add, x,y);
            Delegate del = lambda.Compile();
            Console.WriteLine($"Expression3:{lambda.ToString()}");
            Console.WriteLine($"Result3:{del.DynamicInvoke(arg1, arg2)}");
        }

        private static void Test4()
        {
            ConstantExpression constantHello = Expression.Constant("Hello");
            NewExpression newExpression = Expression.New(typeof(StringBuilder).GetConstructor(new Type[] { typeof(string) }),constantHello);
            LambdaExpression lambda = Expression.Lambda(newExpression);
            Delegate del = lambda.Compile();
            Console.WriteLine($"Expression4:{lambda.ToString()}");
            Console.WriteLine($"Result4:{del.DynamicInvoke()}");
        }

        private static void Test5(object arg1, object arg2)
        {
            ParameterExpression a = Expression.Parameter(typeof(int), "a");
            ParameterExpression b = Expression.Parameter(typeof(int), "b");
            NewArrayExpression newExpression = Expression.NewArrayInit(typeof(int), new Expression[] { a, b, Expression.Add(a,b) });
            LambdaExpression lambda = Expression.Lambda(newExpression,a,b);
            Delegate del = lambda.Compile();
            Console.WriteLine($"Expression5:{lambda.ToString()}");
            int[] result5 = (int[])del.DynamicInvoke(arg1, arg2);
            Console.WriteLine($"Result5:[{string.Join(',',result5.Select(o=>o.ToString()))}]");
        }

        private static void Test6(object arg1,object arg2)
        {
            ParameterExpression a = Expression.Parameter(typeof(int[]), "a");
            ParameterExpression i = Expression.Parameter(typeof(int), "i");
            IndexExpression arrayAccess = Expression.ArrayAccess(a, Expression.Subtract(i, Expression.Constant(1)));
            LambdaExpression lambda = Expression.Lambda(Expression.Multiply(arrayAccess,i), a,i);
            Delegate del = lambda.Compile();
            Console.WriteLine($"Expression6:{lambda.ToString()}");
            Console.WriteLine($"Result6:{del.DynamicInvoke(arg1,arg2)}");
        }

        private static void Test7(object arg1, object arg2)
        {
            ParameterExpression a = Expression.Parameter(typeof(string), "a");
            ParameterExpression b = Expression.Parameter(typeof(int), "b");
            MemberExpression lengthExp = Expression.Property(a, "Length");
            BinaryExpression greaterThan1 = Expression.GreaterThan(lengthExp, b);
            BinaryExpression greaterThan2 = Expression.GreaterThanOrEqual(b,Expression.Constant(0));
            BinaryExpression orExp = Expression.Or(greaterThan1, greaterThan2);
            LambdaExpression lambda = Expression.Lambda(orExp,a,b);
            Delegate del = lambda.Compile();
            Console.WriteLine($"Expression7:{lambda.ToString()}");
            Console.WriteLine($"Result7:{del.DynamicInvoke(arg1, arg2)}");
        }

        private static void Test8(object arg1,object arg2)
        {
            ParameterExpression a = Expression.Parameter(typeof(int), "a");
            ParameterExpression b = Expression.Parameter(typeof(int), "b");
            NewExpression newExp = Expression.New(typeof(Point).GetConstructor(new Type[] { typeof(int),typeof(int) }), a, b);
            LambdaExpression lambda = Expression.Lambda(newExp, a, b);
            Delegate del = lambda.Compile();
            Console.WriteLine($"Expression8:{lambda.ToString()}");
            Console.WriteLine($"Result8:{del.DynamicInvoke(arg1, arg2)}");
        }

        private static void Test9()
        {
            ParameterExpression name = Expression.Parameter(typeof(string), "name");
            NewExpression constructor = Expression.New(typeof(UserInfo).GetConstructor(new Type[] { typeof(string) }),name);
            LambdaExpression constructorLambda = Expression.Lambda(constructor, name);
            Delegate constructorDel = constructorLambda.Compile();
            object obj = constructorDel.DynamicInvoke("Danny Hong");

            // var test = obj;
            ConstantExpression objConstant = Expression.Constant(obj);
            MethodCallExpression sayHello = Expression.Call(objConstant, typeof(UserInfo).GetMethod("SayHello"));
            LambdaExpression sayHelloLambda = Expression.Lambda(sayHello);
            Delegate sayHelloDel = sayHelloLambda.Compile();
            sayHelloDel.DynamicInvoke();
        }

        private class UserInfo
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
}
