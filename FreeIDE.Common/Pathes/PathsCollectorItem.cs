using System;

namespace FreeIDE.Common.Pathes
{
    public class PathsCollectorItem : IDisposable
    {
        public PathItem PathItemFrom { get; set; }
        public PathItem PathItemTo { get; set; }

        public PathsCollectorItem(){ }
        public PathsCollectorItem(PathItem PathItemFrom) => this.PathItemFrom = PathItemFrom;
        public PathsCollectorItem(PathItem PathItemFrom, PathItem PathItemTo) : this(PathItemFrom) => this.PathItemTo = PathItemTo;

        public override string ToString() => this.PathItemFrom.ToString();
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
