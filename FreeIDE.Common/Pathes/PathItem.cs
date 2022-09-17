﻿using System;
using System.IO;

namespace FreeIDE.Common.Pathes
{
    public class PathItem : IDisposable
    {
        public string Path { get; set; }

        public FileInfo GetFileInfo => new FileInfo(this.Path);
        public DirectoryInfo GetDirectoryInfo => new DirectoryInfo(this.Path);
        public StreamReader CreateStreamReader => new StreamReader(this.Path);
        public StreamWriter CreateStreamWriter => new StreamWriter(this.Path);

        public bool IsDirectory => Directory.Exists(this.Path);
        public bool IsFile => File.Exists(this.Path);
        public bool IsExists => this.IsDirectory || this.IsFile;

        public PathItem() => this.Path = null;
        public PathItem(string Path) => this.Path = Path;
        public PathItem(FileInfo Path) => this.Path = Path.FullName;
        public PathItem(DirectoryInfo Path) => this.Path = Path.FullName;

        public override string ToString() => this.Path;
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
