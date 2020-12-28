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
        public int IndFirstVisible { get; set; }
       
        public int IndSelect { get; set; }
       
        public FileExplorer Explorer;
        public string soursePath = "C:\\";       
        public FileListBox(int height_, int width_, int top_, int left_, bool cursorVisible_):base(height_,width_,top_,left_, "C:\\", "", cursorVisible_)
        {
            Explorer = new FileExplorer();
            Sourse = Explorer.GetResourseEntries(soursePath).ToList<ResourseInfo>();
        }

        public FileListBox(int height_, int width_, int top_, int left_) : this(height_, width_, top_, left_, false){}
        public FileListBox():this(Console.WindowHeight, Console.WindowWidth, 0, 0, false) { }
        public void drawItem(int i)
        {
            if (IndSelect == i && isActive) 
                Console.BackgroundColor = selectedColor;
            else
                Console.BackgroundColor = baseColor;
            this.drawText(Sourse[i].Name, getDisplayWidth()-1);
            cursorPosX = beginCursorPosX;
            cursorPosY++;
        }
        public void drawPage (int indBgn, int indEnd)
        {
            for(int i=indBgn; i<indEnd && i<Sourse.Count; i++)
            {
                drawItem(i);
            }
            cursorPosY = beginCursorPosY;

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
                if (Sourse.Count > getDisplayHeight())
                    IndFirstVisible = Sourse.Count - getDisplayHeight();
                else IndFirstVisible = 0;
                IndSelect = Sourse.Count-1;
            }
            drawPage(IndFirstVisible, IndFirstVisible + getDisplayHeight());

        }
        public override void show()
        {
            base.show();
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
                    openDirectory();                    
                    break;
                case ConsoleKey.Escape:                   
                    soursePath = getParentPath(soursePath);
                    getNewSourse();
                    show();
                    break;
                case ConsoleKey.F2://rename
                    
                    break;
                case ConsoleKey.F3: //View file
                    viewFile();
                    break;
                case ConsoleKey.F4://Edit TextFile
                    break;
                case ConsoleKey.F5://copy
                    copyResourse();
                    break;

                case ConsoleKey.F6://cut
                    cutResourse();
                    break;
                case ConsoleKey.F7://new Directory
                    createDirectory();                   
                    break;
                case ConsoleKey.F8://Delete
                    deleteResourse();
                    break;
                case ConsoleKey.F9://Menu Drivers
                    getDrivers();
                    break;
                default:break;
            }
        }

        private void openDirectory()
        {
            try
            {
                soursePath = Sourse[IndSelect].FullName;
                if (Directory.Exists(soursePath))
                {
                    getNewSourse();
                    show();
                }
            }
            catch
            {
                soursePath = getParentPath(soursePath);
                Sourse.Clear();
                Sourse = Explorer.GetResourseEntries(soursePath).ToList<ResourseInfo>();
                IndFirstVisible = 0;
                IndSelect = 0;
                throw;
            }
        }

        private string getParentPath(string path)
        {
            string result;
            DirectoryInfo d_info = Directory.GetParent(path);
            if (d_info != null) result = d_info.FullName;
            else result = Directory.GetDirectoryRoot(path);
            return result;
        }
        public void getNewSourse()
        {
            caption = soursePath;
            Sourse.Clear();
            Sourse = Explorer.GetResourseEntries(soursePath).ToList<ResourseInfo>();
            IndFirstVisible = 0;
            IndSelect = 0;           
            
        }
        private void copyResourse()
        {
            FileListBox target = parent.getInactiveControl() as FileListBox;
            if (target != null && target.soursePath != null && target.soursePath.Length > 0)
            {
                using (DialogWindows dw = new DialogWindows("Подтвердите действие", DialogWindowsType.INFO, $"Выполнить копирование {Sourse[IndSelect].Name} в директорию {target.soursePath}? (Enter-OK, Esc-отмена)"))
                {
                    dw.show();
                    if (dw.Result)
                        Explorer.Copy(Sourse[IndSelect].FullName, target.soursePath);
                    getNewSourse();
                    target.getNewSourse();
                    parent.show();
                }
            }
        }

        private void cutResourse()
        {
            FileListBox target = parent.getInactiveControl() as FileListBox;
            if (target != null && target.soursePath != null && target.soursePath.Length > 0)
            {
                using (DialogWindows dw = new DialogWindows("Подтвердите действие", DialogWindowsType.INFO, $"Переместить {Sourse[IndSelect].Name} в директорию {target.soursePath}?"))
                {
                    dw.show();
                    if (dw.Result)
                        Explorer.Move(Sourse[IndSelect].FullName, target.soursePath);
                    getNewSourse();
                    target.getNewSourse();
                    parent.show();
                }
            }
        }

        private void createDirectory()
        {
            using (DialogWindows dw = new DialogWindows("Название директории", DialogWindowsType.DIALOG))
            {
                dw.show();
                if (dw.Result)
                {
                    Explorer.CreateDirectory(soursePath, dw.ResultText);
                    getNewSourse();
                }
                parent.show();
            }
        }

        private void deleteResourse()
        {
            using (DialogWindows dw = new DialogWindows("Подтвердите действие", DialogWindowsType.INFO,$"Вы уверены, что хотите удалить {Sourse[IndSelect].Name}?"))
            {
                dw.show();
                if (dw.Result)
                    Explorer.delete(Sourse[IndSelect].FullName);
                getNewSourse();
                parent.show();
            }
        }

        private void getDrivers()
        {
            string[] driversMenu = Explorer.GetDrivers();
            using(DialogWindows dw = new DialogWindows("Выбор диска", DialogWindowsType.MENU, null, driversMenu))
            {
                dw.show();
                if (dw.ResultText != null && dw.ResultText.Length > 0)
                { 
                    soursePath = dw.ResultText;
                    getNewSourse();
                }
                
                parent.show();
            }
        }

        private void viewFile()
        {
           
            Console.Clear();
            //Console.BackgroundColor = ConsoleColor.DarkBlue;
            //Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            Explorer.ReadFile(Sourse[IndSelect].FullName);
            Console.WindowWidth--;

        }

    }
}
