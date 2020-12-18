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
        protected int beginCursorPosX { get; }
        protected int beginCursorPosY { get;  }

        public int Top { 
            get
            {
                return Top;
            }
            set
            {
                if (parent != null)
                {
                    if (value > parent.Height || value < parent.beginCursorPosY)
                        Left = parent.beginCursorPosY;
                    else Left = value;
                }
                else
                {
                    if (value > Console.WindowHeight || value < 0)
                        Top = 0;
                    else Top = value;
                }
                
            }
        }

        public int Left
        {
            get
            {
                return Left;
            }
            set
            {
                if (parent != null)
                {
                    if (value > parent.Width || value < parent.beginCursorPosX)
                        Left = parent.beginCursorPosX;
                    else Left = value;
                }
                else
                {
                    if (value > Console.WindowWidth || value < 0)
                        Left = 0;
                    else Left = value;
                }
                
            }
        }
        public int Height
        {
            get
            {
                return Height;
            }
            set
            {
                if (parent != null)
                {
                    if (value > getDisplayHeight())
                        Height = getDisplayHeight();
                    else Height = value;
                }
                else
                {
                    if (value > Console.WindowHeight)
                        Height = Console.WindowHeight;
                    else Height = value;
                }
                
            }
        }
        public int Width {
            get
            {
                return Width;
            }
            set
            {
                if (parent != null)
                {
                    if (value > getDisplayWidth())
                        Width = getDisplayWidth();
                    else Width = value;
                }
                else
                {
                    if (value > Console.WindowWidth)
                        Width = Console.WindowWidth;
                    else Width = value;
                }
                
            }
        }

        public ContainerNC parent { get; set;} 
        public bool isActive { get; set; } = false;
        
        public ControlNC(int height_, int width_, int top_, int left_)
        {
            setWindowsHeight();
            setWindowsWidth();
            Height = height_;
            Width = width_;
            beginCursorPosX = Left + 1;
            beginCursorPosY = Top + 1;
            Top = top_;
            Left = left_;
        }
        public ControlNC(int height_, int width_):this(height_, width_, 0, 0) { }
        public ControlNC() : this(Console.WindowHeight, Console.WindowWidth, 0, 0) { }

        public virtual void takeFocus()
        {
            this.isActive= true;
            Console.SetCursorPosition(beginCursorPosX, beginCursorPosY);
        }
        public virtual void loseFocus()
        {
            this.isActive = false;
        }
       // public abstract void HandleEvent(ConsoleKey key);
        public virtual void Update()
        {
            setWindowsHeight();
            setWindowsWidth();
            if (Height > WindowsHeight)
                Height = WindowsHeight;
            if (Width > WindowsWidth)
                Width = WindowsWidth;
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
            return Height- 2;           
        }
        protected virtual int getDisplayWidth()
        {
            return Width - 2;           
        }
    }
}
