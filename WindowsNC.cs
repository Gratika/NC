using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    class WindowsNC
    {
        FormNC mainForm;
        public WindowsNC()
        {
            mainForm = new FormNC();
            ContainerNC c = new ContainerNC(mainForm.Height - 5, mainForm.Width, mainForm.Top, mainForm.Left,Exis.Vertical, 0.5);
            c.addControl (new FileListBox(), new FileListBox());
            mainForm.addControl(c, true);          
            mainForm.activeDefault=c;
        }
        public void show()
        {
            mainForm.show();
            ConsoleKey consoleKey;
            do 
            {
                if (mainForm.Width != Console.WindowWidth || mainForm.Height != Console.WindowHeight) mainForm.Update();               
                consoleKey = Console.ReadKey().Key;
                try
                {
                    mainForm.keyPress(consoleKey);
                }
                catch (Exception err)
                {
                    new DialogWindows("Error!", err.Message, DialogWindowsType.INFO).show();
                    mainForm.Update();
                }
            } 
            while (consoleKey != ConsoleKey.F10);
            
        }
    }
}
