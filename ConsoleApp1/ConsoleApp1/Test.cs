using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public static class Test
    {
        public static void TestMethod()
        {
            string[] array1 = new string[] { "a", "f", "d", "c" };
            FastSort<string>(array1, 0, array1.Length-1);
            int[] array2 = new int[] { 1,2,2,3,4,5};
            Perm(array2, "", "");
            Console.WriteLine(list.Count);
        }

        // 快速排序
        public static void FastSort<T>(T[] array,int left,int right) where T:IComparable
        {
            int leftBase = left;
            int rightBase = right;
            T compareBase = array[leftBase];
            string direction = "Right_To_Left";
            while (true)
            {
                if (leftBase >= rightBase)// 说明比基准值小的都到了左边，比基准值大的都到了右边
                {
                    break;
                }
                if (direction == "Right_To_Left")
                {
                    if (array[rightBase].CompareTo(compareBase)<0)// 右边基准值要小，调换位置，改变比较方向
                    {
                        array[leftBase] = array[rightBase];
                        array[rightBase] = compareBase;
                        direction = "Left_To_Right";
                        continue;
                    }
                    else
                    {
                        rightBase--;
                    }
                }
                if (direction == "Left_To_Right")
                {
                    if (array[leftBase].CompareTo(compareBase) >= 0)
                    {
                        array[rightBase] = array[leftBase];
                        array[leftBase] = compareBase;
                        direction = "Right_To_Left";
                        continue;
                    }
                    else
                    {
                        leftBase++;
                    }
                }
            }
            if (leftBase-1 > left)
            {
                FastSort(array, left, leftBase - 1);
            }
            if (right > rightBase + 1)
            {
                FastSort(array, rightBase + 1, right);
            }
        }

        // 122345
        // 求全排列
        // 3和5不能相邻，4不能在第三位
        public static IList<string> list = new List<string>();
        public static void Perm(int[]array,string indexString,string resultString)
        {
            for(int i = 0; i < array.Length; i++)
            {
                if (indexString.Contains(i.ToString()))
                {
                    continue;
                }
                var tempIndex = indexString + i.ToString();
                var tempResult = resultString + array[i].ToString();
                if (tempResult.Contains("35") || tempResult.Contains("53") || tempResult.IndexOf("4") == 2 || list.Contains(tempResult))
                {
                    continue;
                }
                if (tempResult.Length == array.Length)
                {
                    list.Add(tempResult);
                    Console.WriteLine(tempResult);
                    break;
                }
                Perm(array, tempIndex, tempResult);
            }
        }
    }
}
