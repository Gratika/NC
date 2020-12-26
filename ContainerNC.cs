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
        public double ratio { get; set; }
        //{
        //    get
        //    {
        //        return ratio;
        //    }
        //    set
        //    {
        //        if (value < 0) ratio = 0;
        //        else
        //        {
        //            if (value > 1) ratio = 1;
        //            else ratio = value;
        //        }

        //    }
        //}

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
                First.isActive = true;
            }

            this.Second = second_;
            if (Second != null) 
            { 
                rateSecond();
                Second.parent = this;
                if (First == null) Second.isActive = true;
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
                    Second.Left = Left;
                    Second.Top = Top;
                    Second.Height = getDisplayHeight();
                    Second.Width = getDisplayWidth();
                }
                else
                {
                    if (exis == Exis.Horizontal)
                    {
                        Second.Top = beginCursorPosX + First.Height;
                        Second.Left = beginCursorPosY;
                        Second.Width = getDisplayWidth();
                        Second.Height = getDisplayHeight() - First.Height;
                    }
                    else
                    {
                        Second.Top = beginCursorPosX;
                        Second.Left = beginCursorPosY + First.Width;
                        Second.Width = getDisplayWidth() - First.Width;
                        Second.Height = getDisplayHeight();
                    }
                    Second.beginCursorPosX = Second.Left+1;
                    Second.beginCursorPosY = Second.Top+1;
                }
            }
        }
       
       
        public override void Update()
        {
            base.Update();
            if(First!=null) rateFirst();
            if(Second !=null) rateSecond();
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

        public override void show()
        {
            First.show();
            Second.show();
        }
    }
}