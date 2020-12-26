using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    public abstract class ControlNC
    {
        protected int WindowsHeight;
        protected int WindowsWidth;
        public int beginCursorPosX { get; set; }
        public int beginCursorPosY { get; set; }

        public int Top { get; set; }        

        public int Left { get; set; }
       
        public int Height { get; set; }
        
        public int Width { get; set; }
        

        public ContainerNC parent { get; set;} 
        public bool isActive { get; set; } = false;
        
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

        public virtual void takeFocus()
        {
            this.isActive= true;
            Console.SetCursorPosition(beginCursorPosX, beginCursorPosY);
            show();
        }
        public virtual void loseFocus()
        {
            this.isActive = false;
            show();
        }
        public abstract void keyPress(ConsoleKey key);
        public abstract void show();
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
            // show();
        }

        protected virtual void setWindowsHeight()
        {
            if (this.parent == null)
                this.WindowsHeight = Console.WindowHeight;
            else
                this.WindowsHeight = parent.getDisplayHeight();
        }

        protected virtual void setWindowsWidth()
        {
            if (this.parent == null)
                this.WindowsWidth = Console.WindowWidth;
            else
                this.WindowsWidth = parent.getDisplayWidth();
        }

        protected virtual int getDisplayHeight()
        {
            return Height;           
        }
        protected virtual int getDisplayWidth()
        {
            return Width;           
        }
    }
}
