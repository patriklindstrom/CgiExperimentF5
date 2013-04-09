using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using F5.Tests;


namespace F5
{
    public class Program
    {

        private static void Main(string[] args)
        {
            var space = new RunSpace {Args = args};
            var allTestToDo = new TestListConfig(space);
           TestController testController;
           RunRightModeOfProgram(space, allTestToDo, out testController);           
        }


        public static void RunRightModeOfProgram(IRunSpace space, ITestListConfig allTestToDo,out TestController testController)
        {
            testController = null;
            if (space.InIIS)
            {

                allTestToDo.GetTestList();
                testController = new TestController();
                BeCgi(allTestToDo, testController);
            }
            else
            {
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
            }
            else
            {
                Console.WriteLine("<div style='color:red;'>");
                Console.WriteLine("Dead");
                Console.WriteLine("</div>");
            }
            Console.WriteLine("</html>");
        }

        private static void CreateConfigFromConsoleMain(IRunSpace space)
        {
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
                        test.CreateConfigFromConsole();
                        testListConfig.Tests.Add(test);
                    }

                }
                testListConfig.SaveToFile();

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
