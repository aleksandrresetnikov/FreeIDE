using System;
using System.IO;

using FreeIDE.Common.Utils;

namespace FreeIDE.Common
{
    public class Logger
    {
        public static readonly string LogPath = $@"{DirectoryUtil.GetLokationFolder()}\Logs";
        public static readonly string LogExtension = ".log";

        public static void Start()
        {
            if (!new DirectoryInfo(LogPath).Exists)
                Directory.CreateDirectory(LogPath);

            LogTasker.Init();
        }

        public static void AddLog(string path, string text, bool drawDate = false)
        {
            try
            {
                LogTasker.AddInvoke(() =>
                {
                    File.AppendAllText(LogPath + path + LogExtension, (drawDate ? LogUtil.GetDumpDate() : "") + text + Environment.NewLine);
                });

                LogTasker.AddInvoke(GetAddLogWriteFile(path, text, drawDate));
            }
            catch (Exception ex)
            {
                ConsoleOutput.WriteLine($"AddLog(\n\tpath = {path}, \n\ttext = {text}, \n\tdrawDate = {drawDate})\n" +
                    $"Exception:\n\t{ex.StackTrace}");
            }
        }

        public static void CreateLog(string path, string text, bool drawDate = false)
        {
            try
            {
                LogTasker.AddInvoke(GetCreateLogWriteFile(path, text, drawDate));
            }
            catch (Exception ex)
            {
                ConsoleOutput.WriteLine($"CreateLog(\n\tpath = {path}, \n\ttext = {text}, \n\tdrawDate = {drawDate})\n" +
                    $"Exception:\n\t{ex.StackTrace}");
            }
        }

        public static void AddNewLog(string path, string text, bool drawDate = false)
        {
            try
            {
                string name = new FileInfo(LogPath + path).Name;
                LogTasker.AddInvoke(GetAddNewLogWriteFile(name, text, drawDate));
            }
            catch (Exception ex)
            {
                ConsoleOutput.WriteLine($"AddNewLog(\n\tpath = {path}, \n\ttext = {text}, \n\tdrawDate = {drawDate})\n" +
                    $"Exception:\n\t{ex.StackTrace}");
            }
        }

        private static Action GetAddLogWriteFile(string name, string text, bool drawDate = false)
        {
            return (Action)(() => {
                AddLogWriteFile(name, text, drawDate);
            });
        }

        private static Action GetCreateLogWriteFile(string name, string text, bool drawDate = false)
        {
            return (Action)(() => {
                CreateLogWriteFile(name, text, drawDate);
            });
        }

        private static Action GetAddNewLogWriteFile(string name, string text, bool drawDate = false)
        {
            return (Action)(() => {
                AddNewLogWriteFile(name, text, drawDate);
            });
        }

        private static void AddLogWriteFile(string path, string text, bool drawDate = false)
        {
            File.AppendAllText(LogPath + path + LogExtension, (drawDate ? LogUtil.GetDumpDate() : "") + text + Environment.NewLine);
        }

        private static void CreateLogWriteFile(string path, string text, bool drawDate = false)
        {
            File.WriteAllText(LogPath + path + LogExtension, (drawDate ? LogUtil.GetDumpDate() : "") + text + Environment.NewLine);
        }

        private static void AddNewLogWriteFile(string name, string text, bool drawDate = false)
        {
            File.WriteAllText(LogPath + @"\" + LogUtil.GetNewDumpName(name) + LogExtension, (drawDate ? LogUtil.GetDumpDate() : "") + text);
        }
    }
}
