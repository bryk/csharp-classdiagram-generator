using Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestClient
{
    [TestClass()]
    public class PasswordHasherTest
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

        [TestMethod()]
        public void ComputeHashTest()
        {
            Assert.AreEqual("c4ca4238a0b923820dcc509a6f75849b".ToUpper(), PasswordHasher.ComputeHash("1"));
            Assert.AreEqual("0cc175b9c0f1b6a831c399e269772661".ToUpper(), PasswordHasher.ComputeHash("a"));
            Assert.AreEqual("eccbc87e4b5ce2fe28308fd9f2a7baf3".ToUpper(), PasswordHasher.ComputeHash("3"));
            Assert.AreNotEqual("0cc175b9c0f1b6a831c399e269772661".ToUpper(), PasswordHasher.ComputeHash("acdsdadsddsd"));
            Assert.AreNotEqual("0cc175b9c0f1b6a831c399e269772661", PasswordHasher.ComputeHash("a"));
            Assert.AreNotEqual("0cc175b9c0f1b6a831c399e269772661".ToUpper(), PasswordHasher.ComputeHash("0cc175b9c0f1b6a831c399e269772661"));
            Assert.AreNotEqual("ala123".ToUpper(), PasswordHasher.ComputeHash("ala123"));
        }
    }
}
