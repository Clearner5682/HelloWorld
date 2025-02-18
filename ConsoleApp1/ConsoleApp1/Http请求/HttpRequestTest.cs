using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
//using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ConsoleApp1.Http请求
{
    public static class HttpRequestTest
    {
        public static void Test()
        {
            string requestUrl = "https://aiocloud.hoplun.com:8100/api/auth/getpublickey";

            // 创建自定义的 HttpClientHandler
            var httpClientHandler = new HttpClientHandler();
            // 设置自定义的证书验证回调，始终返回 true 以忽略证书校验
            httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            HttpClient client = new HttpClient(httpClientHandler);
            HttpResponseMessage response = client.GetAsync(requestUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = response.Content.ReadAsStringAsync().Result;


                #region System.Text.Json

                //dynamic obj = System.Text.Json.JsonSerializer.Deserialize<dynamic>(jsonStr);
               
                //if (obj is JsonElement jsonElement)
                //{
                //    if (jsonElement.ValueKind == JsonValueKind.Object)
                //    {
                //        foreach (var property in jsonElement.EnumerateObject())
                //        {
                //            Console.WriteLine($"属性名: {property.Name}, 属性值: {property.Value}");
                //        }
                //    }
                //    else if (jsonElement.ValueKind == JsonValueKind.Array)
                //    {
                //        int index = 0;
                //        foreach (var item in jsonElement.EnumerateArray())
                //        {
                //            Console.WriteLine($"数组元素 {index++}:");
                //            if (item.ValueKind == JsonValueKind.Object)
                //            {
                //                foreach (var property in item.EnumerateObject())
                //                {
                //                    Console.WriteLine($"  属性名: {property.Name}, 属性值: {property.Value}");
                //                }
                //            }
                //        }
                //    }
                //}

                #endregion

                #region NewtonSoft.Json

                dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonStr);

                if (obj is Newtonsoft.Json.Linq.JObject jObject)
                {
                    foreach (var property in jObject)
                    {
                        Console.WriteLine($"属性名：{property.Key}, 属性值：{property.Value}, 属性类型：{property.Value.Type}:");
                    }
                }
                else if (obj is Newtonsoft.Json.Linq.JArray jArray)
                {
                    int index = 0;
                    foreach (var item in jArray)
                    {
                        Console.WriteLine($"数组元素 {index++}:");
                    }
                }
                else
                {
                    Console.WriteLine($"{obj}");
                }

                #endregion
            }
        }
    }
}
