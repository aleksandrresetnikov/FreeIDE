using System.IO;

using FreeIDE.Common.Utils;

namespace FreeIDE.Common.IDE.Profiles
{
    public class ProfilesLoader
    {
        public static readonly string ProfilesFolder = $@"{DirectoryUtil.GetLokationFolder()}\Data\Profiles";
        public static readonly string[] ProfilesFolderFileItems = {
            "init.py", "profile_info.xml", "profile_logo.png"
        };

        public static ProfilesCollector GetProfiles()
        {
            ProfilesCollector outputValue = new ProfilesCollector();

            foreach (string directoryItem in Directory.GetDirectories(ProfilesFolder))
            {
                bool searchSuccessState = true;
                foreach (string fileItem in ProfilesFolderFileItems)
                    if (!File.Exists($@"{directoryItem}\{fileItem}")) { searchSuccessState = false; break; }

                if (searchSuccessState) outputValue.Add(new Profile(directoryItem));
            }

            return outputValue;
        }
    }
}
