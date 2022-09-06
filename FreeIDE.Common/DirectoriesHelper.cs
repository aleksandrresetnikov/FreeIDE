using System;
using System.IO;

namespace FreeIDE.Common
{
    public class DirectoriesHelper
    {
        public static string PathToCurrentUserDirectory => Environment.GetEnvironmentVariable("USERPROFILE");
        public static DirectoryInfo CurrentUserDirectory => new DirectoryInfo(PathToCurrentUserDirectory);

        public static string PathToDocumentsDirectory => $@"{PathToCurrentUserDirectory}\Documents";
        public static DirectoryInfo DocumentsDirectory => new DirectoryInfo(PathToDocumentsDirectory);

        public static string PathToSoftDocumentsDirectory => $@"{PathToDocumentsDirectory}\FreeIDE";
        public static DirectoryInfo SoftDocumentsDirectory => new DirectoryInfo(PathToSoftDocumentsDirectory);

        public static readonly string[] Directories =
        {
            PathToSoftDocumentsDirectory
        };

        public static void CheckDirectories()
        {
            foreach (string directoryPathItem in Directories)
                if (!Directory.Exists(directoryPathItem))
                    Directory.CreateDirectory(directoryPathItem);
        }
    }
}
