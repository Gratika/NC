using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    public class Panel : ControlNC
    {
        public ConsoleColor baseColor { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor activeColor { get; set; } = ConsoleColor.White;
        public ConsoleColor textColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor inactiveColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor selectedColor { get; set; } = ConsoleColor.Cyan;

        public int cursorPosX { get; set; }
        public int cursorPosY { get; set; }
        public bool cursorVisible { get; set; }
        public bool hasBorder { get; set; } = true;

        public string caption { get; set; }
        public string textContent { get; set; }


        public Panel(int height_, int width_, int top_, int left_, string caption_, string content_, bool cursorVisible_)
            : base(height_, width_, top_, left_)
        {
            this.caption = caption_;
            this.textContent = content_;
            cursorVisible = cursorVisible_;
            Console.CursorVisible = cursorVisible;
            beginCursorPosX = Left + 1;
            beginCursorPosY = Top + 1;
            cursorPosX = beginCursorPosX;
            cursorPosY = beginCursorPosY;
            Console.BackgroundColor = baseColor;
        }
        public Panel(int height_, int width_, int top_, int left_) : this(height_, width_, top_, left_, "", "", false) { }
        public Panel(string caption_, string content_, bool cursorVisible_) : this(Console.WindowHeight, Console.WindowWidth, 0, 0, caption_, content_, cursorVisible_) { }
        public Panel() : this(Console.WindowHeight, Console.WindowWidth, 0, 0, "", "", false) { }

        public void printCaption()
        {
            if (caption != null)
            {
                Console.ForegroundColor = textColor;
                Console.BackgroundColor = baseColor;
                int len = caption.Length;
                cursorPosY = Top;
                if (len >= Width - 4)
                {
                    cursorPosX = Left+2;
                    Console.SetCursorPosition(cursorPosX, cursorPosY);
                    Console.Write(caption.Substring(0, Width - 6)+"..");
                }
                else
                {
                    cursorPosX = (Width - 2 - len) / 2 + Left;
                    Console.SetCursorPosition(cursorPosX, cursorPosY);
                    Console.Write(caption);

                }
            }

            cursorPosX = beginCursorPosX;
            cursorPosY = beginCursorPosY;
        }
        public void drawBackGround()
        {
            int x = Left, y = Top;
            Console.ForegroundColor = baseColor;
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(x, y);
                for (int g = 0; g < Width; g++)
                    Console.Write('\u2588');
                y++;
            }
        }
        public virtual void drawText(string text, int maxWeidth/*, bool wordWrap*/)
        {
            Console.ForegroundColor = textColor;
            Console.SetCursorPosition(cursorPosX, cursorPosY);
            if (text.Length < maxWeidth)
            {
                Console.Write(text.PadRight(maxWeidth));
            }
            else
            {
                Console.Write(text.Substring(0, maxWeidth));
            }

        }

        public override void show()
        {
            drawBackGround();
            if (hasBorder)
            {
                if (isActive)
                    Border.Drow(Left, Top, Width, Height, activeColor, baseColor);
                else
                    Border.Drow(Left, Top, Width, Height, inactiveColor, baseColor);
            }
            printCaption();
            if (textContent != null && textContent.Length > 0)
                drawContent();
        }


        
        protected override int getDisplayHeight()
        {
            return Height - 2;
        }
        protected override int getDisplayWidth()
        {
            return Width - 2;
        }

        public override void keyPress(ConsoleKey key)
        {
            return;
        }
        
        public string getContentText()
        {
            Console.CursorVisible = true;
            int x = beginCursorPosX;
            int y = beginCursorPosY;
            Console.SetCursorPosition(x, y);
            int w = getDisplayWidth();
            int h = getDisplayHeight() - 3;
            StringBuilder sb = new StringBuilder();
            bool loop = true;
            int maxlen = 0;
            while (loop)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        {
                            loop = false;
                            break;
                        }
                    default:
                        {
                            if (sb.Length < 200)
                            {
                                sb.Append(keyInfo.KeyChar);
                                Console.Write(keyInfo.KeyChar);
                                maxlen++;
                                if (maxlen > w - 1)
                                {
                                    maxlen = 0;
                                    y++;
                                    Console.SetCursorPosition(x, y);
                                    h--;
                                    if (h == 0) loop = false;
                                }

                            }
                            break;
                        }
                }
            }

            Console.CursorVisible = false;
            return sb.ToString();
        }

        private void drawContent()
        {

            int maxWeidth = getDisplayWidth();
            cursorPosX = beginCursorPosX;
            cursorPosY = beginCursorPosY;
            string[] words = textContent.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int bgnPos = 0;
            while (bgnPos < words.Length && cursorPosY < Top+getDisplayHeight() - 1)
            {
                int maxLen = 0;
                string str = splitTextForPrint(ref bgnPos, ref words, ref maxLen, maxWeidth);
                drawText(str, maxWeidth);
                cursorPosY++;
            }
        }
        private string splitTextForPrint(ref int bgnPos, ref string[] words, ref int maxlen, int maxWeidth)
        {
            StringBuilder str = new StringBuilder();
            do
            {
                maxlen += words[bgnPos].Length;
                str.Append(words[bgnPos]+' ');
                bgnPos++;
            }   while (bgnPos < words.Length && maxlen + words[bgnPos].Length < maxWeidth);
            return str.ToString();
        }
    }
}
