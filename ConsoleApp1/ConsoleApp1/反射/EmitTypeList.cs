using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.反射
{
    public class EmitTypeList
    {
        private static List<Type> _list = new List<Type>();

        public static void AddType(Type type)
        {
            if(!_list.Contains(type))
            {
                _list.Add(type);
            }
        }

        public static void RemoveType(Type type)
        {
            _list.Remove(type);
        }

        public static Type GetType(string typeName)
        {
            return _list.FirstOrDefault(t => t.Name==typeName);
        }
    }
}
