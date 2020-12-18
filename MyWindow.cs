using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC
{
    class MyWindow
    {
        public int top { get; set; }
        public int left { get; set; }
        public int widht { get; set; }
        public int height { get; set; }
        public  bool isActive { get; set; }

       
        public MyWindow() { }
        public MyWindow(int left_, int top_, int widht_, int height_, bool isActive_)
        {
            top = top_;
            left = left_;
            widht = widht_;
            height = height_;
            isActive = isActive_;
        }
        public void Drow()
        {
            Console.SetCursorPosition(left, top);
            Console.Write('\u2554');
            for (int i = left + 1; i < widht - 1; i++)
                Console.Write('\u2550');
            Console.Write("\u2557\n");
            int j;
            for (j = top + 1; j < height - 1; j++)
            {
                Console.SetCursorPosition(left, j);
                Console.Write('\u2551');
                Console.SetCursorPosition(widht - 1, j);
                Console.Write("\u2551\n");
            }
            Console.SetCursorPosition(left, j);
            Console.Write('\u255A');
            for (int i = left + 1; i < widht - 1; i++)
                Console.Write('\u2550');
            Console.Write("\u255D");
            Console.SetCursorPosition(widht / 2, top);
            Console.Write("<-C:/");
        }
        public void showMenu()
        {
            
            string path = "C:\\";
            string[] s = GetResourseEntries(path, true);
            string[] fullName = Directory.GetFileSystemEntries(path);
            int rez = 0;
            do
            {
                Drow();
                Console.SetCursorPosition(left+1,top+1);
                int len = s.Length;
                rez = Menu.VerticalMenu(s);
                if (rez >= 0 && rez < len)
                {
                    path = fullName[rez];
                }
                if (rez == len)
                {
                    DirectoryInfo d_info = Directory.GetParent(path);
                    if (d_info != null)
                        path = d_info.FullName;
                    else 
                        path = Directory.GetDirectoryRoot(path);
                    
                }
                s = GetResourseEntries(path,true);
                fullName = Directory.GetFileSystemEntries(path);
                Console.Clear();
            } while (rez != -1);
            
        }
        private string[] GetResourseEntries(string path, bool withAttributes)
        {
            List<string> tmp = new List<string>();            
            //string[] directories = Directory.GetDirectories(path);
            //foreach(string item in directories)
            //{
            //    tmp.Add(Path.GetDirectoryName(item));
            //}
            //string[] files = Directory.GetFiles(path);
            //foreach (string item in files)
            //{
            //    tmp.Add(Path.GetFileName(item));
            //}
           
            DirectoryInfo d_info = new DirectoryInfo(path);
            FileSystemInfo[] fsi = d_info.GetFileSystemInfos();
            foreach (var item in fsi)
            {
                StringBuilder f_name = new StringBuilder();
                if (withAttributes)
                {
                    f_name.Append(item.Name.PadRight(15));
                    switch (item.Attributes)
                    {
                        case FileAttributes.Directory:
                            f_name.Append("каталог".PadLeft(10));
                            break;
                        case FileAttributes.Hidden:
                            f_name.Append("скрытый".PadLeft(10));
                            break;
                        case FileAttributes.System:
                            f_name.Append("системный".PadLeft(10));
                            break;

                        default: break;
                    }
                }
                else f_name.Append(item.Name);
                
                tmp.Add(f_name.ToString());
            }
            return tmp.ToArray();
        }
    }
}
