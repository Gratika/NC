using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    class ListBox:Panel
    {
        public List<ResourseInfo> Sourse;
        public int IndFirstVisible
        {
            get
            {
                return IndFirstVisible;
            }
            set
            {
                if (value >= Sourse.Count)
                {
                    IndFirstVisible = 0;
                }
                else
                {
                    if (value < 0)
                        IndFirstVisible = Sourse.Count - 1;
                    else IndFirstVisible = value;
                }
            }
        }
        public int IndSelect
        {
            get
            {
                return IndSelect;
            }
            set
            {
                if (value >= Sourse.Count)
                {
                    IndSelect = 0;
                }
                else
                {
                    if (value < 0)
                        IndSelect = Sourse.Count - 1;
                    else IndSelect = value;
                }
            }
        }

        public void drawItem(int i)
        {
            if (IndSelect == i) 
                Console.BackgroundColor = selectedColor;
            else
                Console.BackgroundColor = baseColor;
            this.drawContent(Sourse[i].Name, getDisplayWidth());
            cursorPosX = beginCursorPosX;
            cursorPosY++;
        }
        public void drawPage (int indBgn, int indEnd)
        {
            for(int i=indBgn; i<indEnd && i<Sourse.Count; i++)
            {
                drawItem(i);
            }
        }

        public void getNextItem(int selectPos)
        {
            if (selectPos>IndFirstVisible && selectPos < getDisplayHeight())
            {
                IndSelect = selectPos + 1;
                drawPage(IndFirstVisible, IndFirstVisible + getDisplayHeight());
            }
        }

    }
}
