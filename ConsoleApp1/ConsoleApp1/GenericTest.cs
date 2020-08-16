using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    public interface IResp<T>
    {
        int ErrorCode { get; set; }
        string Message { get; set; }
        T Data { get; }
    }

    public class Resp<T> : IResp<T>
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public T Data { get; private set; }
    }

    public static class RespExtension
    {
        public static void SetData<T>(this IResp<T> resp,object data)
        {
            Type type = resp.GetType();
            PropertyInfo propertyInfo = type.GetProperty("Data");
            propertyInfo.SetValue(resp, data);
        }
    }

    public static class IEnumerableExtension
    {
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source,string sortField,bool isAscending)
        {
            MethodInfo[] methods = typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static);
            string methodName = "OrderBy";
            if (!isAscending)
            {
                methodName = "OrderByDescending";
            }
            MethodInfo sortMethod = methods.FirstOrDefault(o=>o.IsPublic&&o.IsStatic&&o.Name==methodName&&o.GetParameters().Length==2);
            // 注意这里参数是泛型类型，如果有多个泛型类型，则传多个Type
            MethodInfo genericMethod = sortMethod.MakeGenericMethod(new Type[] {typeof(T),typeof(T).GetProperty(sortField).PropertyType });
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, typeof(T).GetProperty(sortField).GetMethod);
            LambdaExpression lambda = Expression.Lambda(property, parameter);
            Delegate del = lambda.Compile();
            var age = del.DynamicInvoke(new UserInfo { Age = 100 });
            
            // 注意这里要将Expression转化成委托，因为形参是Func<T,TKey>，就是一个委托
            return (IOrderedEnumerable<T>)genericMethod.Invoke(null, new object[] { source,del});
        }
    }
}
