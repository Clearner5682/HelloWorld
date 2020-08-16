using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConsoleApp1
{
    //有1，2，2，3，4，5六个数，要求输出他们组合的全排列，4不能在第三位，5和3不能相邻
    // 全排列算法
    public static class PermTest
    {
        public static IList<string> charList = new List<string>();

        public static void Perm(this char[] array, string indexString,string sortedString)
        {
            for(int i = 0; i < array.Length; i++)
            {
                if (indexString.IndexOf(i.ToString())<0)
                {
                    string tempString = sortedString + array[i];
                    if (tempString.Length == array.Length)
                    {
                        if (tempString.IndexOf('4') != 2 && !tempString.Contains("53") && !tempString.Contains("35")&&!charList.Any(o => o == tempString))
                        {
                            Console.WriteLine($"{tempString}");
                            charList.Add(tempString);
                        }
                        break;
                    }
                    array.Perm(indexString+i.ToString(), tempString);
                }
            }
        }
    }
}
