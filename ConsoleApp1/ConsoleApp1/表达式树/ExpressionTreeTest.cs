using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ConsoleApp1.表达式树
{
    public class ExpressionTreeTest
    {
        public static void Test()
        {
            var parameterExp = Expression.Parameter(typeof(int), "Amount");
            var constantExp = Expression.Constant(50000,typeof(int));
            BinaryExpression expression = Expression.GreaterThanOrEqual(parameterExp,constantExp);
            LambdaExpression lambdaExp=Expression.Lambda(expression,parameterExp);
            var result = (lambdaExp.Compile()).DynamicInvoke(100000);


            var parameterExp2 = Expression.Parameter(typeof(string), "Amount");
            MethodInfo methodInfo = typeof(int).GetMethod("Parse",new Type[] { typeof(string)});
            MethodCallExpression callExpression = Expression.Call(methodInfo, parameterExp2);
            LambdaExpression lambda2 = Expression.Lambda(callExpression, parameterExp2);
            var result2 = (lambda2.Compile()).DynamicInvoke("50000");

            var boolParam = Expression.Parameter(typeof(bool), "a");
            UnaryExpression isTrue = Expression.IsTrue(boolParam);
            LambdaExpression lambda3 = Expression.Lambda(isTrue, boolParam);
            //var result3 = (lambda3.Compile()).DynamicInvoke(false);
            
        }
    }
}
