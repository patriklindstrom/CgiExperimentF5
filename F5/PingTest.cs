using System;
using System.Net.NetworkInformation;

namespace F5
{
    internal class PingTest : IAliveTest
    {
        public static string PingAddress { get; set; }
        private static long Ping(string hostname)
        {
            Ping netMon = new Ping();
            var response = netMon.Send(hostname, 4);
            return response.RoundtripTime;
        }
        public bool IsAlive()
        {
            bool alive = false;
           
           long pingTime= Ping(PingAddress);
            //Special for localhost is mostly zero in time
            if (pingTime ==0 && String.Compare(strA:PingAddress,strB:"localhost",ignoreCase:true)==0)
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