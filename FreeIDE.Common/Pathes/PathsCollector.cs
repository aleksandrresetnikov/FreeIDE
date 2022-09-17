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

        public void AddDictionary(Dictionary<string, string> valuePairs)
        {
            foreach (KeyValuePair<string, string> pair in valuePairs)
                this.Add(new PathsCollectorItem(new PathItem(pair.Key), new PathItem(pair.Value)));
        }

        public void AddPathsCollector(PathsCollector pathsCollector)
        {
            foreach (PathsCollectorItem pathItem in pathsCollector)
                this.Add(pathItem.Clone() as PathsCollectorItem);
        }

        public void Dispose()
        {
            this.Dispose();
        }

        public static PathsCollector Parse(Dictionary<string, string> valuePairs)
        {
            PathsCollector outputValue = new PathsCollector();

            foreach (KeyValuePair<string, string> pair in valuePairs)
                outputValue.Add(new PathsCollectorItem(new PathItem(pair.Key), new PathItem(pair.Value)));

            return outputValue;
        }
    }
}
