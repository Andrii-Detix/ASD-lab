namespace Drawing.classes
{
    internal static class HelpMethods
    {
        /// <summary>
        /// Розбиває число на масив з цифр, з яких складається число. Під індексом 0 буде початкове число
        /// </summary>
        /// <param name="value">число для розбивання на цифри</param>
        /// <returns></returns>
        public static int[] GetNumbers(int value)
        {
            int elementsNum = 0;
            int a = value;
            while (a != 0)
            {
                elementsNum++;
                a /= 10;
            }

            int[] numbers = new int[elementsNum + 1];
            numbers[0] = value;
            for (int i = elementsNum; i > 0; i--)
            {
                numbers[i] = value % 10;
                value /= 10;
            }

            return numbers;
        }

        /// <summary>
        /// Перевіряє чи два числа знаходяться по одну сторону від крайнього
        /// </summary>
        /// <param name="a">перше число</param>
        /// <param name="b">друге число</param>
        /// <param name="ender">крайнє число</param>
        /// <returns></returns>
        public static bool CheckSamePos(float a, float b, float ender)
        {
            return (b <= ender) && (a <= ender) || (b >= ender) && (a >= ender);
        }

        /// <summary>
        /// Перевіряє чи число не вийшло за межі. Якщо вийшло, дає йому максимальне значення, або 0
        /// </summary>
        /// <param name="num">число</param>
        /// <param name="max">максимальне значення</param>
        /// <returns></returns>
        public static int CheckIndexLim(int num, int max)
        {
            if (num < 0)
            {
                num = max;
            }
            else if (num > max)
            {
                num = 0;
            }

            return num;
        }
    }
}