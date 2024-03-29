using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.classes
{
    internal static class HelpMethods
    {
        static public int[] GetNumbers(int value)
        {
            int elementsNum = 0;
            int a = value;
            while (a != 0)
            {
                elementsNum++;
                a /= 10;
            }
            int[] numbers = new int[elementsNum+1];
            numbers[0] = value;
            for (int i = elementsNum; i > 0; i--)
            {
                numbers[i] = value % 10;
                value /= 10;
            }
            return numbers;
        }
        static public bool CheckDifPos(float a, float b, float middle)
        {
            return (b>middle) && (a<middle) || (b<middle)&&(a>middle);
        }
    }
}
