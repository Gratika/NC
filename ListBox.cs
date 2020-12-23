using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    class FileListBox:Panel
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
        public FileExplorer Explorer;
        string soursePath = "C:\\";
        string targetPath;
        public FileListBox()

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

        public void getNextItem()
        {
            IndSelect++;
            if (IndSelect < Sourse.Count && IndSelect >= getDisplayHeight())
                IndFirstVisible++;
            if(IndSelect>=Sourse.Count)
            {
                IndFirstVisible = 0;
                IndSelect = 0;
            }
            drawPage(IndFirstVisible, IndFirstVisible + getDisplayHeight());

        }

        public void getPrevItem()
        {
            IndSelect--;
            if (IndSelect < IndFirstVisible && IndSelect >= 0)
                IndFirstVisible--;
            if (IndSelect <0)
            {
                IndFirstVisible = Sourse.Count-getDisplayHeight();
                IndSelect = Sourse.Count-1;
            }
            drawPage(IndFirstVisible, IndFirstVisible + getDisplayHeight());

        }
        public override void keyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow: 
                    getPrevItem();
                    break;
                case ConsoleKey.DownArrow:
                    getNextItem();
                    break;
                case ConsoleKey.Enter: //if directory then open
                    soursePath = Sourse[IndSelect].FullName;                   
                    if (Directory.Exists(soursePath))
                    {
                        Sourse.Clear();
                        Sourse = Explorer.GetResourseEntries(soursePath).ToList<ResourseInfo>();
                    }                    
                    break;
                case ConsoleKey.Escape:
                    Sourse.Clear();
                    DirectoryInfo d_info = Directory.GetParent(soursePath);
                    if (d_info != null) soursePath = d_info.FullName;
                    else soursePath = Directory.GetDirectoryRoot(soursePath);
                    Sourse = Explorer.GetResourseEntries(soursePath).ToList<ResourseInfo>();
                    break;
                case ConsoleKey.F3: //rename View file
                    break;
                case ConsoleKey.F4://copy Edit File
                    break;
                case ConsoleKey.F5://paste
                    break;
                case ConsoleKey.F6://cut or rename
                    break;
                case ConsoleKey.F7://new Directory
                    break;
                case ConsoleKey.F8://Delete
                    break;
            }
        }

    }
}
