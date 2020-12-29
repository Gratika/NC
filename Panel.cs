using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    public class Panel : ControlNC
    {
        /// <summary>
        /// цвет основы (фона)
        /// </summary>
        public ConsoleColor baseColor { get; set; } = ConsoleColor.DarkBlue;
        /// <summary>
        /// цвет границ активного элемента
        /// </summary>
        public ConsoleColor activeColor { get; set; } = ConsoleColor.White;
        /// <summary>
        /// цвет текста
        /// </summary>
        public ConsoleColor textColor { get; set; } = ConsoleColor.Black;
        /// <summary>
        /// цвет границ неактивного элемента
        /// </summary>
        public ConsoleColor inactiveColor { get; set; } = ConsoleColor.Gray;
        /// <summary>
        /// цвет выбраного элемента
        /// </summary>
        public ConsoleColor selectedColor { get; set; } = ConsoleColor.Cyan;

        /// <summary>
        /// позиция Х курсора 
        /// </summary>
        public int cursorPosX { get; set; }
        /// <summary>
        ///  позиция У курсора 
        /// </summary>
        public int cursorPosY { get; set; }
        /// <summary>
        /// видим курсор или нет
        /// </summary>
        public bool cursorVisible { get; set; }
        /// <summary>
        /// свойство определяет, имеет ли элемент рамку
        /// </summary>
        public bool hasBorder { get; set; } = true;

       /// <summary>
       /// заголовок панели
       /// </summary>
       public string caption { get; set; }
       /// <summary>
       /// текст, который отображается на панели
       /// </summary>
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

        /// <summary>
        /// виводит заголовок панели
        /// </summary>
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
        /// <summary>
        /// печатает фон панели
        /// </summary>
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
       /// <summary>
       /// печатает один рядок текста на панели
       /// если длина рядка больше отведенной под него ширины, то он обрезается
       /// </summary>
       /// <param name="text">рядок текста, который необходимо отобразить</param>
       /// <param name="maxWeidth">ширина, доступная для печати</param>
        public virtual void drawText(string text, int maxWeidth)
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

        /// <summary>
        /// отображение панели на экране;
        /// печатается основа, границы, заголовок и контент(отображаемый на панели текст)
        /// </summary>
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


        
        /// <summary>
        /// возвращает доступную для печати Высоту панели
        /// </summary>
        /// <returns></returns>
        protected override int getDisplayHeight()
        {
            return Height - 2;
        }
        /// <summary>
        /// возвращает доступную для печати Ширину панели
        /// </summary>
        /// <returns></returns>
        protected override int getDisplayWidth()
        {
            return Width - 2;
        }

        /// <summary>
        /// реакция на нажатие клавиш
        /// нет специальных клавиш управления
        /// </summary>
        /// <param name="key"></param>
        public override void keyPress(ConsoleKey key)
        {
            return;
        }

        /// <summary>
        /// ввод текста пользователем
        /// Максимальная длина строки, которую может ввести пользователь не можен привышать 200 символов;
        /// также на длину строки влияет значение Width и Height панели
        /// Ввод текста заканчивается, если пользователь нажал клавишу Enter, или же длина введенного текста превышает максимально допустимую
        /// </summary>
        /// <returns>строка текста, которую ввел пользователь</returns>
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
                if ((keyInfo.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt)
                    continue;
                if ((keyInfo.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
                    continue;                
                if (keyInfo.KeyChar == '\u0000') continue;               
                if (keyInfo.Key == ConsoleKey.Tab) continue;
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if(sb.Length > 0)
                    {
                        show();
                        int oldLen = sb.Length;
                        sb.Remove(oldLen - 1, 1);
                        h = getDisplayHeight() - 3;
                        y = beginCursorPosY;
                        Console.SetCursorPosition(x, y);
                        maxlen = sb.Length%w;
                        for(int i=0; i<sb.Length; i++)
                        {
                            if(i>0 && i % w == 0)
                            {
                                y++;
                                Console.SetCursorPosition(x, y);
                            }
                            Console.Write(sb[i]);
                        }
                    }
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.Enter) 
                { 
                    loop = false;
                    continue;
                }

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
                else loop = false;
                           
                
            }

            Console.CursorVisible = false;
            return sb.ToString();
        }


        /// <summary>
        /// печать контента на форме
        /// если длина текста превышает ширину панели, он разбивается на строки;
        /// количество строк текста не может превышать высоту панели 
        /// </summary>
        private void drawContent()
        {

            int maxWeidth = getDisplayWidth();
            cursorPosX = beginCursorPosX;
            cursorPosY = beginCursorPosY;
            string[] words = textContent.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int bgnPos = 0;
            while (bgnPos < words.Length && cursorPosY < Top+getDisplayHeight() - 1)
            {
                //int maxLen = 0;
                string str = splitTextForPrint(ref bgnPos, ref words,/* ref maxLen,*/ maxWeidth);
                drawText(str, maxWeidth);
                cursorPosY++;
            }
        }
        /// <summary>
        /// формирует из слов отдельные строки
        /// </summary>
        /// <param name="bgnPos">начальная позиция в массиве слов</param>
        /// <param name="words">массив слов</param>       
        /// <param name="maxWeidth">максимально допустимая длина строки</param>
        /// <returns>сформированная строка текста</returns>
        private string splitTextForPrint(ref int bgnPos, ref string[] words,/* ref int maxlen,*/ int maxWeidth)
        {
            int maxlen = 0;
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
