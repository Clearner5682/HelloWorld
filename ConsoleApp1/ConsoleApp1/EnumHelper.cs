using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ConsoleApp1
{
    public static class EnumHelper
    {
        public static string GetDescription<EnumType>(this EnumType enumValue) where EnumType : Enum
        {
            Type type = typeof(EnumType);
            string name = Enum.GetName(type, enumValue);
            FieldInfo fieldInfo = type.GetField(name);
            var attributes= fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute));
            foreach(var item in attributes)
            {
                if(item is DescriptionAttribute)
                {
                    return (item as DescriptionAttribute).Description;
                }
            }

            return default;
        }
    }

    public enum RecommendType
    {
        [Description("推荐")]
        Recommend,
        [Description("流行")]
        Popular
    }
}
