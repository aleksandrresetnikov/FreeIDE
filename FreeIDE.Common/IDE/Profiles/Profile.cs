using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FreeIDE.Common.IDE.Profiles
{
    public class Profile
    {
        private XDocument ProfileInfoXDocument;

        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Language { get; set; }
        public string ProfileType { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }

        public string ProfileInfoXDocumentPath => $@"{Path}\profile_info.xml";
        public string ProfileLogoPath => $@"{Path}\profile_logo.png";
        public string ProfileInitScriptPath => $@"{Path}\init.py";

        public Profile (string Path)
        {
            this.Path = Path;

            this.ProfileInfoXDocument = XDocument.Load(ProfileInfoXDocumentPath);
            this.Name = this.ProfileInfoXDocument.Root.Element("Name").Value;
            this.Description = this.ProfileInfoXDocument.Root.Element("Description").Value;
            this.Language = this.ProfileInfoXDocument.Root.Element("Language").Value;
            this.ProfileType = this.ProfileInfoXDocument.Root.Element("Type").Value;
            this.Platform = this.ProfileInfoXDocument.Root.Element("Platform").Value;
            this.Version = this.ProfileInfoXDocument.Root.Element("Version").Value;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Profile:");
            Console.WriteLine($"\tName: {this.Name}");
            Console.WriteLine($"\tDescription: {this.Description}");
            Console.WriteLine($"\tLanguage: {this.Language}");
            Console.WriteLine($"\tProfileType: {this.ProfileType}");
            Console.WriteLine($"\tPlatform: {this.Platform}");
            Console.WriteLine($"\tVersion: {this.Version}");
        }
    }
}
