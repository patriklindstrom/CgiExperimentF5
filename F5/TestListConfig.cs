
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using F5.Tests;

namespace F5
{
    public interface ITestListConfig
    {
        TestList Tests { get; set; }
    }

    public class TestListConfig : ITestListConfig
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
            XDocument doc = XDocument.Load(ConfigFileName);
            IEnumerable<XElement> testList = doc.Root.Elements();
            foreach (var iAliveTest in testList)
            {
                var test = iAliveTest.Descendants().First();
                var foo = test.Name;
                if (test.Name == "DatabaseConnTest")
                {
                    Tests.Add(new DatabaseConnTest { ConnString = test.Element("ConnString").Value });
                }
                if (test.Name == "PingTest")
                {
                    Tests.Add(new PingTest { PingAddress = test.Element("PingAddress").Value });
                }
            }          
        }
        public void SaveToFile()
        {
            XmlSerializer x = new XmlSerializer(Tests.GetType(), "http://www.lcube.se");
            StreamWriter writer = new StreamWriter(ConfigFileName);
            x.Serialize(writer, Tests);
            writer.Flush();
            writer.Close();           
        }




    }
}