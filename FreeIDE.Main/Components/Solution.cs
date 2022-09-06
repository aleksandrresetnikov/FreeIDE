using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FreeIDE.Common.Utils;

namespace FreeIDE.Components
{
    internal class Solution
    {
        // Solution Folder
        public DirectoryInfo SolutionDirectory
        { get; private protected set; }

        // Solution Name
        public string SolutionName
        { get; private protected set; }

        // SolutionFile (Extension: .fis - 'Free IDE Solution')
        public FileInfo SolutionFile
        {
            get
            {
                return new FileInfo($@"{SolutionDirectory.FullName}\{SolutionName.RemoveExtension()}.fis");
            }
            set
            {
                this.SolutionDirectory = value.Directory.ReMakeDirectoryInfo();
                this.SolutionName = value.Name;
            }
        }

        public Solution(FileInfo SolutionFile)
        {
            this.SolutionFile = SolutionFile.ReMakeFileInfo();
        }
    }
}
