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
        string[] userMenu = new string[]
        {
            "F2-Rename",
            "F3-View",
            "F4-Edit",
            "F5-Copy",
            "F6-Cut",
            "F7-New",
            "F8-Delete",
            "F9-Drivers",
            "F10-Exit"
        };
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
            int wItem = Console.WindowWidth / userMenu.Length;
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Menu.GorizontallMenu(userMenu, wItem, false);
            ConsoleKey consoleKey;
            do 
            {
                if (mainForm.Width != Console.WindowWidth || mainForm.Height != Console.WindowHeight) 
                { 
                    mainForm.Update();
                    mainForm.show();
                    wItem = Console.WindowWidth / userMenu.Length;
                    Console.SetCursorPosition(0, Console.WindowHeight - 2);
                    Menu.GorizontallMenu(userMenu, wItem, false);
                }               
                consoleKey = Console.ReadKey().Key;
                try
                {
                    mainForm.keyPress(consoleKey);
                }
                catch (Exception err)
                {
                    using (DialogWindows dw =new DialogWindows("Error!", DialogWindowsType.INFO, err.Message)) 
                    { 
                        dw.show();
                    }
                    mainForm.Update();
                }
            } 
            while (consoleKey != ConsoleKey.F10);
            
        }
    }
}
