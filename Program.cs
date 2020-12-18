using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{

    class Program
    {
       
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            int maxH = Console.LargestWindowHeight;
            int maxW = Console.LargestWindowWidth;
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(maxW, maxH);
            MyWindow w = new MyWindow(0, 0, maxW / 2 - 1, maxH,true);
            w.Drow();
            w.showMenu();
            //MyWindow.Drow(maxW / 2+1, 0, maxW, maxH);
           
           
        }
    }
}
