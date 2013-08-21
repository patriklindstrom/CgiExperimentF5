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
            //Arrange
            var fakedRunSpace = new RunSpaceTest();
            var dbConnTest = new DBConnTest();
            var fakedTestList = new F5.TestList {dbConnTest};
            var allfakedTestToDo = new TestListConfigTest(fakedTestList);
            TestController testController;
            //Act
            Program.RunRightModeOfProgram(fakedRunSpace, allfakedTestToDo, out testController);
            //Assert         
            Assert.IsTrue( testController.AllTestGood,"Test DB test are good");
        }
    }
}
