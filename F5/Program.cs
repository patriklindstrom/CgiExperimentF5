﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Specialized;
using System.Web;



namespace F5
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (RunAsCgi())
            {
                Console.WriteLine("\r\n\r\n");
                Console.WriteLine("<html>");
                string queryString = Environment.GetEnvironmentVariable("QUERY_STRING");
                DoQueyStringChores(queryString);
                if (AllTestGood())
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
                var foo = args;
                if (args.Length == 0)
                {
                    Console.WriteLine("Do you want to create AliveTestConfig File? [y]");
                    string answere = Console.ReadLine();
                    if (String.Compare(strA:answere,strB:"y",ignoreCase:true)==0)
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
                    choice = "-1";
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

        private static bool RunAsCgi()
        {
            string appPoolId = Environment.GetEnvironmentVariable("APP_POOL_ID");
            return !String.IsNullOrEmpty(appPoolId);
        }

        private static bool AllTestGood()
        {
            bool allTestGood = true;
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
                //if (!string.IsNullOrEmpty(query["ping"]))
                //{
                //    Console.WriteLine("<h1>Check ping </h1>");
                //    Console.WriteLine("Responstime ms to {0}: {1}", query["ping"], Ping(query["ping"]));
                //}
                //Console.WriteLine("<br>");
                //Console.WriteLine("<h1>Check DB</h1>");
                //Console.WriteLine(ReadConnectionStrings());
            }
        }

        private static Boolean FindConfigFile(out string configFile)
        {
            bool fileExist = false;
            configFile = "NoConfigFileExist";
            // APP_POOL_ID: CGITest  instead?
            //string pathToWebSite = Environment.GetEnvironmentVariable("PATH_TRANSLATED");
            //string WebsiteDirectory = Path.GetDirectoryName(pathToWebSite);
            string appPoolId = Environment.GetEnvironmentVariable("APP_POOL_ID");
            fileExist = File.Exists(appPoolId + ".config");
            if (fileExist)
            {
                configFile = appPoolId;
            }
            return fileExist;
        }

        private static List<IAliveTest> FindExtraTest()
        {
            string config;
            if (FindConfigFile(out config))
            {
            }

            throw new NotImplementedException();
        }






    }
}
