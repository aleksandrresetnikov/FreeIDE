using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeIDE.Common.Utils
{
    public static class DirectoryUtil
    {
        public static void GetLokationFolderProc(ref string val, int step = 0)
        {
            ProcDir(GetLokationFolder(), ref val, step);
        }

        public static void ProcDir(string path, ref string outVal, int step = 0)
        {
            outVal += StringUtil.Steping(step) + "DIR: " + path/*.Replace(@"C:\Users\user\source\repos\fastLPI engine\bin\Debug", "")*/ + "\n";

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = System.IO.Directory.GetDirectories(path);
            foreach (string subdirectory in subdirectoryEntries)
                ProcDir(subdirectory, ref outVal, step + 1);

            // Process the list of files found in the directory.
            string[] fileEntries = System.IO.Directory.GetFiles(path);
            foreach (string fileName in fileEntries)
                outVal += StringUtil.Steping(step + 1) + fileName.Replace(@"C:\Users\user\source\repos\fastLPI engine\bin\Debug", "") + "\n";
        }

        public static void MoveDir(string moveFromPath, string moveToPath) => System.IO.Directory.Move(moveFromPath, moveToPath);
        public static void CopyDir(string copyFromPath, string copyToPath)
        {
            System.IO.Directory.CreateDirectory(copyToPath);
            foreach (string s1 in System.IO.Directory.GetFiles(copyFromPath))
            {
                string s2 = copyToPath + "\\" + System.IO.Path.GetFileName(s1);
                System.IO.File.Copy(s1, s2);
            }
            foreach (string s in System.IO.Directory.GetDirectories(copyFromPath))
                CopyDir(s, copyToPath + "\\" + System.IO.Path.GetFileName(s));
        }
        public static long DirSize(System.IO.DirectoryInfo d)
        {
            long size = 0;

            // Add file sizes.
            System.IO.FileInfo[] fis = d.GetFiles();
            foreach (System.IO.FileInfo fi in fis)
                size += fi.Length;

            // Add subdirectory sizes.
            System.IO.DirectoryInfo[] dis = d.GetDirectories();
            foreach (System.IO.DirectoryInfo di in dis)
                size += DirSize(di);

            return size;
        }
        public static string GetTemporaryDirectory(string unname = "lpiTemp_")
        {
            return System.IO.Path.Combine(System.IO.Path.GetTempPath(), unname + System.IO.Path.GetRandomFileName().RemovePath());
        }
        public static string RemovePath(this string path)
        {
            return System.IO.Path.ChangeExtension(path, null);
        }
        public static void UntripFolder(string path)
        {
            if (System.IO.Directory.Exists(path))
                System.IO.Directory.Delete(path, true);
            System.IO.Directory.CreateDirectory(path);
        }
        public static string GetLokationFolder()
        {
            return new System.IO.FileInfo(System.Windows.Forms.Application.ExecutablePath).Directory.FullName;
        }
    }
}
