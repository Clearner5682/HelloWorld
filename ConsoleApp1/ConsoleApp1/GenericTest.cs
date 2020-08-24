using ConsoleApp1.Models;
using Newtonsoft.Json;
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

    public class GenericTest
    {
        public static void Test()
        {
            //IResp<dynamic> resp = new Resp<dynamic>();
            //resp.Data = new { UserId = "1111", UserName = "hongyan", Age = 18 };

            //Console.WriteLine(JsonConvert.SerializeObject(resp));

            // MakeGenericType
            Type genericType = typeof(Resp<>).MakeGenericType(typeof(UserInfo));
            PropertyInfo dataProperty = genericType.GetProperty("Data");
            object resp = genericType.Assembly.CreateInstance(genericType.FullName);
            //ConstructorInfo[] constructorInfos = genericType.GetConstructors();
            //object resp2 = constructorInfos[0].Invoke(null);
            dataProperty.SetValue(resp, new UserInfo { UserId = "1111", UserName = "hongyan", Age = 18 });
            Console.WriteLine(JsonConvert.SerializeObject(resp));

            // MakeGenericMethod
            MethodInfo setDataMethod = typeof(RespExtension).GetMethod("SetData");
            // 注意这里参数是泛型类型，如果有多个泛型类型，则传多个Type
            MethodInfo genericMethod = setDataMethod.MakeGenericMethod(typeof(UserInfo));
            genericMethod.Invoke(null, new object[] { resp, new UserInfo { UserId = "2222", UserName = "liyang", Age = 30 } });
            Console.WriteLine(JsonConvert.SerializeObject(resp));

            // Sort IEnumerable
            List<UserInfo> userInfoList = new List<UserInfo>
            {
                new UserInfo { UserId="1111",UserName="hongyan",Age=18},
                new UserInfo{ UserId="2222",UserName="liyang",Age=30},
                new UserInfo{ UserId="3333",UserName="wulinhao",Age=28}
            };
            var ageAscendingList = userInfoList.OrderBy("Age", true).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(ageAscendingList));
            var ageDescendingList = userInfoList.OrderBy("Age", false).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(ageDescendingList));
        }
    }
}
