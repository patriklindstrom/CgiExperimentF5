using F5;

namespace AliveProgramTester
{
    public class TestListConfigTest : ITestListConfig
    {
        public TestListConfigTest(TestList tests)
        {
            Tests = tests;
        }

        public TestList Tests { get; set; }

        public void GetTestList()
        {//fake get a list
            Tests = Tests;
        }
    }
}