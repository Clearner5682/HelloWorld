using ConsoleApp1.Models;
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

            List<UserInfo> userList = new List<UserInfo> { new UserInfo { UserId="id1111",UserName="name1111",Age=18 }, new UserInfo { UserId = "id2222", UserName = "name2222", Age = 18 } };
            SetAllProperty(userList, "UserName", "danny_hong");
        }

        public static void SetAllProperty<T>(List<T> list,string property,object propertyValue)
        {
            // o.Author="Ninputer"
            PropertyInfo propertyInfo = typeof(T).GetProperty(property);
            MethodInfo setMethod = propertyInfo.SetMethod;

            ParameterExpression parameter = Expression.Parameter(typeof(object), "o");
            var convertExp = Expression.Convert(parameter, propertyInfo.PropertyType);
            
            ParameterExpression instance = Expression.Parameter(typeof(T),"x");
            MethodCallExpression callExpression = Expression.Call(instance, setMethod, convertExp);
            Console.WriteLine(callExpression.ToString());
            LambdaExpression lambdaExpression = Expression.Lambda<Action<T,object>>(callExpression, instance, parameter);
            Delegate del = lambdaExpression.Compile();

            

            Console.WriteLine(lambdaExpression.ToString());

            foreach (var item in list)
            {
                del.DynamicInvoke(item, propertyValue);
            }
        }
    }
}
