using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace F5
{
    internal class TestListConfig
    {
        public TestList Tests { get; set; }
        public string ConfigFileName { get; set; }
        public TestListConfig(string  cFName)
        {
            ConfigFileName = cFName;
            Tests = new TestList();
        }

        public void SaveToFile()
        {
            string path = ConfigFileName;
            // Check if ConfigFileName exist. If so create other file. Do not overwrite.            
            XmlSerializer x = new XmlSerializer(Tests.GetType());
            StreamWriter writer = new StreamWriter(GetFileName(path));
            x.Serialize(writer, Tests); 
        }

        private string GetFileName(string suggestedFileName)
        {
            if (File.Exists(suggestedFileName))
            {
                return string.Format(@"{0}.config", Guid.NewGuid());
               // return Path.GetTempFilename();
            }
            return suggestedFileName;
        }

        static FileStream CreateFileWithUniqueName(string folder, string fileName,
        int maxAttempts = 1024)
        {
            // get filename base and extension
            var fileBase = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);
            // build hash set of filenames for performance
            var files = new HashSet<string>(Directory.GetFiles(folder));

            for (var index = 0; index < maxAttempts; index++)
            {
                // first try with the original filename, else try incrementally adding an index
                var name = (index == 0)
                    ? fileName
                    : String.Format("{0} ({1}){2}", fileBase, index, ext);

                // check if exists
                var fullPath = Path.Combine(folder, name);
                if (files.Contains(fullPath))
                    continue;

                // try to open the stream
                try
                {
                    return new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write);
                }
                catch (DirectoryNotFoundException) { throw; }
                catch (DriveNotFoundException) { throw; }
                catch (IOException) { } // ignore this and try the next filename
            }

            throw new Exception("Could not create unique filename in " + maxAttempts + " attempts");
        }

    }
}