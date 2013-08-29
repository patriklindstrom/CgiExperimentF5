#define TRACE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Diagnostics;
using F5.Tests;

//TraceInfo
/*
 The first digit can detail the general class: 1xxx can be used for 'Start' operations, 2xxx for normal behaviour, 3xxx for activity tracing, 4xxx for warnings, 5xxx for errors, 8xxx for 'Stop' operations, 9xxx for fatal errors, etc.
The second digit can detail the area, e.g. 21xx for database information (41xx for database warnings, 51xx for database errors), 22xx for calculation mode (42xx for calculation warnings, etc), 23xx for another module, etc.
 */

namespace F5
{
    public class Program
    {
        static TraceSource _mainTrace = new TraceSource("MainLog");
        static TraceSource _configTrace = new TraceSource("ConfigLog");
        static TraceSource _cgiTrace = new TraceSource("cgilog");
        private static void Main(string[] args)
        {
            // Trace start
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            Trace.CorrelationManager.StartLogicalOperation("Main");
            _mainTrace.TraceEvent(TraceEventType.Start, 1000, "Program start.");
            _mainTrace.TraceEvent(TraceEventType.Information, 1010, "Get RunSpaceinfo.");
            var space = new RunSpace {Args = args};
            _mainTrace.TraceEvent(TraceEventType.Information, 1020, "Get TestListConfig.");
            var allTestToDo = new TestListConfig(space);
           TestController testController;
           _mainTrace.TraceEvent(TraceEventType.Information, 1030, "Decide Program mode.");
           RunRightModeOfProgram(space, allTestToDo, out testController);
           _mainTrace.TraceEvent(TraceEventType.Stop, 1090, "Program Stop.");
           Trace.CorrelationManager.StopLogicalOperation();
        }


        public static void RunRightModeOfProgram(IRunSpace space, ITestListConfig allTestToDo,out TestController testController)
        {
            testController = null;
            if (space.InIIS)
            {

                allTestToDo.GetTestList();
                testController = new TestController();
                _mainTrace.TraceEvent(TraceEventType.Information, 1031, "Be a CGI program.");
                BeCgi(allTestToDo, testController);
            }
            else
            {
                _mainTrace.TraceEvent(TraceEventType.Information, 1032, "Be a CGI program.");
                BeCmdLine(space);
            }
        }

        public static void BeCmdLine(IRunSpace space)
        {
            if (space.Args.Length == 0)
            {
                Console.WriteLine("Do you want to create AliveTestConfig File? [y]");
                string answere = Console.ReadLine();
                if (String.Compare(answere, "y", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _mainTrace.TraceEvent(TraceEventType.Information, 1033, "Lets Do a commandLineSession");
                    CreateConfigFromConsoleMain(space);
                }
            }
        }

        public static void BeCgi(ITestListConfig allTestToDo, TestController testController)
        {
            Console.WriteLine("\r\n\r\n");
            Console.WriteLine("<html>");

            testController.DoAllTest(allTestToDo.Tests);

            if (testController.AllTestGood)
            {
                Console.WriteLine("<div style='color:green;'>");
                Console.WriteLine("Alive");
                Console.WriteLine("</div>");
                Console.WriteLine("<div style='color:green;'>");
                Console.WriteLine(DateTime.Now.ToString("O"));
                Console.WriteLine("</div>");
            }
            else
            {
                Console.WriteLine("<div style='color:red;'>");
                Console.WriteLine("Dead");
                Console.WriteLine("</div>");
                Console.WriteLine("<div style='color:red;'>");
                Console.WriteLine(DateTime.Now.ToString("O"));
                Console.WriteLine("</div>");
            }
            Console.WriteLine("</html>");
        }

        private static void CreateConfigFromConsoleMain(IRunSpace space)
        {
             _configTrace.TraceEvent(TraceEventType.Start, 2230, "Enter CreateConfigFromConsoleMain with space {0}.",space.ToString());
            Console.WriteLine("Enter Name of Application pool?");
            string applPool = Console.ReadLine();
            if (!String.IsNullOrEmpty(applPool))
            {
                space.AppPoolId = applPool;
                space.ConfigFile = applPool + ".xml"; 
                var testListConfig = new TestListConfig(space);
                string choice = "0";
                while (choice != "-1")
                {
                    Console.WriteLine("For help enter 0");
                    Console.WriteLine("Create SQL Server DB enter number 1");
                    Console.WriteLine("Create TCP/IP Ping enter number 2");
                    Console.WriteLine("Done/Save config file press Enter");
                    choice = Console.ReadLine();
                    IAliveTest test = null;
                    _configTrace.TraceEvent(TraceEventType.Information, 2231, "Choice of config {0}.", choice);
                    switch (choice)
                    {
                        case "0":
                            Console.WriteLine("No help for you");
                            break;
                        case "1":
                            Console.WriteLine("New DB Test");
                            test = new DatabaseConnTest();
                            break;
                        case "2":
                            Console.WriteLine("New Ping Test");
                            test = new PingTest();
                            break;

                        default:
                            Console.WriteLine("No more tests");
                            choice = "-1";
                            break;
                    }
                    if (test != null)
                    {
                        _configTrace.TraceEvent(TraceEventType.Information, 2231, "CreateConfigFromConsole.");
                        _configTrace.TraceEvent(TraceEventType.Information, 2232, "Create Test of type: {0}",test.GetType().ToString());
                        test.CreateConfigFromConsole();
                        
                        testListConfig.Tests.Add(test);
                        _configTrace.TraceEvent(TraceEventType.Information, 2233, "Added Test: {0}", test.GetType().ToString());
                    }

                }
                testListConfig.SaveToFile();
                Console.WriteLine("Press Enter to Exit");
                choice = Console.ReadLine();
                _configTrace.TraceEvent(TraceEventType.Stop, 2239, "Stop Creating configfile.");

            }
        }


        private class RunSpaceTest : IRunSpace
        {
            public RunSpaceTest()
            {
                AppPoolId = "CGITest";
                InIIS = true;
                ConfigFile = @".\CGITest";
            }

            public string AppPoolId { get; set; }
            public bool InIIS { get; set; }
            public string ConfigFile { get; set; }
            public string QueryString { get; set; }
            public string[] Args { get; private set; }
        }
    }
}
