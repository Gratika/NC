using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    /// <summary>
    /// класс, наследуемый от панели и умеющий работать со списком файлов и директорий
    /// </summary>
    class FileListBox:Panel
    {
        /// <summary>
        /// список файлов и директорий
        /// </summary>
        public List<ResourseInfo> Sourse;      
        /// <summary>
        /// индекс первого видимого на экране элемента списка
        /// </summary>
        public int IndFirstVisible { get; set; }

        /// <summary>
        /// индекс выделеного элемента списка
        /// </summary>
        public int IndSelect { get; set; }       
        
        public FileExplorer Explorer;
        /// <summary>
        /// путь к каталогу, содержимое которого отображается
        /// </summary>
        public string soursePath = "C:\\";       
        public FileListBox(int height_, int width_, int top_, int left_, bool cursorVisible_):base(height_,width_,top_,left_, "C:\\", "", cursorVisible_)
        {
            Explorer = new FileExplorer();
            Sourse = Explorer.GetResourseEntries(soursePath).ToList<ResourseInfo>();
        }

        public FileListBox(int height_, int width_, int top_, int left_) : this(height_, width_, top_, left_, false){}
        public FileListBox():this(Console.WindowHeight, Console.WindowWidth, 0, 0, false) { }
        /// <summary>
        /// отображение элемента списка
        /// </summary>
        /// <param name="i">индекс элемента, который отображается</param>
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
        /// <summary>
        /// отображение "страницы" элементов из списка
        /// </summary>
        /// <param name="indBgn">индекс первого отображаемого на странице элемента списка</param>
        /// <param name="indEnd">индекс последнего отображаемого на странице элемента списка</param>
        public void drawPage (int indBgn, int indEnd)
        {
            for(int i=indBgn; i<indEnd && i<Sourse.Count; i++)
            {
                drawItem(i);
            }
            cursorPosY = beginCursorPosY;

        }

        /// <summary>
        /// выделить следующий элемент списка
        /// </summary>
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

        /// <summary>
        /// выделить предыдущий элемент списка
        /// </summary>
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
        /// <summary>
        /// отображение на экране
        /// </summary>
        public override void show()
        {
            base.show();
            drawPage(IndFirstVisible, IndFirstVisible + getDisplayHeight());
        }
        /// <summary>
        /// реакция на нажатие клавиш
        /// </summary>
        /// <param name="key">нажатая клавиша</param>
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
                case ConsoleKey.Enter: // open resourse
                    openResourse();                    
                    break;
                case ConsoleKey.Escape:                   
                    soursePath = getParentPath(soursePath);
                    getNewSourse();
                    show();
                    break;
                case ConsoleKey.F2://rename
                    rename();
                    break;
                case ConsoleKey.F3: //open file
                    openResourse();
                    break;
                case ConsoleKey.F4://create TextFile
                    createFile();
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

        /// <summary>
        /// открыть файл или директорию
        /// </summary>
        private void openResourse()
        {
           
            if (Directory.Exists(Sourse[IndSelect].FullName))
            {
                soursePath = Sourse[IndSelect].FullName;
                openDirectory();
            }
            else  
            if (File.Exists(Sourse[IndSelect].FullName))
               openFile() ;


        }
        /// <summary>
        /// открыть директорию
        /// </summary>
        private void openDirectory()
        {
            try
            {
                getNewSourse();
                show();
               
            }
            catch
            {
                soursePath = getParentPath(soursePath);
                caption = soursePath;
                Sourse.Clear();
                Sourse = Explorer.GetResourseEntries(soursePath).ToList<ResourseInfo>();
                IndFirstVisible = 0;
                IndSelect = 0;
                throw;
            }
        }

        /// <summary>
        /// получить путь к родительскому каталогу
        /// </summary>
        /// <param name="path">путь к каталогу, для которого необходимо получить родительский</param>
        /// <returns>путь к родительскому каталогу</returns>
        private string getParentPath(string path)
        {
            string result;
            DirectoryInfo d_info = Directory.GetParent(path);
            if (d_info != null) result = d_info.FullName;
            else result = Directory.GetDirectoryRoot(path);
            return result;
        }
        /// <summary>
        /// обновить список файлов и директорий
        /// </summary>
        public void getNewSourse()
        {
            caption = soursePath;
            Sourse.Clear();
            Sourse = Explorer.GetResourseEntries(soursePath).ToList<ResourseInfo>();
            IndFirstVisible = 0;
            IndSelect = 0;           
            
        }
       /// <summary>
       /// скопировать файл или директорию
       /// </summary>
       private void copyResourse()
        {
            FileListBox target = parent.getInactiveControl() as FileListBox;
            if (target != null && target.soursePath != null && target.soursePath.Length > 0)
            {
                using (DialogWindows dw = new DialogWindows("Подтвердите действие", DialogWindowsType.INFO, $"Выполнить копирование {Sourse[IndSelect].Name} в директорию {target.soursePath}? (Enter-OK, Esc-отмена)"))
                {
                    dw.show();
                    parent.show();
                    if (dw.Result)//пользователь нажал Enter 
                    {
                        Explorer.Copy(Sourse[IndSelect].FullName, target.soursePath);
                        getNewSourse();
                        target.getNewSourse();
                        parent.show();
                    }
                    
                }
            }
        }

        /// <summary>
        /// переместить файл или директорию
        /// </summary>
        private void cutResourse()
        {
            FileListBox target = parent.getInactiveControl() as FileListBox;
            if (target != null && target.soursePath != null && target.soursePath.Length > 0)
            {
                using (DialogWindows dw = new DialogWindows("Подтвердите действие", DialogWindowsType.INFO, $"Переместить {Sourse[IndSelect].Name} в директорию {target.soursePath}?(Enter-OK, Esc-отмена)"))
                {
                    dw.show();
                    parent.show();
                    if (dw.Result)
                    { 
                        Explorer.Move(Sourse[IndSelect].FullName, target.soursePath);
                        getNewSourse();
                        target.getNewSourse();
                        parent.show();
                    }
                    
                }
            }
        }

        /// <summary>
        /// создать директорию
        /// </summary>
        private void createDirectory()
        {
            using (DialogWindows dw = new DialogWindows("Название директории", DialogWindowsType.DIALOG))
            {
                dw.show();
                parent.show();
                if (dw.Result)
                {
                    Explorer.CreateDirectory(soursePath, dw.ResultText);
                    getNewSourse();                   
                    parent.show();
                }
               
            }
        }

       /// <summary>
       /// удалить файл или директорию
       /// </summary>
       private void deleteResourse()
        {
            using (DialogWindows dw = new DialogWindows("Подтвердите действие", DialogWindowsType.INFO,$"Вы уверены, что хотите удалить {Sourse[IndSelect].Name}?(Enter-OK, Esc-отмена)"))
            {
                dw.show();
                parent.show();
                if (dw.Result)
                { 
                    Explorer.delete(Sourse[IndSelect].FullName);
                    getNewSourse();
                    parent.show();
                }
                
            }
        }

        /// <summary>
        /// получить список логических дисков ПК
        /// </summary>
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

        /// <summary>
        /// открыть файл
        /// </summary>
        private void openFile()
        { 
            Explorer.openFile(Sourse[IndSelect].FullName);
        }

        /// <summary>
        /// создать и открыть текстовый файл
        /// </summary>
        private void createFile()
        {
            using(DialogWindows dw = new DialogWindows("Название файла", DialogWindowsType.DIALOG))
            {
                dw.show();
                parent.show();
                if (dw.Result)
                {
                    string fileName = dw.ResultText + ".txt";
                    string fullFileName = Path.Combine(soursePath, fileName);
                    Explorer.createFile(soursePath, fileName);
                    getNewSourse();
                    parent.show();
                    Explorer.openFile(fullFileName);
                }
               
            }
        }
        /// <summary>
        /// переименовать файл или директорию
        /// </summary>
        private void rename()
        {
            using (DialogWindows dw = new DialogWindows("Новое название", DialogWindowsType.DIALOG))
            {
                dw.show();
                parent.show();
                if (dw.Result)
                {
                    string newName = dw.ResultText;
                    Explorer.renameResourse(Sourse[IndSelect].FullName, newName);
                    getNewSourse();
                }
                           
            }
            parent.show();
        }
    }
}
