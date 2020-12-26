using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    public enum DialogWindowsType { INFO,DIALOG}
    public class DialogWindows:IDisposable
    {
        // String[] ButtonName;
        // FormNC DialogForm;
        Panel messagePanel;
        DialogWindowsType WindowType;
        public string ResultText { get; set; } = "";
        public bool Result { get; set; } = false;
        int Left;
        int Top;
        int Width;
        int Height;
        public DialogWindows(String caption_, String message, DialogWindowsType windowsType)
        {
            WindowType = windowsType;
            if (windowsType == DialogWindowsType.INFO && message != null && message.Length > 0)
                setSizeInfoWindows(message);
            else setSizeDialogWindows();
           
          
            messagePanel = new Panel(Height, Width, Top, Left, caption_, message, false);
            messagePanel.hasBorder = false;
            messagePanel.baseColor = ConsoleColor.Gray;
            messagePanel.textColor = ConsoleColor.DarkBlue;
            messagePanel.beginCursorPosY = Top + 2;            

        }
        public void show()
        {
            messagePanel.show();
            if (WindowType == DialogWindowsType.DIALOG)
                ResultText = messagePanel.getContentText();
            ConsoleKey key;
            do
            {               
                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.Enter:
                        Result = true; 
                        break;
                    case ConsoleKey.Escape:
                        Result = false;
                        break;
                    default:break;
                }                
            }
            while (key != ConsoleKey.Enter && key != ConsoleKey.Escape);
        }
        private void setSizeDialogWindows()
        {
            this.Width = Console.WindowWidth / 3;
            this.Left = this.Width;
            this.Height = 6;
            this.Top = Console.WindowHeight / 2 - 3;
        }

        private void setSizeInfoWindows(string message)
        {
            int len = message.Length;
            if (len <= Console.WindowWidth - 4)
            {
                this.Left = (Console.WindowWidth - len) / 2 + 2;
                this.Width = len + 2;
                this.Height = Console.WindowHeight > 7 ? 7 : Console.WindowHeight;

            }
            else
            {
                this.Left = 0;
                this.Width = Console.WindowWidth;
                this.Height = Console.WindowHeight > 7 + len / this.Width ? 7 + len / this.Width : Console.WindowHeight;

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
