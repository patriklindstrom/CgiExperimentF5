using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

namespace F5
{
    internal class Program
    {
        private string _testSQLResultContains;
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
            Console.WriteLine("<h1>Check IIS</h1>");
            
            if (TestDB( ReadConnectionStrings()))
            {
                Console.WriteLine("<div>");
                    Console.WriteLine("Alive");
                Console.WriteLine("</div>");
            }
            Console.WriteLine("</html>");
        }

        public static string ReadConnectionStrings()
        {
            ConnectionStringSettingsCollection connections =
                ConfigurationManager.ConnectionStrings;
            return connections[ConnStr].ConnectionString;
        }

        public static bool TestDB(string connectionString)
        {
            bool dbWorks = false;
            using (var connection =
                new SqlConnection(connectionString))
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
    }
}
