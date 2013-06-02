using ManagerPanel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PersistenceLayer;
using System.Windows.Input;
using System.Collections.Generic;

using System.Collections.ObjectModel;

namespace ManagerPanel.Tests
{
    
    
    /// <summary>
    ///This is a test class for MainWindowViewModelTest and is intended
    ///to contain all MainWindowViewModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MainWindowViewModelTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for sumContracts
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ManagerPanel.exe")]
        public void sumContractsTest()
        {
            MainWindowViewModel_Accessor target = new MainWindowViewModel_Accessor(); // TODO: Initialize to an appropriate value
            Project pr = null; // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            actual = target.sumContracts(pr);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Employees
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ManagerPanel.exe")]
        public void EmployeesTest()
        {
            MainWindowViewModel_Accessor target = new MainWindowViewModel_Accessor(); // TODO: Initialize to an appropriate value
            IEnumerable<EmployeeDescription> actual;
            actual = target.Employees;
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for LastSummaryDate
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ManagerPanel.exe")]
        public void LastSummaryDateTest()
        {
            MainWindowViewModel_Accessor target = new MainWindowViewModel_Accessor(); // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = target.LastSummaryDate;
            Assert.IsNotNull(actual);
        }


        /// <summary>
        ///A test for Projects
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ManagerPanel.exe")]
        public void ProjectsTest()
        {
            MainWindowViewModel_Accessor target = new MainWindowViewModel_Accessor(); // TODO: Initialize to an appropriate value
            IEnumerable<Project> actual;
            actual = target.Projects;
            Assert.IsNotNull(actual);
        }


        /// <summary>
        ///A test for SelectedEmployee
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ManagerPanel.exe")]
        public void SelectedEmployeeTest()
        {
            MainWindowViewModel_Accessor target = new MainWindowViewModel_Accessor(); // TODO: Initialize to an appropriate value
            EmployeeDescription expected = null; // TODO: Initialize to an appropriate value
            EmployeeDescription actual;
            target.SelectedEmployee = expected;
            actual = target.SelectedEmployee;
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SelectedProject
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ManagerPanel.exe")]
        public void SelectedProjectTest()
        {
            MainWindowViewModel_Accessor target = new MainWindowViewModel_Accessor();
            TemporaryDataBase db = new TemporaryDataBase();
            Project expected = db.GetProjectsOfManager()[0]; // TODO: Initialize to an appropriate value
            Project actual;
            target.SelectedProject = expected;
            actual = target.SelectedProject;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UncountedHours
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ManagerPanel.exe")]
        public void UncountedHoursTest()
        {
            MainWindowViewModel_Accessor target = new MainWindowViewModel_Accessor(); // TODO: Initialize to an appropriate value
            double actual;
            actual = target.UncountedHours;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
