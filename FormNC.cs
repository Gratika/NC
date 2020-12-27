using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    public class FormNC:ContainerNC
    {
        private List<ControlNC> controls;
        public int activeIndex;
        public ControlNC activeDefault { get; set; }
           
       
        public FormNC(int height_, int width_, int top_, int left_, Exis exis_, double ratio_) : base(height_, width_, top_, left_, exis_, ratio_) 
        {
            this.controls = new List<ControlNC>();
        }
        public FormNC(int height_, int width_, int top_, int left_) :this(height_, width_, top_, left_, Exis.Vertical, 0) { }
        public FormNC() : this(Console.WindowHeight, Console.WindowWidth, 0, 0, Exis.Vertical, 0) { }
        public override void takeFocus()
        {
            activeDefault.takeFocus();
        }
        public override void keyPress(ConsoleKey key)
        {            
            switch (key)
            {
                case ConsoleKey.F10:
                   return;                
                default:
                    controls[activeIndex].keyPress(key);
                    break;
            }
        }
       
        public void addControl(ControlNC control, bool isActive=false)
        {
           //TODO:как распределить высоты?
            control.Width = getDisplayWidth();
            control.Top = beginCursorPosY;
            control.Left = beginCursorPosX;
            control.parent = this;
            beginCursorPosY += control.Height;
            if (isActive) activeIndex = controls.Count;
            controls.Add(control);
           
           
        }        
        
        public int getCountControl()
        {
            return controls.Count;
        }
        public override void show()
        {
            foreach(ControlNC c in controls)
            {
                c.show();
            }

        }
        public override void Update()
        {
            base.Update();
            foreach(ControlNC item in controls)
            {
                item.Update();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();            
        }

    }
}
