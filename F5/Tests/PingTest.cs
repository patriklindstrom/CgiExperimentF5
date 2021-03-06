﻿using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace F5.Tests
{
    public class PingTest : IAliveTest
    {
        static TraceSource _cgiTrace = new TraceSource("cgilog");
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
           
            _cgiTrace.TraceEvent(TraceEventType.Start, 2400, "Ping Test Start.");
            bool alive = false;
            long pingTime = -1; 
            try
            {
                _cgiTrace.TraceEvent(TraceEventType.Information, 2403, "Before ping call");
                pingTime =  Ping(PingAddress);
                _cgiTrace.TraceEvent(TraceEventType.Information, 2404, "After ping call");
                //Special for localhost is mostly zero in time
                if (pingTime == 0 && String.Compare(PingAddress, "localhost", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _cgiTrace.TraceEvent(TraceEventType.Information, 2405, "Localhost ping fake time {0}", pingTime.ToString());
                    alive = true;
                }
                // Normal ping time
                if (pingTime > -1)
                {
                    _cgiTrace.TraceEvent(TraceEventType.Information, 2406, "Normal ping took place");
                    alive = true;
                }
                _cgiTrace.TraceEvent(TraceEventType.Information, 2407, "Alive Status id {0}", alive.ToString());
                _cgiTrace.TraceEvent(TraceEventType.Stop, 2408, "Ping Test Stop. Ping in {0}", pingTime.ToString());

            }
            catch (Exception e)
            {
                _cgiTrace.TraceEvent(TraceEventType.Error, 9401, "Ping Test error. ErrorMessage: {0}" + e.Message);
                System.Net.Sockets.SocketException innerException = (System.Net.Sockets.SocketException) e.InnerException;
                if (innerException.ErrorCode == 11001)
                {
                    // not correct hostname
                    _cgiTrace.TraceEvent(TraceEventType.Error, 9401, "Config Error in Pingtest No Such host as: {0} ",PingAddress);
                    Console.WriteLine("Config Error in Pingtest No Such host as: {0} ", PingAddress);
                }
                else
                {
                    throw;
                }
                
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