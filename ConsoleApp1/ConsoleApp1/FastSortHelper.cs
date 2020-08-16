using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public static class FastSortHelper
    {
        public static void FastSort<T>(this T[] array, int left, int right) where T : IComparable
        {
            T compareBase = array[left];
            int leftBase = left;
            int rightBase = right;
            string direction = "Right_To_Left";
            while (true)
            {
                if (left >= right)
                {
                    break;
                }
                if (direction == "Right_To_Left")
                {
                    if (compareBase.CompareTo(array[right]) > 0)
                    {
                        array[left] = array[right];
                        array[right] = compareBase;
                        direction = "Left_To_Right";
                        continue;
                    }
                    else
                    {
                        right--;
                    }
                }
                if (direction == "Left_To_Right")
                {
                    if (compareBase.CompareTo(array[left]) <= 0)
                    {
                        array[right] = array[left];
                        array[left] = compareBase;
                        direction = "Right_To_Left";
                        continue;
                    }
                    else
                    {
                        left++;
                    }
                }
            }
            if (left - 1 > leftBase)
            {
                FastSort(array, leftBase, left - 1);
            }
            if (rightBase > right + 1)
            {
                FastSort(array, right + 1, rightBase);
            }
        }
    }
}
