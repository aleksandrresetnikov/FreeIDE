using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeIDE.Common.Utils
{
    public class IconsUtil
    {
        public static int GetImageIndexMini(string fileType)
        {
            switch (fileType.ToLower())
            {
                case ".xml": return 2;
                default: return 1;
            }
        }
    }
}
