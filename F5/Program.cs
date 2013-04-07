using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using F5.Tests;


namespace F5
{
    internal class Program
    {
        public static IRunSpace Space;
        private static void Main(string[] args)
        {
             Space = new RunSpaceTest();

            if (Space.InIIS)

            {
                Console.WriteLine("\r\n\r\n");
                Console.WriteLine("<html>");
                string queryString = Environment.GetEnvironmentVariable("QUERY_STRING");
                if (!String.IsNullOrEmpty(queryString)) { DoQueyStringChores(queryString); }               
                var allTestToDo = new TestListConfig(Space.ConfigFile);
                if (AllTestGood(allTestToDo))
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
            else
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Do you want to create AliveTestConfig File? [y]");
                    string answere = Console.ReadLine();
                    if (String.Compare(answere, "y", StringComparison.OrdinalIgnoreCase)==0)
                    {
                        CreateConfigFromConsoleMain();
                    }
                }
            }
        }

        private static void CreateConfigFromConsoleMain()
        {
            Console.WriteLine("Enter Name of Application pool?");
            string applPool = Console.ReadLine();
            if (!String.IsNullOrEmpty(applPool))
            {
                var testListConfig = new TestListConfig(applPool);
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

        private static bool AllTestGood(TestListConfig allTestToDo)
        {
            bool allTestGood = true;
           
            allTestToDo.ReadExtraTest();
            foreach (var test in allTestToDo.Tests)
            {
              allTestGood =  test.IsAlive();
                if (!allTestGood)
                {
                    break;
                }
            }
            return allTestGood;
        }

        private static void DoQueyStringChores(string queryString)
        {
            if (String.IsNullOrEmpty(queryString)) return;
            NameValueCollection query = HttpUtility.ParseQueryString(queryString);
            if (query["debug"] == "true")
            {
                Console.WriteLine("<h1>Check IIS</h1>");
                foreach (DictionaryEntry var in Environment.GetEnvironmentVariables())
                    Console.WriteLine("<hr><b>{0}</b>: {1}", var.Key, var.Value);
                Console.WriteLine("<br>");
                if (!string.IsNullOrEmpty(query["ping"]))
                {
                    Console.WriteLine("<h1>Check ping </h1>");
                    var ping = new PingTest {PingAddress = query["ping"]};
                    if (ping.IsAlive())
                    {
                        Console.WriteLine("{0} is Alive", ping.PingAddress);
                    }
                }
                Console.WriteLine("<br>");
                Console.WriteLine("<h1>Check DB</h1>");
               // Console.WriteLine(ReadConnectionStrings());
            }
        }
    }
}
