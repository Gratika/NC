using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    public class FormNC:ContainerNC
    {
        /// <summary>
        /// список элементов, которые размещаются на форме
        /// </summary>
        private List<ControlNC> controls;
        /// <summary>
        /// индекс активного элемента
        /// </summary>
        public int activeIndex;
        /// <summary>
        /// элемент на форме, который получает фокус по умолчанию
        /// </summary>
        public ControlNC activeDefault { get; set; }
           
       
        public FormNC(int height_, int width_, int top_, int left_, Exis exis_, double ratio_) : base(height_, width_, top_, left_, exis_, ratio_) 
        {
            this.controls = new List<ControlNC>();
        }
        public FormNC(int height_, int width_, int top_, int left_) :this(height_, width_, top_, left_, Exis.Vertical, 0) { }
        public FormNC() : this(Console.WindowHeight, Console.WindowWidth, 0, 0, Exis.Vertical, 0) { }
        /// <summary>
        /// описывает поведение формы при получении фокуса
        /// фокус передается элементу, который определен как активный по умолчанию
        /// </summary>
        public override void takeFocus()
        {
            activeDefault.takeFocus();
        }
        /// <summary>
        /// реакция на нажатие клавиши.
        /// управление передается в метод keyPress активного элемента
        /// </summary>
        /// <param name="key">нажатая клавиша</param>
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
       
        /// <summary>
        /// добавление элемента на форму;
        /// ширина элемента устанавливается равной ширине формы
        /// </summary>
        /// <param name="control">элемент, который добавляется на форму</param>
        /// <param name="isActive">флаг, определяющий активность элемента</param>
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
        
       /// <summary>
       /// возвращает количество элементов на форме
       /// </summary>
       /// <returns></returns>
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
       /// <summary>
       /// обновление формы
       /// </summary>
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
