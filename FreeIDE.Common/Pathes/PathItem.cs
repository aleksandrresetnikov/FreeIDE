using System;
using System.IO;

using FreeIDE.Common.API;

namespace FreeIDE.Common.Pathes
{
    public class PathItem : IDisposable, ICloneable, IPathCloneable, IPathItemFileFunctions
    {
        public string Path { get; set; }

        public FileInfo GetFileInfo => new FileInfo(this.Path);
        public DirectoryInfo GetDirectoryInfo => new DirectoryInfo(this.Path);
        public StreamReader CreateStreamReader => new StreamReader(this.Path);
        public StreamWriter CreateStreamWriter => new StreamWriter(this.Path);
        public string GetFileExtension => this.IsFile ? this.GetFileInfo.Extension : null;

        public bool IsDirectory => Directory.Exists(this.Path);
        public bool IsFile => File.Exists(this.Path);
        public bool IsExists => this.IsDirectory || this.IsFile;

        public PathItem() => this.Path = null;
        public PathItem(string Path) => this.Path = Path;
        public PathItem(FileInfo Path) => this.Path = Path.FullName;
        public PathItem(DirectoryInfo Path) => this.Path = Path.FullName;

        public override string ToString() => this.Path;
        public void Dispose() => GC.SuppressFinalize(this);
        public object Clone() => new PathItem(this.Path);
        public PathItem ClonePath() => this.Clone() as PathItem;

        public void Delete()
        {
            if (this.IsFile) File.Delete(this.Path);
            else if (this.IsDirectory) Directory.Delete(this.Path, true);
        }
        public void MoveTo(string pathTo)
        {
            if (this.IsFile) File.Move(this.Path, pathTo);
            else if (this.IsDirectory) Directory.Move(this.Path, pathTo);
            this.Path = pathTo;
        }
        public void CreateFile()
        {
            File.Create(this.Path);
        }
        public void CreateDirectory()
        {
            Directory.CreateDirectory(this.Path);
        }
    }
}
