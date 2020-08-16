using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConsoleApp1.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
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
            var ageAscendingList = userInfoList.OrderBy("Age",true).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(ageAscendingList));
            var ageDescendingList = userInfoList.OrderBy("Age", false).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(ageDescendingList));

            Console.ReadLine();
        }
    }
}
