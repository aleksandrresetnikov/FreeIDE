using System.IO;

namespace FreeIDE.Common.Utils
{
    public static class CommonUtils
    {
        public static FileInfo ReMakeFileInfo(this FileInfo SourseFileInfo)
        {
            return new FileInfo(SourseFileInfo.FullName);
        }

        public static DirectoryInfo ReMakeDirectoryInfo(this DirectoryInfo SourseDirectoryInfo)
        {
            return new DirectoryInfo(SourseDirectoryInfo.FullName);
        }
    }
}
