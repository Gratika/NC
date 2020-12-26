using System;

namespace NC
{
    public static class Border
    {
        public static void Drow(int left, int top, int widht, int height, ConsoleColor color, ConsoleColor baseColor)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = baseColor;
            Console.SetCursorPosition(left, top);
            Console.Write('\u2554');
            for (int i = 0; i < widht-2; i++)
                Console.Write('\u2550');
            Console.Write("\u2557\n");
            int y= top+1;
            for (int j =  0; j < height-2 ; j++)
            {
                Console.SetCursorPosition(left, y);
                Console.Write('\u2551');
                Console.SetCursorPosition(widht+left-1, y);
                Console.Write("\u2551\n");
                y++;
            }
            Console.SetCursorPosition(left, y);
            Console.Write('\u255A');
            for (int i = 0; i < widht-2; i++)
                Console.Write('\u2550');
            Console.Write("\u255D");
           
        }
    }
}