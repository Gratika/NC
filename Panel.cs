using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    public class Panel: ControlNC
    {
        public ConsoleColor baseColor { get; set; }
        public ConsoleColor activeColor { get; set; }
        public ConsoleColor textColor { get; set; }
        public int cursorPosX { get; set; }
        public int cursorPosY { get; set; }
        public bool cursorVisible { get; set; }
        public Border border = null;
        public string caption { get; set; }

        public Panel(int height_, int width_, int top_, int left_, ConsoleColor baseColor_, ConsoleColor activeColor_, ConsoleColor textColor_, bool cursorVisible_ )
            :base(height_, width_, top_, left_)
        {
            this.baseColor = baseColor_;
            this.activeColor = activeColor_;
            this.textColor = textColor_;            
            cursorPosX = Left + 1;
            cursorPosY = Top + 1;
            cursorVisible = cursorVisible_;
            Console.CursorVisible = cursorVisible;
        }
        public Panel(int height_, int width_, int top_, int left_): this(height_, width_, top_, left_, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.DarkBlue, false) { }
        public Panel():this(Console.WindowHeight, Console.WindowWidth, 0, 0, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.DarkBlue, false) { }

        public void printCaption()
        {
            Console.ForegroundColor = textColor;
            int len = caption == null ? 0 : caption.Length;
            cursorPosX = Top;
            if(len>= Width - 2)
            {
                cursorPosY = Left + 1;
                Console.SetCursorPosition(cursorPosY, cursorPosX);
                Console.Write(caption.Substring(0, Width - 2));
            }
            else
            {
                cursorPosY = (Width - 2 - len) / 2;
                Console.SetCursorPosition(cursorPosY, cursorPosX);
                Console.Write(caption);

            }
            cursorPosX = Left + 1;
            cursorPosY = Top + 1;
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
        public void drawContent(string text)
        {
            int len = text.Length;
        }

        public override void keyPress(ConsoleKey key)
        {
            throw new NotImplementedException();
        }
        //protected void beginDraw()

    }
}
