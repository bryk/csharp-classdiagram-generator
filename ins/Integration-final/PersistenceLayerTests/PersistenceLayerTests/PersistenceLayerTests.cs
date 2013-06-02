using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersistenceLayer.Dto;
using PersistenceLayer;
using System.Runtime.Serialization;
using System.IO;
using System.Text;


namespace PersistenceLayerTests
{
    [TestClass]
    public class PersistenceLayerTests
    {
        private IWorkTimeAccountingPlatformDAO Dao { get; set; }

        #region ImplementedByPiotrBryk

        [TestInitialize]
        public void Initialize()
        {
            Dao = new VelocityDbWorkTimeAccountingPlatformDAO();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Dao.ClearDatabase();
            Dao.Dispose();
        }

        [TestMethod]
        public void SetUserShouldSaveProperly()
        {
            var robinHood = new User { PublicUserInfo = new PublicUserInfo { Login = "robinHood" } };
            var billGates = new User { PublicUserInfo = new PublicUserInfo { Login = "billGates" } };
            Dao.SetUser(robinHood);
            Assert.AreEqual(robinHood, ((IManagerPanelDAO)Dao).GetUser("robinHood"));
            Dao.SetUser(billGates);
            Assert.AreEqual(robinHood, ((IManagerPanelDAO)Dao).GetUser("robinHood"));
            Assert.AreEqual(billGates, ((IManagerPanelDAO)Dao).GetUser("billGates"));
        }

        [TestMethod]
        public void TestThatThereIsRoot()
        {
            Assert.IsNotNull(((IManagerPanelDAO)Dao).GetUser("root"));
        }

