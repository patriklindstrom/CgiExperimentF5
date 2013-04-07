
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
            if (doc.Root != null)
            {
                IEnumerable<XElement> testList = doc.Root.Elements();
                foreach (var iAliveTest in testList)
                {
                    var test = iAliveTest.Descendants().First();
                    if (test.Name == "DatabaseConnTest")
                    {
                        var xElement = test.Element("ConnString");
                        if (xElement != null)
                            Tests.Add(new DatabaseConnTest { ConnString = xElement.Value });
                    }
                    if (test.Name == "PingTest")
                    {
                        var xElement = test.Element("PingAddress");
                        if (xElement != null)
                            Tests.Add(new PingTest { PingAddress = xElement.Value });
                    }
                }
            }
        }
        public void SaveToFile()
        {
            var x = new XmlSerializer(Tests.GetType(), "http://www.lcube.se/alivetest.xsd");
            var writer = new StreamWriter(ConfigFileName);
            x.Serialize(writer, Tests);
            writer.Flush();
            writer.Close();           
        }




    }
}