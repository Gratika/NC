using System;
using System.Collections.Generic;
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

        public void CopyDirectory( string soursePath, string targetPath)
        {
            DirectoryInfo targetInfo = new DirectoryInfo(targetPath);
            if (!targetInfo.Exists) targetInfo.Create(); //если папка назначения не существует, создаем ее
            DirectoryInfo sourseInfo = new DirectoryInfo(soursePath);
            if (sourseInfo.Exists) //если источник (папка, которую копируем) существует
            {
                targetInfo.CreateSubdirectory(sourseInfo.Name); //создаем в папке назначения подкаталог с соответствующим именем
                FileInfo[] files = sourseInfo.GetFiles(); //проверяем, содержатся ли в папке-источнике файли
                if (files.Length > 0)
                {
                    foreach(FileInfo item in files) //если да, то копируем их
                    {
                        item.CopyTo(Path.Combine(targetInfo.FullName, item.Name), true);
                    }
                }
                DirectoryInfo[] dir = sourseInfo.GetDirectories(); //проверяем, содержатся ли в папке-источнике подкаталоги
                if (dir.Length > 0)
                {
                    foreach (DirectoryInfo item in dir)//если да, то вызаваем для них рекурсивно копирование содержимого
                    {
                        CopyDirectory(item.FullName, Path.Combine(targetPath, sourseInfo.Name));
                    }
                }
            }
        }

        public void moveDirectory(string soursePath, string targetPath)
        {
            DirectoryInfo sourseInfo = new DirectoryInfo(soursePath);
            if (!sourseInfo.Exists && Directory.Exists(targetPath) == false)
            {
                sourseInfo.MoveTo(targetPath);
            }
            
        }

        public void deleteDirectory(string dirPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            dirInfo.Delete(true);

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
        
        public string openFile(string filePath)
        {
            FileInfo fInfo = new FileInfo(filePath);
            using (StreamReader sr = fInfo.OpenText())
            {
               string fileText = sr.ReadToEnd();
               return fileText;

            }
        }
        public void deleteFile(string filePath)
        {
            File.Delete(filePath);
        }
        public void moveFile(string filePath, string targetFile)
        {
            FileInfo fInso = new FileInfo(filePath);
            fInso.MoveTo(targetFile);

        }

        public ResourseInfo[] GetResourseEntries(string path, bool withAttributes)
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
    }
}
