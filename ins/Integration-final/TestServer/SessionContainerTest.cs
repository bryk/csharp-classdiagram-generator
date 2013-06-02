using System.ServiceModel;
using NMock2;
using Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PersistenceLayer.Dto;
using PersistenceLayer;
using System.Collections.Generic;
using Server.Shared.Exceptions;

namespace TestServer
{
    [TestClass()]
    public class SessionContainerTest
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

        private Mockery mockery;
        private IWorkTimeAccountingPlatformDAO dependency;
        private SessionContainer testee;

        [TestInitialize]
        public void SetUp()
        {
            mockery = new Mockery();
            dependency = mockery.NewMock<IWorkTimeAccountingPlatformDAO>();
            testee = new SessionContainer(dependency);
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

        [TestMethod()]
        public void TryLoginWithNullRolesTest()
        {
            Assert.IsFalse(testee.TryLogin("123", "zenek", "passwd", null));
        }

        [TestMethod()]
        public void TryLoginWithWrongPasswdTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            List<Role> requestedRoles = new List<Role>() { Role.NormalUser };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(false));
            Assert.IsFalse(testee.TryLogin("12345", login, passwdHash, requestedRoles));
        }

        [TestMethod()]
        public void TryLoginWithInsufficientPriviledgesTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            List<Role> actualRoles = new List<Role>() {Role.NormalUser};
            List<Role> requestedRoles = new List<Role>() { Role.NormalUser, Role.Administrator };
            User jan = new User
                {
                    PasswordHash = passwdHash,
                    PublicUserInfo = new PublicUserInfo {Login = login, Name = "Jan", Surname = "Kowalski"},
                    Roles = actualRoles
                };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            Assert.IsFalse(testee.TryLogin("12345", login, passwdHash, requestedRoles));
        }

        [TestMethod()]
        public void TryLoginWithExcessivePriviledgesTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            List<Role> actualRoles = new List<Role>() { Role.NormalUser, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Manager };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            Assert.IsTrue(testee.TryLogin("12345", login, passwdHash, requestedRoles));
        }

        [TestMethod()]
        public void TryLoginWithEqualPriviledgesTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Manager, Role.Administrator };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            Assert.IsTrue(testee.TryLogin("12345", login, passwdHash, requestedRoles));
        }

        [TestMethod()]
        public void FindSessionSuccessTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Manager };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            Session sessionRegistered = testee.FindSession(sessionId);
            Assert.IsNotNull(sessionRegistered);
            Assert.AreEqual(sessionId, sessionRegistered.SessionId);
            Assert.AreEqual(login, sessionRegistered.Login);
            CollectionAssert.AreEqual(requestedRoles, sessionRegistered.Roles);
            CollectionAssert.IsSubsetOf(sessionRegistered.Roles, actualRoles);
        }

        [TestMethod()]
        public void FindSessionFailureTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Manager, Role.Manager, Role.NormalUser };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            Session sessionRegistered = testee.FindSession(sessionId);
            Assert.IsNull(sessionRegistered);
        }

        [TestMethod()]
        public void HasActiveSessionSuccessTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Manager };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            Assert.IsTrue(testee.HasActiveSession(sessionId));
            Assert.IsFalse(testee.HasActiveSession(sessionId + "sdcfsadfbs"));
            Assert.IsFalse(testee.HasActiveSession(null));
        }

        [TestMethod()]
        public void HasActiveSessionFailureTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Administrator, Role.Manager, Role.NormalUser };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            Assert.IsFalse(testee.HasActiveSession(sessionId));
            Assert.IsFalse(testee.HasActiveSession(sessionId + "sdcfsadfbs"));
            Assert.IsFalse(testee.HasActiveSession(null));
        }


        [TestMethod()]
        public void IsAdminSuccessIsManagerSuccessTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Administrator, Role.Manager };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            Assert.IsTrue(testee.IsAdmin(sessionId));
            Assert.IsTrue(testee.IsManager(sessionId));
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void IsAdminSuccessIsManagerFailureTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Administrator };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            Assert.IsTrue(testee.IsAdmin(sessionId));
            Assert.IsFalse(testee.IsManager(sessionId));
            Assert.IsTrue(true);
        }

        [TestMethod()]
        [ExpectedException(typeof(FaultException<NoActiveSession>))]
        public void IsAdminNoSessionTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Administrator };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            Assert.IsTrue(testee.IsAdmin(sessionId));
            testee.IsAdmin(sessionId + "fsdfsdfd");
            Assert.IsTrue(true);
        }

        [TestMethod()]
        [ExpectedException(typeof(FaultException<NoActiveSession>))]
        public void IsManagerNoSessionTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Manager };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            testee.IsManager(sessionId + "fsdfsdfd");
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void IsManagerWithSpecifiedLoginTest1_IsNormalUserWithSpecifiedLoginTest1()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Administrator, Role.Manager };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            Assert.IsTrue(testee.IsManagerWithSpecifiedLogin(sessionId, login));
            Assert.IsFalse(testee.IsNormalUserWithSpecifiedLogin(sessionId, login));
            Assert.IsFalse(testee.IsManagerWithSpecifiedLogin(sessionId, login + "sdfsdf"));
            Assert.IsFalse(testee.IsNormalUserWithSpecifiedLogin(sessionId, login + "sdcfvsdfsd"));
        }

        [TestMethod()]
        public void IsManagerWithSpecifiedLoginTest2_IsNormalUserWithSpecifiedLoginTest2()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.NormalUser };
            List<Role> requestedRoles = new List<Role>() { Role.NormalUser };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            Assert.IsFalse(testee.IsManagerWithSpecifiedLogin(sessionId, login));
            Assert.IsTrue(testee.IsNormalUserWithSpecifiedLogin(sessionId, login));
            Assert.IsFalse(testee.IsManagerWithSpecifiedLogin(sessionId, login + "sdfsdf"));
            Assert.IsFalse(testee.IsNormalUserWithSpecifiedLogin(sessionId, login + "sdcfvsdfsd"));
        }

        [TestMethod()]
        [ExpectedException(typeof(FaultException<NoActiveSession>))]
        public void IsManagerWithSpecifiedLoginNoSessionTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Manager };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            testee.IsManagerWithSpecifiedLogin(sessionId + "fsdfsdfd", login);
            Assert.IsTrue(true);
        }

        [TestMethod()]
        [ExpectedException(typeof(FaultException<NoActiveSession>))]
        public void IsNormalUserWithSpecifiedLoginNoSessionTest()
        {
            string login = "jan";
            string passwdHash = "ala123";
            string sessionId = "qwerty098";
            List<Role> actualRoles = new List<Role>() { Role.Administrator, Role.Manager };
            List<Role> requestedRoles = new List<Role>() { Role.Manager };
            User jan = new User
            {
                PasswordHash = passwdHash,
                PublicUserInfo = new PublicUserInfo { Login = login, Name = "Jan", Surname = "Kowalski" },
                Roles = actualRoles
            };
            Expect.Once.On(dependency).Method("CheckLoginAndPassword").With(login, passwdHash).Will(Return.Value(true));
            Expect.Once.On(dependency).Method("GetUser").With(login).Will(Return.Value(jan));
            testee.TryLogin(sessionId, login, passwdHash, requestedRoles);

            testee.IsNormalUserWithSpecifiedLogin(sessionId + "fsdfsdfd", login);
            Assert.IsTrue(true);
        }
    }
}
