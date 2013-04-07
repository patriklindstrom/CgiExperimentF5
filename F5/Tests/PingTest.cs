using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace F5.Tests
{
    public class PingTest : IAliveTest
    {
        public string PingAddress { get; set; }
        private static long Ping(string hostname)
        {
            var netMon = new Ping();
            var response = netMon.Send(hostname, 4);
            Debug.Assert(response != null, "response != null");
            return response.RoundtripTime;
        }
        public bool IsAlive()
        {
            bool alive = false;
           
           long pingTime= Ping(PingAddress);
            //Special for localhost is mostly zero in time
            if (pingTime ==0 && String.Compare(PingAddress, "localhost", StringComparison.OrdinalIgnoreCase)==0)
            {
                alive = true;
            }
            // Normal ping time
           if (pingTime>0)
            {
                alive = true;
            }
            return alive;
        }

        public void CreateConfigFromConsole()
        {
            Console.WriteLine("Add Ping Address");
            string pingAddress = Console.ReadLine();
            PingAddress = pingAddress;
        }
    }
}