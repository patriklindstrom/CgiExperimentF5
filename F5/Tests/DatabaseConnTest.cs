using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace F5.Tests
{
    public class DatabaseConnTest : IAliveTest
    {
        static TraceSource _cgiTrace = new TraceSource("cgilog");
        private const string DefTestSQL = "Select @@version;";
        private const string DefTestSQLResultContains = "Microsoft";

        private static string TestSQL
        {
            get { return (DefTestSQL); }
        }

        private static string TestSQLResultContains
        {
            get { return (DefTestSQLResultContains); }
        }

        public  string ConnString { get; set; }


        private static bool TestDB(string cnstr)
        {
            bool dbWorks = false;
            try
            {
                _cgiTrace.TraceEvent(TraceEventType.Start, 2500, "SQL Test Start.");
                using (var connection =
                    new SqlConnection(cnstr))
                {
                    _cgiTrace.TraceEvent(TraceEventType.Information, 2403, "Before SQL call of {0}", TestSQL);
                    var command = new SqlCommand(TestSQL, connection);
                    connection.Open();
                    var qResult = command.ExecuteScalar();
                    if (qResult != null)
                    {
                        string qResultStr = qResult.ToString();
                        _cgiTrace.TraceEvent(TraceEventType.Information, 2506, "Result from TestSQLQuery: {0} ", qResultStr);
                        _cgiTrace.TraceEvent(TraceEventType.Information, 2507, "TestSQLResultContains {0}", TestSQLResultContains);
                        if (qResultStr.Contains(TestSQLResultContains))
                        {                           
                            dbWorks = true;                            
                        }
                        _cgiTrace.TraceEvent(TraceEventType.Information, 2508, "dbWorks was {0} ", dbWorks.ToString());
                    }
                    connection.Close();
                    _cgiTrace.TraceEvent(TraceEventType.Information, 2509, "DB Connection closed and Alive Status id {0}", dbWorks.ToString());
                    _cgiTrace.TraceEvent(TraceEventType.Stop, 2500, "SQL Test Stop.");
                }
            }
            catch (Exception e)
            {
                _cgiTrace.TraceEvent(TraceEventType.Error, 9501, "SQL Test error. ErrorMessage: " + e.Message);
                dbWorks = false;
                
            }

            return dbWorks;
        }

        public bool IsAlive()
        {
           return TestDB(ConnString);
        }

        public void CreateConfigFromConsole()
        {
            Console.WriteLine("Add Database Connection string");
            string  conString = Console.ReadLine();
            ConnString = conString;
        }

        //private static string ReadConnectionStrings()
        //{
        //    XDocument doc = XDocument.Load(@".\web.config");
        //    var cnElement = doc.Elements("configuration").Elements("connectionStrings").ElementAt(0);
        //    return cnElement.Element("add").Attribute("connectionString").Value;
        //}
    }
}