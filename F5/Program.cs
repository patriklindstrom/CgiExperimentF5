using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using System.Xml;
using System.Xml.Linq;


namespace F5
{
    internal class Program
    {


        private const string DefTestSQL = "Select @@version;";
        private const string DefTestSQLResultContains = "Microsoft";
        

        public static string TestSQL
        { get { return ( DefTestSQL); } }

        public static string TestSQLResultContains
        { get { return ( DefTestSQLResultContains); } }

        private static void Main(string[] args)
        {
           Console.WriteLine("\r\n\r\n");
            Console.WriteLine("<html>");
            string queryString = Environment.GetEnvironmentVariable("QUERY_STRING");
            DoQueyStringChores(queryString);
            if (TestDB( ReadConnectionStrings()))
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
                    Console.WriteLine("Responstime ms to {0}: {1}",query["ping"], Ping(query["ping"]));
                }
                Console.WriteLine("<br>");
                Console.WriteLine("<h1>Check DB</h1>");
                Console.WriteLine(ReadConnectionStrings());
            }
        }

        public static string ReadConnectionStrings()
        {
            XDocument doc = XDocument.Load(@".\web.config");
            var cnElement = doc.Elements("configuration").Elements("connectionStrings").ElementAt(0);
            return  cnElement.Element("add").Attribute("connectionString").Value;
        }

        public static bool TestDB(string cnstr)
        {
            bool dbWorks = false;
            try
            {       
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
                }
            }
                 catch (Exception)
            {

                dbWorks = false;
            }

            return dbWorks;
            
            

        }
        public static long Ping(string hostname)
        {
            Ping netMon = new Ping();
            var response = netMon.Send(hostname, 4);
            return response.RoundtripTime;                          
        }
    }
}