        [TestMethod]
        public void TestIdGenereation()
        {
            var robinHood = new User { PublicUserInfo = new PublicUserInfo { Login = "robinHood" } };
            Dao.SetUser(robinHood);
            var billGates = new User { PublicUserInfo = new PublicUserInfo { Login = "billGates" } };
            Dao.SetUser(billGates);
            Assert.AreEqual(robinHood.Id + 2 /* PublicUserInfo saved too */, billGates.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetUserShouldUpdateProperly()
        {
            var robinHood = new User { PublicUserInfo = new PublicUserInfo { Login = "robinHood", Name = "Robin" } };
            Dao.SetUser(robinHood);
            var robin2 = ((IManagerPanelDAO)Dao).GetUser("robinHood");
            robin2.PublicUserInfo.Login = "robin2";
            robin2.PublicUserInfo.Name = "New Robin";
            Dao.SetUser(robin2);
            Assert.AreEqual("New Robin", ((IManagerPanelDAO)Dao).GetUser("robin2").PublicUserInfo.Name);
            ((IManagerPanelDAO)Dao).GetUser("robinHood"); // should throw exception
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetUserFailWhileGettingNonExisitngUser()
        {
            ((IManagerPanelDAO)Dao).GetUser("sadsadsadaa");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetUserFailWhileGettingNonExisitngUser2()
        {
            var robinHood = new User { PublicUserInfo = new PublicUserInfo { Login = "robinHood" } };
            Dao.SetUser(robinHood);
            ((IManagerPanelDAO)Dao).GetUser("billGates");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetUserShouldntAllowToSaveUsersWithTheSameLogin()
        {
            var robinHood = new User { PublicUserInfo = new PublicUserInfo { Login = "robinHood" } };
            Dao.SetUser(robinHood);
            Dao.SetUser(new User { PublicUserInfo = new PublicUserInfo { Login = "robinHood" } });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetUserShouldntAllowToSaveUsersWithoutPublicInfo()
        {
            Dao.SetUser(new User()); // no public user info
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetUserShouldntAllowToSaveUsersWithEmptyLogin()
        {
            Dao.SetUser(new User { PublicUserInfo = new PublicUserInfo { Login = "" } }); // no login
        }

        [TestMethod]
        public void SetPasswordShouldUpdatePassword()
        {
            var robinHood = new User { PublicUserInfo = new PublicUserInfo { Login = "robinHood", Name = "Robin" } };
            Dao.SetUser(robinHood);
            Assert.IsNull(((IManagerPanelDAO)Dao).GetUser("robinHood").PasswordHash);
            ((IManagerPanelDAO)Dao).SetPassword("robinHood", "passwd");
            Assert.AreEqual("passwd", ((IManagerPanelDAO)Dao).GetUser("robinHood").PasswordHash);
        }

        [TestMethod]
        public void TestSetHourlyRate()
        {
            var robinHood = new User { PublicUserInfo = new PublicUserInfo { Login = "robinHood", Name = "Robin" } };
            var project = new Project() { Manager = robinHood.PublicUserInfo };
            var desc = new EmployeeDescription { HourlyRate = 100, Employee = robinHood.PublicUserInfo, Project = project };
            Dao.SetUser(robinHood);
            Dao.SetProject(project);
            Dao.SetEmployeeDescription(desc);
            Dao.SetHourlyRate(desc, 200);
            Assert.AreEqual(200, Dao.GetEmployeeDescriptions(robinHood)[0].HourlyRate);
        }

        [TestMethod]
        public void SerializationTest()
        {
            var robinHood = new User { Id = 7, PublicUserInfo = new PublicUserInfo { Id = 33, Login = "robinHood", Name = "Robin" } };
            User copied = null;
            DataContractSerializer dataContractSerializer = new DataContractSerializer(robinHood.GetType());

            using (MemoryStream memoryStream = new MemoryStream())
            {
                dataContractSerializer.WriteObject(memoryStream, robinHood);
                memoryStream.Position = 0;
                copied = (User)dataContractSerializer.ReadObject(memoryStream);
            }
            Assert.AreEqual(robinHood.Id, copied.Id);
            Assert.AreEqual(robinHood.PublicUserInfo.Id, copied.PublicUserInfo.Id);
            Assert.AreEqual(robinHood.PublicUserInfo.Login, copied.PublicUserInfo.Login);
            Assert.AreEqual(robinHood.PublicUserInfo.Name, copied.PublicUserInfo.Name);
        }


        [TestMethod]
        public void TestGetContractsOfManager()
        {
            var robinHood = new User { PublicUserInfo = new PublicUserInfo { Login = "robinHood", Name = "Robin" } };
            Dao.SetUser(robinHood);
            Project project = new Project() { Manager = robinHood.PublicUserInfo };
            Dao.SetProject(project);
            Contract contract = new Contract
            {
                Creator = robinHood.PublicUserInfo,
                Employee = robinHood.PublicUserInfo,
                Project = project
            };
            Dao.SetContract(contract);
            Assert.AreEqual(1, Dao.GetContractsOfManager(robinHood).Count);
            Assert.AreEqual(contract, Dao.GetContractsOfManager(robinHood)[0]);
        }
        #endregion

        #region ImplementedByAdamObuchowicz

        [TestMethod]
        public void CheckingCorrectLoginAndPassword()
        {
            Dao.SetUser(new User() { PublicUserInfo = new PublicUserInfo { Login = "a" }, PasswordHash = "b" });
            Assert.IsTrue(Dao.CheckLoginAndPassword("a", "b"));
        }

        [TestMethod]
        public void CheckingIncorrectPassword()
        {
            Assert.IsFalse(Dao.CheckLoginAndPassword("a", "c"));
        }

        [TestMethod]
        public void CheckingIncorrectLogin()
        {
            Assert.IsFalse(Dao.CheckLoginAndPassword("b", "b"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPrivilegesFailWhileGettingNonExistingUsersPrivileges()
        {
            Dao.GetPrivileges("I don't exist");
        }

        [TestMethod]
        public void GetPrivilegesShouldReturnCorrectPrivileges()
        {
            IList<Role> roles = new List<Role>();
            roles.Add(Role.Administrator);
            Dao.SetUser(new User() { PublicUserInfo = new PublicUserInfo { Login = "Administrator" }, Roles = roles });
            roles = new List<Role>();
            roles.Add(Role.NormalUser);
            Dao.SetUser(new User() { PublicUserInfo = new PublicUserInfo { Login = "NormalUser" }, Roles = roles });
            roles = new List<Role>();
            roles.Add(Role.Manager);
            roles.Add(Role.Administrator);
            Dao.SetUser(new User() { PublicUserInfo = new PublicUserInfo { Login = "AdministratorAndManager" }, Roles = roles });

            IList<Role> returning_roles = Dao.GetPrivileges("Administrator");
            Assert.AreEqual(returning_roles[0], Role.Administrator);
            returning_roles = Dao.GetPrivileges("AdministratorAndManager");
            Assert.AreEqual(returning_roles[0], Role.Manager);
            Assert.AreEqual(returning_roles[1], Role.Administrator);
            returning_roles = Dao.GetPrivileges("NormalUser");
            Assert.AreEqual(returning_roles[0], Role.NormalUser);
        }

        [TestMethod]
        public void RemovedRecordIsRemovedPermanently()
        {
            PublicUserInfo userInfo = new PublicUserInfo() { Login = "Worker" };
            Project project = new Project() { Manager = userInfo };
            EmployeeDescription employeeDescription = new EmployeeDescription() { Employee = userInfo, Project = project };
            WorkRecord workRecord = new WorkRecord() { EmployeeDescription = employeeDescription };
            User user = new User() { PublicUserInfo = userInfo };
            Dao.SetUser(user);
            Dao.SetProject(project);
            Dao.SetEmployeeDescription(employeeDescription);
            Dao.SetRecord(workRecord);
            Assert.IsTrue(Dao.GetRecordsOfEmployee(employeeDescription).Count > 0);
            Dao.RemoveRecord(workRecord);
            Assert.IsFalse(Dao.GetRecordsOfEmployee(employeeDescription).Count > 0);
        }

        private void PrepareForRemoving(String Login)
        {
            PublicUserInfo userInfo = new PublicUserInfo() { Login = "Employee" };
            User user = new User() { PublicUserInfo = userInfo };
            Project project = new Project() { Manager = userInfo };
            EmployeeDescription employeeDescription = new EmployeeDescription() { Employee = userInfo, Project = project };
            Contract contract = new Contract() { Creator = userInfo, Employee = userInfo, Project = project };
            WorkRecord record = new WorkRecord { EmployeeDescription = employeeDescription, MinutesWorked = 12 };
            Summary summary = new Summary { EmployeeDescription = employeeDescription };
            Dao.SetUser(user);
            Assert.IsFalse(Dao.GetEmployeeDescriptions(user).Count > 0);
            Assert.IsFalse(Dao.GetRecords(user).Count > 0);
            Assert.IsFalse(Dao.GetSummaries(user).Count > 0);
            Assert.IsFalse(Dao.GetContracts(user).Count > 0);
            Dao.SetProject(project);
            Dao.SetEmployeeDescription(employeeDescription);
            Dao.SetRecord(record);
            Dao.SetSummary(summary);
            Dao.SetContract(contract);
            Assert.IsTrue(Dao.GetEmployeeDescriptions(user).Count > 0);
            Assert.IsTrue(Dao.GetRecords(user).Count > 0);
            Assert.IsTrue(Dao.GetContracts(user).Count > 0);
            Assert.IsTrue(Dao.GetSummaries(user).Count > 0);
        }

        [TestMethod]
        public void DeleteUserRemoveDepencies()
        {
            PrepareForRemoving("Employee");
            User user = ((IEmployeePanelDAO)Dao).GetUser("Employee");
            Dao.RemoveUser(user);
            Assert.IsFalse(Dao.GetEmployeeDescriptions(user).Count > 0);
            Assert.IsFalse(Dao.GetRecords(user).Count > 0);
            Assert.IsFalse(Dao.GetContracts(user).Count > 0);
            Assert.IsFalse(Dao.GetSummaries(user).Count > 0);
        }

        [TestMethod]
        public void DeleteProjectRemoveDepencies()
        {
            PrepareForRemoving("Employee");
            User user = ((IEmployeePanelDAO)Dao).GetUser("Employee");
            Project project = Dao.GetEmployeeDescriptions(user)[0].Project;
            Assert.AreNotEqual(0, project.Id);
            Dao.RemoveProject(project);
            Assert.IsFalse(Dao.GetEmployeeDescriptions(user).Count > 0);
            Assert.IsFalse(Dao.GetRecords(user).Count > 0);
            Assert.IsFalse(Dao.GetContracts(user).Count > 0);
            Assert.IsFalse(Dao.GetContracts(user).Count > 0);
        }

        [TestMethod]
        public void DeleteEmployeeDescriptionRemoveDepencies()
        {
            PrepareForRemoving("Employee");
            User user = ((IEmployeePanelDAO)Dao).GetUser("Employee");
            EmployeeDescription employeeDescription = Dao.GetEmployeeDescriptions(user)[0];
            Dao.RemoveEmployeeDescription(employeeDescription);
            Assert.IsFalse(Dao.GetEmployeeDescriptions(user).Count > 0);
            Assert.IsFalse(Dao.GetRecords(user).Count > 0);
            Assert.IsFalse(Dao.GetSummaries(user).Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetRecordDontAllowToAddNewEmployeeDescription()
        {
            WorkRecord record = new WorkRecord();
            record.EmployeeDescription = new EmployeeDescription();
            Dao.SetRecord(record);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetContractDontAllowToAddNewEmployees()
        {
            Contract contract = new Contract();
            contract.Employee = new PublicUserInfo() { Login = "new employee" };
            Dao.SetContract(contract);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetContractDontAllowToAddNewManagers()
        {
            Contract contract = new Contract();
            contract.Creator = new PublicUserInfo() { Login = "new manager" };
            Dao.SetContract(contract);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RelationsFromProjectMustExist()
        {
            Project project = new Project() { Manager = null };
            Dao.SetProject(project);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RelationsFromContractMustExist1()
        {
            User employee = new User() { PublicUserInfo = new PublicUserInfo() { Login = "employee" } };
            Dao.SetUser(employee);
            Contract contract = new Contract() { Creator = null, Employee = employee.PublicUserInfo };
            Dao.SetContract(contract);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RelationsFromContractMustExist2()
        {
            User manager = new User() { PublicUserInfo = new PublicUserInfo() { Login = "employee" } };
            Dao.SetUser(manager);
            Contract contract = new Contract() { Employee = null, Creator = manager.PublicUserInfo };
            Dao.SetContract(contract);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RelationsFromEmployeeDescriptionMustExist1()
        {
            User employee = new User() { PublicUserInfo = new PublicUserInfo() { Login = "employee" } };
            Dao.SetUser(employee);
            EmployeeDescription employeeDescription = new EmployeeDescription() { Project = null, Employee = employee.PublicUserInfo };
            Dao.SetEmployeeDescription(employeeDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RelationsFromEmployeeDescriptionMustExist2()
        {
            User manager = new User() { PublicUserInfo = new PublicUserInfo() { Login = "employee" } };
            Project project = new Project() { Manager = manager.PublicUserInfo };
            Dao.SetProject(project);
            EmployeeDescription employeeDescription = new EmployeeDescription() { Project = project, Employee = null };
            Dao.SetEmployeeDescription(employeeDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RelationsFromWorkRecordMustExist()
        {
            WorkRecord record = new WorkRecord();
            Dao.SetRecord(record);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RelationsFromSummaryMustExist()
        {
            Summary summary = new Summary();
            Dao.SetSummary(summary);
        }

        [TestMethod]
        public void GetLastSummariesReturnsLastSummaries()
        {
            User user = new User { PublicUserInfo = new PublicUserInfo { Login = "aaa" } };
            Project project = new Project { Manager = user.PublicUserInfo };
            EmployeeDescription empDesc = new EmployeeDescription { Employee = user.PublicUserInfo, Project = project };
            Summary sum1 = new Summary { Date = new DateTime(2012, 10, 01), EmployeeDescription = empDesc };
            Summary sum2 = new Summary { Date = new DateTime(2012, 09, 01), EmployeeDescription = empDesc };
            Summary sum3 = new Summary { Date = new DateTime(2013, 01, 01), EmployeeDescription = empDesc };
            Dao.SetUser(user);
            Dao.SetProject(project);
            Dao.SetEmployeeDescription(empDesc);
            Dao.SetSummary(sum1);
            Dao.SetSummary(sum2);
            Dao.SetSummary(sum3);
            var sums = Dao.GetLastSummaries(empDesc, 2);
            Assert.AreEqual(sum3, sums[0]);
            Assert.AreEqual(sum1, sums[1]);
            Assert.AreEqual(2, sums.Count);
        }

        #endregion
    }
}