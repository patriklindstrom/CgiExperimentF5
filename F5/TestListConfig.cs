using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace F5
{
   
    public class TestListConfig
    {
        public TestList Tests { get; set; }
        public string ConfigFileName { get; set; }
        public TestListConfig(string  cFName)
        {
            ConfigFileName = cFName;
            Tests = new TestList();
        }

        public void ReadExtraTest()
        {
           // try
            //{
             // var xmlSerializer1 = new XmlSerializer(typeof(String));
            //var foo = typeof (TestList);
         //   Console.WriteLine(foo.ToString());
           // var xmlSerializer = new XmlSerializer(foo,"http://www.lcube.se");
           // var xmlSerializer = new XmlSerializer(Tests.GetType());

                       XDocument doc = XDocument.Load(@".\CGITest.xml");
            //var cnElement1 = doc.Elements("TestList");
            //var cnElement = cnElement1.Elements("IAliveTest");
            //var aLiveTests = cnElement.Elements("DatabaseConnTest");
            //           var dbTest = aLiveTests.Elements("ConnString").ElementAt(0);
            //           string cnStr = dbTest.Value;




               // var xmlReader = XmlReader.Create(new StreamReader(ConfigFileName));
                //XElement xelement = XElement.Load(xmlReader);
                       IEnumerable<XElement> databaseConnTests = doc.Elements().Where(n => n.Name == "DatabaseConnection");
                Console.WriteLine("databaseConnection :");
                foreach (var dbConn in databaseConnTests)
                {
                    Console.WriteLine(dbConn.Element("ConnString").Value);
                    Tests.Add(new DatabaseConnTest { ConnString = dbConn.Element("ConnString").Value });
                }
            //var dbTest = new DatabaseConnTest
            //    {
            //        ConnString =
            //            @"data source=Herkules\Dev;initial catalog=HaxitDB;integrated security=True;;Connection Timeout=5;"
            //    };
            var pingTest = new PingTest {PingAddress = "sunet.se"};

            Tests.Add(pingTest);

           
            //}
            // catch (Exception)
            // {
                //Console.WriteLine("</br>");
               // Console.WriteLine("Error in reading extratest config");
               // throw;
            // }

            
        }
        public void SaveToFile()
        {
            string path = ConfigFileName;
            //DatabaseConnTest dbT = new DatabaseConnTest { ConnString = "kalle" };
            //PingTest pT = new PingTest {PingAddress = "sunet.se"};
            //TestList aList = new TestList {dbT, pT};
            // Check if ConfigFileName exist. If so create other file. Do not overwrite.    

            XmlSerializer x = new XmlSerializer(Tests.GetType(), "http://www.lcube.se");
            StreamWriter writer = new StreamWriter(GetFileName(path));
            x.Serialize(writer, Tests);
            writer.Flush();
            writer.Close();
            
        }

        private string GetFileName(string suggestedFileName)
        {
            if (File.Exists(suggestedFileName))
            {
                return suggestedFileName+".xml";
                //string.Format(@"{0}.config", Guid.NewGuid());
                // return Path.GetTempFilename();
            }
            return suggestedFileName + ".xml";
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