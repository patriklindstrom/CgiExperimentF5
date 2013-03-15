using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace F5
{
    internal class Program
    {

        private const string DefConnStr = "AliveDB";
        private const string DefTestSQL = "Select @@version;";
        private const string DefTestSQLResultContains = "Microsoft";
        public static string ConnStr
        { get
        {
            return ( DefConnStr);
        }
        }

        public static string TestSQL
        { get { return ( DefTestSQL); } }

        public static string TestSQLResultContains
        { get { return ( DefTestSQLResultContains); } }

        private static void Main(string[] args)
        {
            Console.WriteLine("\r\n\r\n");
            Console.WriteLine("<html>");
            Ping("sunet.se");
            
            Console.WriteLine("<h1>Check IIS</h1>");
            foreach (DictionaryEntry var in Environment.GetEnvironmentVariables())
                Console.WriteLine("<hr><b>{0}</b>: {1}", var.Key, var.Value);
            Console.WriteLine("<br>");
            Console.WriteLine("<h1>Check DB</h1>");
            Console.WriteLine("track writing ");
            Console.WriteLine(ReadConnectionStrings());
          //  if (TestDB( ReadConnectionStrings()))
            if(true)
            {
                Console.WriteLine("<div color=red>");
                    Console.WriteLine("Alive");
                Console.WriteLine("</div>");
            }
            Console.WriteLine("</html>");
        }

        public static string ReadConnectionStrings()
        {
            string cnStr = @"data source=Herkules\Dev;initial catalog=HaxitDB;user=sa;password=ABOverT9";
            //PATH_TRANSLATED: P:\inetpub\wwwroot\mvc1 

            // SERVER_NAME: localhost 

            // SCRIPT_NAME: /cgi/F5.exe 
            //ConnectionStringSettingsCollection connections =
            //    ConfigurationManager.ConnectionStrings;
            //return connections[ConnStr].ConnectionString;
            //XDocument doc = XDocument.Load(@"P:\inetpub\wwwroot\cgi\F5.exe.config");
            //var cnElement = doc.Elements("configuration").Elements("connectionStrings");
            //var cn = cnElement.ElementAt(0);
            //var foo = cn.Element("add");
            //var fun = foo.Attribute("connectionString");
            //cnStr = fun.Value;  
            //Console.WriteLine("<div");
            //Console.WriteLine("Here comes connectionstring: <br>" );
            //        Console.WriteLine(cnStr);
            //    Console.WriteLine("</div>");
            //    Console.WriteLine("<br>");
            
            return cnStr;
        }

        public static bool TestDB(string cnstr)
        {
            bool dbWorks = false;
            using (var connection =
                new SqlConnection(cnstr))
            {
                var command = new SqlCommand(TestSQL, connection);
                connection.Open();
                var qResult = command.ExecuteScalar();
                if (qResult != null)
                {
                    string qResultStr = qResult.ToString();
                    if (qResultStr.Contains(TestSQLResultContains))
                    {
                        dbWorks = true;
                    }
                }
                connection.Close();
                return dbWorks;
            }


        }
        public static void Ping(string hostname)
        {
            //Create ping object
            Ping netMon = new Ping();

            //Ping host (this will block until complete)
            var response = netMon.Send(hostname, 4);

            //Process ping response
            if (response != null)
            {
                string pingTime = response.RoundtripTime.ToString();
                Console.WriteLine(pingTime);
            }
        }
    }
}
