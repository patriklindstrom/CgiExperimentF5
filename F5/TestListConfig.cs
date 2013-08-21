
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        void GetTestList();
    }

    public class TestListConfig : ITestListConfig
    {
        static TraceSource _mainTrace = new TraceSource("MainLog");
        public TestList Tests { get; set; }
        public string ConfigFileName { get; set; }
        public TestListConfig(IRunSpace  runSpace)
        {
            ConfigFileName = runSpace.ConfigFile;
            _mainTrace.TraceEvent(TraceEventType.Information, 1021, "ConfigFileName {0} ", ConfigFileName);
            Tests = new TestList();          
        }

        public void GetTestList()
        {
            if (File.Exists(ConfigFileName))
            {
                XDocument doc = XDocument.Load(ConfigFileName);
                if (doc.Root != null)
                {
                    IEnumerable<XElement> testList = doc.Root.Elements();
                    _mainTrace.TraceEvent(TraceEventType.Information, 1022, "Start Adding testcases to testlist");
                    foreach (var iAliveTest in testList)
                    {
                        var test = iAliveTest.Descendants().First();
                        if (test.Name == "DatabaseConnTest")
                        {
                            var xElement = test.Element("ConnString");
                            if (xElement != null)
                            {
                                Tests.Add(new DatabaseConnTest {ConnString = xElement.Value});
                                _mainTrace.TraceEvent(TraceEventType.Information, 1021, "Add ConnString {0} ",
                                    xElement.Value);
                            }
                        }
                        if (test.Name == "PingTest")
                        {
                            var xElement = test.Element("PingAddress");
                            if (xElement != null)
                            {
                                Tests.Add(new PingTest {PingAddress = xElement.Value});
                                _mainTrace.TraceEvent(TraceEventType.Information, 1022, "Add PingAddress {0} ",
                                    xElement.Value);
                            }
                        }
                    }
                    _mainTrace.TraceEvent(TraceEventType.Information, 1023, "Added number of test {0} ",
                        Tests.Count);
                }
            }
            else
            {
                _mainTrace.TraceEvent(TraceEventType.Information, 1024, "Cant find test configfile {0} ",
                       ConfigFileName);
            }
        }
        public void SaveToFile()
        {
            try
            {
                _mainTrace.TraceEvent(TraceEventType.Information, 2210, "Save Config file to disk");
                var x = new XmlSerializer(Tests.GetType(), "http://www.lcube.se/alivetest.xsd");
                Console.WriteLine("Give Path to file");
                string pathString = Console.ReadLine();
                //ConfigFileName = pathString + "\\" + ConfigFileName;
                if (pathString != null) ConfigFileName = Path.Combine(pathString, ConfigFileName);
                _mainTrace.TraceEvent(TraceEventType.Information, 2211, "Save Config file to disk: {0}", ConfigFileName);
                Console.WriteLine("outputpath {0}", ConfigFileName);
                var writer = new StreamWriter(ConfigFileName);
                x.Serialize(writer, Tests);
                writer.Flush();
                writer.Close();
                _mainTrace.TraceEvent(TraceEventType.Information, 2212, "Saved Config file to disk: {0}", ConfigFileName);
            }
            catch (Exception e)
            {
                string  errorMsg = "Could not Save XML Config file to disk. ErrorMessage: " + e.Message;
                _mainTrace.TraceEvent(TraceEventType.Critical, 9001, "Could not Save XML Config file to disk. ErrorMessage: {0}" , e.Message);
                _mainTrace.Flush();
                Console.WriteLine(errorMsg);
                Console.Beep();
                Console.WriteLine("Try other Path? [y]");

              var  answer = Console.ReadLine();
                if (answer=="y")
                {
                    SaveToFile();
                }
            }
          
        }




    }
}