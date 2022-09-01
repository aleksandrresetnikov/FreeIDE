using System;

namespace FreeIDE.Common.Utils
{
    public static class LogUtil
    {
        public static string GetDumpDate()
        {
            return $"[{DateTime.Now.ToString()}] - ";
        }
        public static string GetNewDumpName(string name)
        {
            string path = Logger.LogPath;
            int value = 1;

            string newName = name;
            while (true)
            {
                newName = name + StringUtil.GetFixNum(value);
                if (!new System.IO.FileInfo(path + $@"\{newName}.log").Exists) break;
                value++;
            }

            return newName;
        }
    }
}
