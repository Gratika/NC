using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    public class Panel: ControlNC
    {
        public ConsoleColor baseColor { get; set; } = ConsoleColor.Blue;
        public ConsoleColor activeColor { get; set; } = ConsoleColor.White;
        public ConsoleColor textColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor inactiveColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor selectedColor { get; set; } = ConsoleColor.Cyan;

        public int cursorPosX { get; set; }
        public int cursorPosY { get; set; }
        public bool cursorVisible { get; set; }
        public bool hasBorder { get; set; }

        public string caption { get; set; }
        public string textContent { get; set; }


        public Panel(int height_, int width_, int top_, int left_, string caption_, string content_,bool cursorVisible_ )
            :base(height_, width_, top_, left_)
        {
            this.caption = caption_;
            this.textContent = content_;
            cursorVisible = cursorVisible_;
            Console.CursorVisible = cursorVisible;
            cursorPosX = beginCursorPosX;
            cursorPosY = beginCursorPosY;
        }
        public Panel(int height_, int width_, int top_, int left_): this(height_, width_, top_, left_,"","", false) { }
        public Panel():this(Console.WindowHeight, Console.WindowWidth, 0,0,"","", false) { }

        public void printCaption()
        {
            Console.ForegroundColor = textColor;
            int len = caption == null ? 0 : caption.Length;
            cursorPosX = Top;
            if(len>= Width - 2)
            {
                cursorPosY = beginCursorPosY;
                Console.SetCursorPosition(cursorPosY, cursorPosX);
                Console.Write(caption.Substring(0, Width - 2));
            }
            else
            {
                cursorPosY = (Width - 2 - len) / 2;
                Console.SetCursorPosition(cursorPosY, cursorPosX);
                Console.Write(caption);

            }
            cursorPosX = beginCursorPosX;
            cursorPosY = beginCursorPosY;
        }
        public void drawBackGround()
        {
            Console.SetCursorPosition(Left, Top);
            Console.ForegroundColor = baseColor;
            for(int i=Top; i<Height; i++)
            {
                for (int g = Left; g < Width; g++)
                    Console.Write('\u2588');
                Console.Write('\n');
            }
        }
        public virtual void drawContent(string text, int maxWeidth/*, bool wordWrap*/)
        {
            Console.ForegroundColor = textColor;
            Console.SetCursorPosition(cursorPosX, cursorPosY);
            if (text.Length < maxWeidth) 
            {
                Console.WriteLine(text.PadRight(maxWeidth));
            }
            else
            {
                Console.WriteLine(text.Substring(0, maxWeidth));
            }
           
            //if (!wordWrap)
            //{
            //    Console.WriteLine(text.Substring(0, maxWeidth));
            //    cursorPosX = beginCursorPosX;
            //    cursorPosY++;

            //}
            //else
            //{
            //    string[] words = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //    int bgnPos=0;
            //    while (bgnPos < words.Length)
            //    {
            //        int maxLen = 0;
            //        string str = splitTextForPrint(ref bgnPos, ref words, ref maxLen, maxWeidth);
            //        Console.WriteLine(str);
            //    }


            //}
            //int len = text.Length;
            //int dh = this.getDisplayHeight()- 2;
            //int dw = this.getDisplayWidth() - 2;
            //cursorPosY = dh / 2 - 1;
            //if (len > dw)
            //{
            //   // StringBuilder str1 = new StringBuilder();
            //    //StringBuilder str2 = new StringBuilder();               
            //    string[] words = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //    int dl = 0, bgnPos=0;


            //    while (i < words.Length)
            //    {
            //        str2.Append(words[i]);
            //        i++;
            //    }
            //    cursorPosX = beginCursorPosX + (dw - dl) / 2;
            //    Console.SetCursorPosition(cursorPosX, cursorPosY);
            //    Console.WriteLine(str1);
            //    cursorPosX = beginCursorPosX + (dw - text.Length + dl) / 2;
            //    cursorPosY = dh / 2;
            //    Console.SetCursorPosition(cursorPosX, cursorPosY);
            //    Console.WriteLine(str2);

            //}
            //else
            //{
            //    cursorPosX = beginCursorPosX + (dw - text.Length) / 2;
            //    Console.SetCursorPosition(cursorPosX, cursorPosY);
            //    Console.WriteLine(text);
            //}

        }

        public virtual void draw()
        {
            drawBackGround();
            if (hasBorder)
            {
                Border.Drow(Left, Top, Width, Height);
            }
            printCaption();
            drawContent(textContent,getDisplayWidth());
        }
               

        public override void keyPress(ConsoleKey key)
        {
            throw new NotImplementedException();
        }
        protected string splitTextForPrint(ref int bgnPos, ref string[] words, ref int maxlen, int maxWeidth)
        {
            StringBuilder str = new StringBuilder();
            do
            {
                maxlen += words[bgnPos].Length;
                str.Append(words[bgnPos]);
                bgnPos++;
            } while (maxlen + words[bgnPos].Length < maxWeidth);
            return str.ToString();
        }
        //protected void beginDraw()

    }
}
