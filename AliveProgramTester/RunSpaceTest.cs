
using F5;

namespace AliveProgramTester
{
    class RunSpaceTest : IRunSpace
    {
        public RunSpaceTest()
        {
            AppPoolId = "CGITest";
            InIIS = true;
            ConfigFile = @".\CGITest.xml";
        }

        public string AppPoolId { get; set; }
        public bool InIIS { get; set; }
        public string ConfigFile { get; set; }
        public string QueryString { get;  set; }
    }
}
