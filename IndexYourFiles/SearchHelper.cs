using IndexYourFiles.Expands;
using IndexYourFiles.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexYourFiles
{
    class SearchHelper
    {
        #region Disk Directories
        public static IEnumerable<DriveInfoExpand> GetAllDriveInfo()
        {
            var allDrives = DriveInfo.GetDrives();
            DriveInfoExpand[] allDrivesExpand = new DriveInfoExpand[allDrives.Length];
            for (int i = 0; i < allDrives.Length; i++)
            {
                allDrivesExpand[i] = new DriveInfoExpand
                {
                    Drive = allDrives[i],
                };
            }
            foreach (var item in allDrivesExpand)
            {
                //Fixed hard disk
                //Removable USB
                if (item.Drive.IsReady)
                {
                    yield return item;
                }
            }
        }
        #endregion

        public static IEnumerable<DirectoryInfo> ListFolders(string path)
        {
            if (!Directory.Exists(path))
            {
                yield break;
            }
            DirectoryInfo _di = null ;
            var folders = Directory.GetDirectories(path);
            foreach (var item in folders)
            {
                try
                {
                    _di = new DirectoryInfo(item);
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex);
                    continue;
                }
                yield return _di;
            }
        }


        public static IEnumerable<FileInfo> ListFiles(string path)
        {
            if (!Directory.Exists(path))
            {
                yield break;
            }
            FileInfo _file = null;
            var folders = Directory.GetFiles(path);
            foreach (var item in folders)
            {
                try
                {
                    _file = new FileInfo(item);
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex);
                    continue;
                }
                yield return _file;
            }
        }

        public static Process OpenInFileExplorer(string path)
        {
            try
            {
                var psi = new ProcessStartInfo("Explorer.exe");

                psi.Arguments = $"/select,{path}";
                return Process.Start(psi);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }

        }


        public static List<DirectoryInfoExpand> SearchDirectories(string path, string name)
        {
            if (!Directory.Exists(path))
            {
                return null;
            }

            try
            {

                var ds = Directory.GetDirectories(path);
                List< DirectoryInfoExpand> Directories = new List<DirectoryInfoExpand>();
                for (int i = 0; i < ds.Length; i++)
                {
                    if (ds[i].Substring(ds[i].LastIndexOf('\\')).ToLower().Contains(name.ToLower()))
                    {
                        Directories.Add(new DirectoryInfoExpand(name, new DirectoryInfo(ds[i])));
                    }
                    
                }
                return Directories;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

        public static List<FileInfo> SearchFiles(string path, string name)
        {
            if (!Directory.Exists(path))
            {
                return null;
            }

            try
            {
                var ds = Directory.GetFiles(path);
                List<FileInfo>  fileInfos  = new List<FileInfo>();
                foreach (var item in ds)
                {
                    if (item.Substring(item.LastIndexOf('\\')).ToLower().Contains(name.ToLower()))
                    {
                        string item1 = item;
                        fileInfos.Add(new FileInfo(item1));
                    }
                }
                return fileInfos;
            }
            catch (Exception ex)
            {
                _ = LogHelper.LogError(ex);

                return null;
            }
        }



    }
}


//2021.8.6 中晚、2021.8.7 早中晚、8.8早、8.9早、8.10早
//5+5+5+ 22 + 17
//    49
//        59



/*
C 99 2

99!
/
97!

/

2!


9702
9702/2=4851


解法1
4!*2 = 4*3*2*1 = 24 = 48

3! = 3*2*1 = 6

4!*2 - 3! = 42



解法2（暴力）
||1|2|3||

count! * 2 + 1

6 + 1 = 7

7 * 3! = 42


w w 1 2 3
w 1 w 2 3
w 1 2 w 3
w 1 2 3 w
1 2 3 w w
1 2 w 3 w
1 w 2 3 w


解法3（排除法）
5!/2 = 5*4*3*2*1 = 120/2 = 60


p3 2

3!
/(3-2)! = 6

3!/2 = 3

18

60-18 = 42



 
 */


