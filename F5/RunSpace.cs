using System;
using System.Collections.Generic;
using System.IO;


namespace F5
{
    public interface IRunSpace
    {
        string AppPoolId { get; set; }
        bool InIIS { get; set; }
        string ConfigFile { get; set; }
        string QueryString { get; }
    }

    class RunSpace : IRunSpace
    {
        public string AppPoolId { get; set; }

        public bool InIIS { get; set; }

        public string ConfigFile { get; set; }
        public string QueryString { get; private set; }

        public RunSpace()
        {
            AppPoolId = Environment.GetEnvironmentVariable("APP_POOL_ID");
            InIIS = !String.IsNullOrEmpty(AppPoolId);
            ConfigFile = @".\" + AppPoolId + ".xml";
            QueryString = Environment.GetEnvironmentVariable("QUERY_STRING");
        }

        static FileStream CreateFileWithUniqueName(string folder, string fileName,int maxAttempts = 1024)
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
