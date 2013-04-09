using System;
using F5.Tests;

namespace AliveProgramTester
{
    public class DBConnTest : IAliveTest
    {
        public bool IsAlive()
        {
            return true;
        }

        public void CreateConfigFromConsole()
        {
            throw new NotImplementedException();
        }
    }
}