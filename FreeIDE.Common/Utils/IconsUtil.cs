using System.Collections.Generic;

namespace FreeIDE.Common.Utils
{
    public class IconsUtil
    {
        public static readonly Dictionary<string, int> FileTypesImagesDictionary = new Dictionary<string, int>
        {
            { ".txt", 3 },
            { ".md", 3 },
            { ".log", 3 },
            { ".xml", 4 },
            { ".png", 5 },
            { ".ico", 5 },
            { ".bmp", 5 },
            { ".jpg", 5 },
            { ".jpeg", 5 },
            { ".svg", 5 },
            { ".ai", 5 },
            { ".json", 6 },
            { ".js", 6 },
            { ".ts", 6 },
            { ".ps1", 6 },
            { ".bat", 6 },
            { ".xaml", 6 },
            { ".mf", 7 },
            { ".config", 7 },
            { ".settings", 7 },
            { ".html", 8 },
            { ".css", 9 },
            { ".cpp", 10 },
            { ".h", 10 },
            { ".ino", 10 },
            { ".cs", 11 },
            { ".csharp", 11 },
            { ".fs", 12 },
            { ".fsharp", 12 },
            { ".fsi", 12 },
            { ".ml", 12 },
            { ".mli", 12 },
            { ".fsx", 12 },
            { ".fsscript", 12 },
            { ".vb", 13 },
            { ".py", 14 },
            { ".ttf", 15 },
            { ".otf", 15 },
            { ".gtf", 15 },
            { ".vfb", 15 },
            { ".obj", 16 },
            { ".bin", 16 },
            { ".data", 16 },
            { ".class", 16 },
            { ".jar", 17 },
            { ".dll", 17 },
            { ".exe", 18 },
            { ".mp3", 19 },
            { ".wav", 19 },
        };

        public static int GetImageIndexMini(string fileType)
        {
            if (!FileTypesImagesDictionary.ContainsKey(fileType.ToLower())) return 2;
            else return FileTypesImagesDictionary[fileType.ToLower()];
        }
    }
}
