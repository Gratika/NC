using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    public enum DialogWindowsType { INFO,DIALOG, MENU}
    /// <summary>
    /// диалоговое окно. Используется для отображения информации для пользователя, получения информации от пользователя
    /// </summary>
    public class DialogWindows:IDisposable
    {
        /// <summary>
        /// основной элемент окна - панель
        /// </summary>
        Panel messagePanel;
        /// <summary>
        /// тип окна
        /// </summary>
        DialogWindowsType WindowType;
        /// <summary>
        /// меню - для выбора одного из нескольких возможных вариантов
        /// </summary>
        string[] WindowMenu;
        /// <summary>
        /// текст, введенный пользователем
        /// </summary>
        public string ResultText { get; set; } = "";
        /// <summary>
        /// результат окна;
        /// true-пользователь подтвердил действие;
        /// false - пользователь отменил действие;
        /// </summary>
        public bool Result { get; set; } = false;
        int Left;
        int Top;
        int Width;
        int Height;
        public DialogWindows(String caption_, DialogWindowsType windowsType, String message = null, string [] menu = null)
        {
            WindowType = windowsType;
            WindowMenu = menu;
            if (windowsType == DialogWindowsType.INFO && message != null && message.Length > 0)
                setSizeInfoWindows(message);
            else setSizeDialogWindows();
           
          
            messagePanel = new Panel(Height, Width, Top, Left, caption_, message, false);
            messagePanel.hasBorder = false;
            messagePanel.baseColor = ConsoleColor.Gray;
            messagePanel.textColor = ConsoleColor.DarkBlue;
            messagePanel.beginCursorPosY = Top + 2;            

        }
        /// <summary>
        /// отображение окна на экране
        /// </summary>
        public void show()
        {
            messagePanel.show();
            if (WindowType != DialogWindowsType.MENU)
            {
                if (WindowType == DialogWindowsType.DIALOG)
                {
                    ResultText = messagePanel.getContentText();
                    Console.SetCursorPosition(Left+1, Top + Height - 2);
                    Console.Write("Enter-подтвердить, Esc-отмена");
                }
                ConsoleKey key;
                do
                {
                    key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            Result = true;
                            break;
                        case ConsoleKey.Escape:
                            Result = false;
                            break;
                        default: break;
                    }
                }
                while (key != ConsoleKey.Enter && key != ConsoleKey.Escape);
            }
            else showMenu();

            
        }
        /// <summary>
        /// отображение меню на панели
        /// </summary>
        private void showMenu()
        {
            int weidthItem = 8 * WindowMenu.Length <= Width ? 8 : Width / WindowMenu.Length;
            int x = messagePanel.beginCursorPosX + (Width - weidthItem * WindowMenu.Length) / 2 - 1;
            Console.SetCursorPosition(x, messagePanel.beginCursorPosY);
           
            int rez = Menu.GorizontallMenu(WindowMenu, weidthItem);
            if (rez > 0)
                ResultText = WindowMenu[rez];
            else ResultText = "";
        }
        private void setSizeDialogWindows()
        {
            this.Width = Console.WindowWidth / 3;
            this.Left = this.Width;
            this.Height = 8;
            this.Top = Console.WindowHeight / 2 - 3;
        }
              

        private void setSizeInfoWindows(string message)
        {
            int len = message.Length;
            if (len <= Console.WindowWidth - 4)
            {
                this.Left = (Console.WindowWidth - len) / 2 + 2;
                this.Width = len + 2;
                this.Height = Console.WindowHeight > 9 ? 9 : Console.WindowHeight;

            }
            else
            {
                this.Left = 0;
                this.Width = Console.WindowWidth;
                this.Height = Console.WindowHeight > 9 + len / this.Width ? 9 + len / this.Width : Console.WindowHeight;

            }
            if (this.Height < Console.WindowHeight)
                this.Top = (Console.WindowHeight - this.Height) / 2;
            else this.Top = 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
