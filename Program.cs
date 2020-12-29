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

            
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            //int maxH = Console.LargestWindowHeight;
            //int maxW = Console.LargestWindowWidth;           
            //Console.SetWindowSize(maxW, maxH);
            WindowsNC NC = new WindowsNC();
            NC.show();
           
            

        }
    }
}
