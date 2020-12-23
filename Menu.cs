using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    class Menu
    {
        public static int VerticalMenu(string[] elements)
        {
            int len = elements.Length;
            int maxLen = 0;
            foreach (var item in elements)
            {
                if (item.Length > maxLen)
                    maxLen = item.Length;
            }
            ConsoleColor bg = Console.BackgroundColor;
            ConsoleColor fg = Console.ForegroundColor;
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorVisible = false;
            int pos = 0;
            while (true)
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    Console.SetCursorPosition(x, y + i);
                    if (i == pos)
                    {
                        Console.BackgroundColor = fg;
                        Console.ForegroundColor = bg;
                    }
                    else
                    {
                        Console.BackgroundColor = bg;
                        Console.ForegroundColor = fg;
                    }
                    Console.Write(elements[i].PadRight(maxLen));
                }

                ConsoleKey consoleKey = Console.ReadKey().Key;
                switch (consoleKey)
                {

                    case ConsoleKey.Enter:

                        //Console.CursorVisible = true;
                        return pos;
                      
                    case ConsoleKey.Escape:
                        //Console.CursorVisible = true;
                        return len;
                      
                    case ConsoleKey.UpArrow:
                        if (pos > 0)
                            pos--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (pos < elements.Length - 1)
                            pos++;
                        break;

                    case ConsoleKey.Tab:
                        return -1;

                    default:
                        break;
                }


            }
        }

        public static int GorizontallMenu(string[] elements, int widthItem)
        {
            ConsoleColor bg = Console.BackgroundColor;
            ConsoleColor fg = Console.ForegroundColor;
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorVisible = false;
            int pos = 0; int dx = 0;
            while (true)
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    Console.SetCursorPosition(x +dx, y);
                    if (i == pos)
                    {
                        Console.BackgroundColor = fg;
                        Console.ForegroundColor = bg;
                    }
                    else
                    {
                        Console.BackgroundColor = bg;
                        Console.ForegroundColor = fg;
                    }
                    Console.Write(elements[i].PadRight(widthItem));
                }

                ConsoleKey consoleKey = Console.ReadKey().Key;
                switch (consoleKey)
                {

                    case ConsoleKey.Enter:

                        //Console.CursorVisible = true;
                        return pos;                    

                    case ConsoleKey.LeftArrow:
                        if (pos > 0)
                            pos--;
                        break;

                    case ConsoleKey.RightArrow:
                        if (pos < elements.Length - 1)
                            pos++;
                        break;

                    default:
                        break;
                }


            }
        }
    }
}
