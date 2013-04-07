using System.Collections.Generic;
using F5;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AliveProgramTester
{
    [TestClass]
    public class ProgramTestPhase
    {
        [TestMethod]
        public void Do_A_DB_ConnTest()
        {
            //Appupall
            var fakedRunSpace = new RunSpaceTest();
            var dbConnTest = new DBConnTest();
            var fakedTestList = new F5.TestList {dbConnTest};
            var allfakedTestToDo = new TestListConfigTest(fakedTestList);
            var testController = new TestController();
            //Act
            Program.RunRightModeOfProgram(null, fakedRunSpace, allfakedTestToDo, testController);
            //Assert

        }
    }
}
