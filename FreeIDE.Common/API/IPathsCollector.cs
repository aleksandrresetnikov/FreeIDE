namespace FreeIDE.Common.API
{
    public interface IPathsCollector
    {
        System.Collections.Generic.List<System.IO.FileInfo> GetFileInfos();
        System.Collections.Generic.List<System.IO.DirectoryInfo> GetDirectoryInfos();
    }
}
