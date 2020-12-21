using System;

namespace NC
{
    public static class Border
    {
        public static void Drow(int left, int top, int widht, int height)
        {
            Console.SetCursorPosition(left, top);
            Console.Write('\u2554');
            for (int i = left + 1; i < widht - 1; i++)
                Console.Write('\u2550');
            Console.Write("\u2557\n");
            int j;
            for (j = top + 1; j < height - 1; j++)
            {
                Console.SetCursorPosition(left, j);
                Console.Write('\u2551');
                Console.SetCursorPosition(widht - 1, j);
                Console.Write("\u2551\n");
            }
            Console.SetCursorPosition(left, j);
            Console.Write('\u255A');
            for (int i = left + 1; i < widht - 1; i++)
                Console.Write('\u2550');
            Console.Write("\u255D");
            Console.SetCursorPosition(widht / 2, top);
            Console.Write("<-C:/");
        }
    }
}