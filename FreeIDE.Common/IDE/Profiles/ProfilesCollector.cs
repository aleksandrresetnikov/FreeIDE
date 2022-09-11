using System.Collections.Generic;

namespace FreeIDE.Common.IDE.Profiles
{
    public class ProfilesCollector : List<Profile>
    { 
        public void PrintInfo()
        {
            foreach (var profile in this)
                profile.PrintInfo();
        }
    }
}
