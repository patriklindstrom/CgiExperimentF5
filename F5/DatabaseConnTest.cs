using System;
using System.Data.SqlClient;


namespace F5
{
    public class DatabaseConnTest : IAliveTest
    {
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