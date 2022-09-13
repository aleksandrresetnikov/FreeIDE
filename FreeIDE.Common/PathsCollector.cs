using System;
using System.Collections.Generic;
using System.IO;

using FreeIDE.Common.API;

namespace FreeIDE.Common
{
    public class PathsCollector : List<string>, IPathsCollector, IDisposable
    {
        public List<FileInfo> GetFileInfos()
        {
            List<FileInfo> outputValue = new List<FileInfo>();

            foreach (string path in this)
                if (File.Exists(path))
                    outputValue.Add(new FileInfo(path));

            return outputValue;
        }

        public List<DirectoryInfo> GetDirectoryInfos()
        {
            List<DirectoryInfo> outputValue = new List<DirectoryInfo>();

            foreach (string path in this)
                if (Directory.Exists(path))
                    outputValue.Add(new DirectoryInfo(path));

            return outputValue;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
