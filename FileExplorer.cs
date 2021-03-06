﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

struct ResourseInfo
{
    public string FullName { get; set; }
    public string Name { get; set; }
   // public int Size { get; set; }
    public string DateCreate { get; set; }

    public string TimeCreate { get; set; }
    public string FAttr { get; set; }

    public string printName()
    {
        return Name;
    }
    public string printAll()
    {
        return $"{Name} {DateCreate} {TimeCreate} {FAttr}";
    }
}
namespace NC
{
    class FileExplorer
    {
        public void CreateDirectory(string targetPath, string newDirName)
        {
            DirectoryInfo sourseInfo = new DirectoryInfo(targetPath);
            if (!sourseInfo.Exists) sourseInfo.Create(); 
            sourseInfo.CreateSubdirectory(newDirName);
        }

        public void Copy( string soursePath, string targetPath)
        {
            DirectoryInfo targetInfo = new DirectoryInfo(targetPath);
            if (!targetInfo.Exists) targetInfo.Create(); //если папка назначения не существует, создаем ее
            if (File.Exists(soursePath))
            {
                string newNameFile = Path.Combine(targetPath, Path.GetFileName(soursePath));
                File.Copy(soursePath, newNameFile);
            }
            if (Directory.Exists(soursePath))
            {
                DirectoryInfo sourseInfo = new DirectoryInfo(soursePath);
                if (sourseInfo.Exists) //если источник (папка, которую копируем) существует
                {
                    DirectoryInfo subD = targetInfo.CreateSubdirectory(sourseInfo.Name); //создаем в папке назначения подкаталог с соответствующим именем
                    FileInfo[] files = sourseInfo.GetFiles(); //проверяем, содержатся ли в папке-источнике файли
                    if (files.Length > 0)
                    {
                        foreach (FileInfo item in files) //если да, то копируем их
                        {
                            item.CopyTo(Path.Combine(subD.FullName, item.Name), true);
                        }
                    }
                    DirectoryInfo[] dir = sourseInfo.GetDirectories(); //проверяем, содержатся ли в папке-источнике подкаталоги
                    if (dir.Length > 0)
                    {
                        foreach (DirectoryInfo item in dir)//если да, то вызаваем для них рекурсивно копирование содержимого
                        {
                            Copy(item.FullName, Path.Combine(targetPath, sourseInfo.Name));
                        }
                    }
                }
            }
           
        }

        public void Move(string soursePath, string targetPath)
        {
            if (File.Exists(soursePath))
            {
                string newFileName = Path.Combine(targetPath, Path.GetFileName(soursePath));
                File.Move(soursePath, newFileName);
            }
            if (Directory.Exists(soursePath))
            {
                Directory.Move(soursePath, targetPath);
            }           
        }

        public void delete(string soursePath)
        {
            if (Directory.Exists(soursePath))
                Directory.Delete(soursePath, true);
            if (File.Exists(soursePath))
                File.Delete(soursePath);

        }

        public void createFile(string targetPath, string fileName)
        {
            DirectoryInfo targetInfo = new DirectoryInfo(targetPath);
            if (!targetInfo.Exists) targetInfo.Create();
            FileInfo newFile = new FileInfo(Path.Combine(targetPath, fileName)); 
            if (!newFile.Exists)
                using (StreamWriter sw = newFile.CreateText())
                {
                    string userText = Console.ReadLine();
                    sw.Write(userText);
                }
            else throw new ApplicationException($"Файл с указанным именем уже существует");
        }
        
        public void ReadFile(string filePath)
        {
            Console.WriteLine("\nДля управления просмотром используйте: PageDown - следующая страница текста; Alt-X - закрыть файл\n\nДля продоления нажмите любую клавишу...");
            Console.ReadKey(true);
            Console.Clear();            
            FileInfo fInfo = new FileInfo(filePath);
            int cnt = 0;
            using (StreamReader sr = new StreamReader(filePath, Encoding.Unicode))//fInfo.OpenText())            {
            { 
                while (true)
                {
                    int h = Console.WindowHeight;
                    string line;
                    while ((line = sr.ReadLine()) != null && cnt<h-1)
                    {
                        Console.WriteLine(line);
                        cnt++;
                    }
                    
                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.PageDown)  cnt = 0;                      
                   
                    if (cki.Key == ConsoleKey.DownArrow) cnt--;
                                     
                    if ((cki.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt && cki.Key == ConsoleKey.X) return;                   

                }
            }
        }
       
        

        public ResourseInfo[] GetResourseEntries(string path)
        {
            List<ResourseInfo> rList = new List<ResourseInfo>();           

            DirectoryInfo d_info = new DirectoryInfo(path);
            FileSystemInfo[] fsi = d_info.GetFileSystemInfos();
            foreach (var item in fsi)
            {
                ResourseInfo tmp = new ResourseInfo();
                tmp.Name = item.Name;
                tmp.FullName = item.FullName;
                tmp.DateCreate = item.CreationTime.Date.ToShortDateString();
                tmp.TimeCreate = item.CreationTime.TimeOfDay.ToString();
                
                switch (item.Attributes)
                {
                   case FileAttributes.Archive:
                        tmp.FAttr = "Архивный";                           
                        break;
                   case FileAttributes.Hidden:
                        tmp.FAttr = "Скрытый";
                        break;
                   case FileAttributes.System:
                        tmp.FAttr = "Системный";
                        break;
                  default: break;
                   
                }

               rList.Add(tmp);
            }
            return rList.ToArray();
        }

        public string[] GetDrivers()
        {
            return Directory.GetLogicalDrives();
        }

        public void openFile(string fileName)
        {
            Process p = Process.Start(fileName);
        }

        public void renameResourse(string soursePath, string newName)
        {
            string fullNewName;
            if (File.Exists(soursePath))
            {
                FileInfo fi = new FileInfo(soursePath);
                string dir = fi.DirectoryName;
                string r = fi.Extension;
                fullNewName = Path.Combine(dir, newName + fi.Extension);
                fi.MoveTo(fullNewName);
            }
            if (Directory.Exists(soursePath))
            {
                DirectoryInfo di = new DirectoryInfo(soursePath);
                fullNewName = Path.Combine(di.Parent.FullName, newName);
                di.MoveTo(fullNewName);
            }
        }
        
    }
}
