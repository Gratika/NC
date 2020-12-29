 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    public abstract class ControlNC
    {
        protected int WindowsHeight;//высота окна
        protected int WindowsWidth;//ширина окна
        public int beginCursorPosX { get; set; }//начальная Х позиция курсора
        public int beginCursorPosY { get; set; }//начальная У позиция курсора

        public int Top { get; set; }  //позиция элемента относительно верхнего края      

        public int Left { get; set; }//позиция элемента относительно левого края   

        public int Height { get; set; }//высота элемента
        
        public int Width { get; set; }//ширина элемента
        

        public ContainerNC parent { get; set;} //родительский элемент
        public bool isActive { get; set; } = false;//является ли элемент активным
        
        public ControlNC(int height_, int width_, int top_, int left_)
        {
            setWindowsHeight();
            setWindowsWidth();
            Height = height_;
            Width = width_;
            Top = top_;
            Left = left_;
            beginCursorPosX = Left;
            beginCursorPosY = Top;
           
        }
        public ControlNC(int height_, int width_):this(height_, width_, 0, 0) { }
        public ControlNC() : this(Console.WindowHeight, Console.WindowWidth, 0, 0) { }

        /// <summary>
        /// описывает поведение элемента при получении фокуса
        /// </summary>
        public virtual void takeFocus()
        {
            this.isActive= true;
            Console.SetCursorPosition(beginCursorPosX, beginCursorPosY);
            show();
        }
        /// <summary>
        /// описывает поведение элемента при потери фокуса
        /// </summary>
        public virtual void loseFocus()
        {
            this.isActive = false;
            show();
        }
        public abstract void keyPress(ConsoleKey key);
        public abstract void show();
        /// <summary>
        /// обновление размеров элемента при изменении размеров окна
        /// </summary>
        public virtual void Update()
        {
            setWindowsHeight();
            setWindowsWidth();
            if (Height != WindowsHeight)
                Height = WindowsHeight;
            if (Width != WindowsWidth)
                Width = WindowsWidth;
            Top = 0;
            Left = 0;
            beginCursorPosX = Left;
            beginCursorPosY = Top;            
        }

        /// <summary>
        /// установить значение поля WindowsHeight
        /// </summary>
        protected virtual void setWindowsHeight()
        {
            if (this.parent == null)
                this.WindowsHeight = Console.WindowHeight-3;
            else
                this.WindowsHeight = parent.getDisplayHeight();
        }
        /// <summary>
        /// установить значение поля WindowsWidth
        /// </summary>
        protected virtual void setWindowsWidth()
        {
            if (this.parent == null)
                this.WindowsWidth = Console.WindowWidth;
            else
                this.WindowsWidth = parent.getDisplayWidth();
        }

        /// <summary>
        /// вернуть доступную для печати высоту элемента
        /// </summary>
        /// <returns></returns>
        protected virtual int getDisplayHeight()
        {
            return Height;           
        }
        /// <summary>
        ///  вернуть доступную для печати ширину элемента
        /// </summary>
        /// <returns></returns>
        protected virtual int getDisplayWidth()
        {
            return Width;           
        }
    }
}
