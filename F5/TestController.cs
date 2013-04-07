using System.Collections.Generic;
using F5.Tests;

namespace F5
{
    public class TestController
    {
        public bool AllTestGood { get; set; }

        public void DoAllTest(IEnumerable<IAliveTest> allTestToDoTestList)
        {
            bool allTGood = true;

            foreach (var test in allTestToDoTestList
                )
            {
                allTGood = test.IsAlive();
                if (!allTGood)
                {
                    break;
                }
            }
            AllTestGood = allTGood;
        }


    }
}