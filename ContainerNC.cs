using System;

namespace NC
{
    public enum Exis { Vertical = 1, Horizontal = 2 }
    public class ContainerNC : ControlNC
    {
        public ControlNC First { get; set; }
        public ControlNC Second { get; set; }
        public Exis exis { get; set; } = Exis.Vertical;
        /// <summary>
        /// соотношение контролов First и Second;
        /// 0-всю доступную область контейнера занимает контрол First;
        /// 1 - всю доступную область контейнера занимает контрол Second;
        /// промежуточное значение указывает на промежуточный размер ;
        /// </summary>
        public double ratio
        {
            get
            {
                return ratio;
            }
            set
            {
                if (value < 0) ratio = 0;
                else
                {
                    if (value > 1) ratio = 1;
                    else ratio = value;
                }

            }
        }

        public ContainerNC(int height_, int width_, int top_, int left_, Exis exis_, double ratio_) : base(height_, width_, top_, left_)
        {
            this.exis = exis_;
            this.ratio = ratio_;
        }
        public ContainerNC(int height_, int width_, int top_, int left_) : this(height_, width_, top_, left_, Exis.Vertical, 0) { }

        public ContainerNC() : this(Console.WindowHeight, Console.WindowWidth, 0, 0, Exis.Vertical, 0) { }

        /// <summary>
        /// Привязка контролов к контейнеру 
        /// </summary>
        /// <param name="first_">первый контрол; в зависимости от положения оси занимает левую или верхнюю половину контейнера</param>
        /// <param name="second_">второй контрол;в зависимости от положения оси занимает правую или нижнюю половину контейнера</param>
        public void addControl(ControlNC first_, ControlNC second_ = null)
        {

            this.First = first_;
            if (First != null) 
            { 
                rateFirst();
                First.parent = this;
            }

            this.Second = second_;
            if (Second != null) 
            { 
                rateSecond();
                Second.parent = this;
            }
            
        }

        /// <summary>
        /// Расчет размеров и положения первого контрола в контейнере;
        /// левый верхний угол контрола совпадает с левым верхним углом контейнера;
        /// в зависимости от положения оси занимает левую или верхнюю половину контейнера
        /// </summary>        
        protected void rateFirst()
        {
            First.Top = beginCursorPosX;
            First.Left = beginCursorPosY;
            if (ratio == 1)
            {
                First.Height = 0;
                First.Width = 0;
            }
            else
            {
                if (ratio == 0)
                {
                    First.Height = getDisplayHeight();
                    First.Width = getDisplayWidth();
                }
                else
                {
                    if (exis == Exis.Vertical)
                    {
                        First.Height = getDisplayHeight();
                        First.Width = (int)Math.Round(getDisplayWidth() * ratio) - 1;
                    }
                    else
                    {
                        First.Width = getDisplayWidth();
                        First.Height = (int)Math.Round(getDisplayHeight() * ratio) - 1;
                    }
                }
            }

        }

        /// <summary>
        /// Расчет размеров и положения второго контрола в контейнере;
        /// положение и размер расчитывается с учетом размеров первого контрола
        /// </summary>        
        protected void rateSecond()
        {
            if (ratio == 0)
            {
                Second.Height = 0;
                Second.Width = 0;
            }
            else
            {
                if (ratio == 1)
                {
                    Second.Height = getDisplayHeight();
                    Second.Width = getDisplayWidth();
                }
                else
                {
                    if (exis == Exis.Horizontal)
                    {
                        Second.Top = beginCursorPosX + First.Height + 2;
                        Second.Left = beginCursorPosY;
                        Second.Width = getDisplayWidth();
                        Second.Height = getDisplayHeight() - First.Height - 2;
                    }
                    else
                    {
                        Second.Top = beginCursorPosX;
                        Second.Left = beginCursorPosY + First.Width + 2;
                        Second.Width = getDisplayWidth() - First.Width - 2;
                        Second.Height = getDisplayHeight();
                    }
                }
            }
        }
        protected override int getDisplayHeight()
        {
            if (First != null && Second != null)
                return 0;
            if (First==null && Second==null || exis == Exis.Vertical)
                return base.getDisplayHeight();            
            else
            {
                int h = Height;
                if (First != null) h -= First.Height + 1;
                if (Second != null) h -= Second.Height + 1;
                return h;
            }  
        }
        protected override int getDisplayWidth()
        {
            if (First != null && Second != null)
                return 0;
            if (First == null && Second == null || exis == Exis.Horizontal) 
               return base.getDisplayWidth();
            else
            {
                int w = Width;
                if (First != null) w -= First.Width + 1;
                if (Second != null) w -= Second.Width + 1;
                return w;
            }
                       
        }
        public override void Update()
        {
            base.Update();
            rateFirst();
            rateSecond();
        }
        protected ControlNC getActiveControl()
        {
            if (First != null && First.isActive)
                return First;
            if (Second != null && Second.isActive)
                return Second;
            return null;
        }
        public override void takeFocus()
        {
            isActive = true;
            if(First!=null && First.Height>0 && Second.Width>0)
                First.takeFocus();
            else
            {
                if (Second != null && Second.Height > 0 && Second.Width > 0)
                    Second.takeFocus();
            }
        }
        public override void loseFocus()
        {
            base.loseFocus();
            if (getActiveControl() != null)
            {
                getActiveControl().isActive = false;
            }

        }
        public override void keyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Tab:
                    transferFocus();
                    break;
                default:
                    if (getActiveControl() != null)
                        getActiveControl().keyPress(key);
                    break;
            }
        }
        private void transferFocus()
        {
            if (First != null && First.isActive)
            {
                if (Second != null) 
                {
                    Second.takeFocus();
                    First.loseFocus();
                }
                return;
                
            }
            if(Second!=null && Second.isActive)
            {
                if (First != null)
                {
                    First.takeFocus();
                    Second.loseFocus();
                }
            }
                

        }
    }
}