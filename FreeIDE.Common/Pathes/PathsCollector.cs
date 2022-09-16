using System;
using System.Collections.Generic;
using System.IO;

using FreeIDE.Common.API;

namespace FreeIDE.Common.Pathes
{
    public class PathsCollector : List<PathsCollectorItem>, IPathsCollector, IDisposable
    {
        public List<FileInfo> GetFileInfos()
        {
            List<FileInfo> outputValue = new List<FileInfo>();

            foreach (PathItem pathItem in this.GetPathItems())
                if (pathItem.IsFile)
                    outputValue.Add(pathItem.GetFileInfo);

            return outputValue;
        }

        public List<DirectoryInfo> GetDirectoryInfos()
        {
            List<DirectoryInfo> outputValue = new List<DirectoryInfo>();

            foreach (PathItem pathItem in this.GetPathItems())
                if (pathItem.IsDirectory)
                    outputValue.Add(pathItem.GetDirectoryInfo);

            return outputValue;
        }

        public List<PathItem> GetPathItems()
        {
            List<PathItem> outputValue = new List<PathItem>();

            foreach (PathsCollectorItem pathsCollectorItem in this)
                outputValue.Add(pathsCollectorItem.PathItemFrom);

            return outputValue;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
