using DependencyAnalyzer.Analyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using NMock2;

namespace TestDependencyAnalyzer
{
    
    
    /// <summary>
    ///This is a test class for DependencyAnalyzerTest and is intended
    ///to contain all DependencyAnalyzerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyAnalyzerTest
    {

        Mockery mock ;
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
        ///A test for countDependencies
        ///</summary>
        [TestMethod()]
        public void countDependenciesTest()
        {

            int sleepTime = 0; 
            DependencyAnalyzer.Analyzer.DependencyAnalyzer target = new DependencyAnalyzer.Analyzer.DependencyAnalyzer(sleepTime); 
            List<Producer> producers = new List<Producer>();
            Dictionary<Tuple<string, string>, int> oldDependencies = new Dictionary<Tuple<string, string>, int>();
            Dictionary<Tuple<string, string>, int> help;
            Dictionary<Tuple<string, string>, int>  expected  = new Dictionary<Tuple<string,string>,int>(); 
            Dictionary<Tuple<string, string>, int> actual = new Dictionary<Tuple<string,string>,int>();
            actual = target.countDependencies(producers, oldDependencies);
            Assert.AreEqual(actual.Count,0);
            mock = new Mockery();
            IDataBaseAnalyser pr33 = mock.NewMock<IDataBaseAnalyser>();
            
            Producer pr1 = new Producer("prod1");
            Producer pr2 = new Producer("prod2");
            Producer pr3 = new Producer("prod3");
            Producer pr4 = new Producer("prod4");
            Producer pr5 = new Producer("prod5");
            Producer pr6 = new Producer("prod6");
            Producer pr7 = new Producer("prod7");
            Producer pr8 = new Producer("prod8");
            Producer pr9 = new Producer("prod9");
            Producer pr10 = new Producer("prod10");
            Producer pr11 = new Producer("prod11");
            Producer pr12 = new Producer("prod12");
            Producer pr13 = new Producer("prod13");
            Producer pr14 = new Producer("prod14");
            Producer pr15 = new Producer("prod15");
            Post p1 = new Post("1", "login1", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p2 = new Post("2", "login1", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p3 = new Post("3", "login1", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p4 = new Post("4", "login1", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p5 = new Post("5", "login1", null, null, null, System.DateTime.Now, PostStatus.Analysed);

            Post p6 = new Post("6", "login2", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p7 = new Post("7", "login2", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p8 = new Post("8", "login2", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p9 = new Post("9", "login2", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p10 = new Post("10", "login2", null, null, null, System.DateTime.Now, PostStatus.Analysed);


            Post p11 = new Post("11", "login3", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p12 = new Post("12", "login3", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p13 = new Post("13", "login3", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p14 = new Post("14", "login3", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p15 = new Post("15", "login3", null, null, null, System.DateTime.Now, PostStatus.Analysed);

            Post p16 = new Post("16", "login4", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p17 = new Post("17", "login4", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p18 = new Post("18", "login4", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p19 = new Post("19", "login4", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p20 = new Post("20", "login4", null, null, null, System.DateTime.Now, PostStatus.Analysed);

            Post p21 = new Post("16", "login5", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p22 = new Post("17", "login6", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p23 = new Post("18", "login7", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p24 = new Post("19", "login8", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            Post p25 = new Post("20", "login9", null, null, null, System.DateTime.Now, PostStatus.Analysed);
            oldDependencies = actual;
            producers.Add(pr1);
            pr1.addPost(p1);
            pr1.addPost(p2);
            pr1.addPost(p6);
            expected.Add(new Tuple<string, string>("login1", "login2"), 1);
            expected.Add(new Tuple<string, string>("login1", "prod1"), -2);
            expected.Add(new Tuple<string, string>("login2", "prod1"), -1);
            expected.Add(new Tuple<string, string>("1", ""), -1);
            expected.Add(new Tuple<string, string>("2", ""), -1);
            expected.Add(new Tuple<string, string>("6", ""), -1);
            expected.Add(new Tuple<string, string>("", "prod1"), -3);
            int k;
            actual = target.countDependencies(producers, oldDependencies);


            Assert.AreEqual(expected.Count, actual.Count);
            foreach (KeyValuePair<Tuple<string, string>, int> t in actual)
            {
                Assert.IsTrue(expected.TryGetValue(t.Key, out k));
                Assert.AreEqual(k, t.Value);
            }

            oldDependencies = actual;
            producers.Add(pr2);
            pr1.addPost(p3);
            pr2.addPost(p3);
            pr2.addPost(p7);
            pr2.addPost(p8);
            actual = target.countDependencies(producers, oldDependencies);

            expected.Remove(new Tuple<string, string>("login1", "login2"));
            expected.Add(new Tuple<string, string>("login1", "login2"), 2); //= target.countDependencies(producers);
            expected.Remove(new Tuple<string, string>("login1", "prod1"));
            expected.Add(new Tuple<string, string>("login1", "prod1"), -3);
            expected.Add(new Tuple<string, string>("login1", "prod2"), -1);
            expected.Add(new Tuple<string, string>("7", ""), -1);
            expected.Add(new Tuple<string, string>("8", ""), -1);
            expected.Remove(new Tuple<string, string>("", "prod1"));
            expected.Add(new Tuple<string, string>("", "prod1"), -4);
            expected.Add(new Tuple<string, string>("", "prod2"), -3);
            expected.Add(new Tuple<string, string>("login2", "prod2"), -2);
            expected.Add(new Tuple<string, string>("3", ""), -1);
            Assert.AreEqual(expected.Count, actual.Count);
            foreach (KeyValuePair<Tuple<string, string>, int> t in actual)
            {
                Assert.IsTrue(expected.TryGetValue(t.Key, out k));
                Assert.AreEqual(k, t.Value);
            }
            oldDependencies = actual;


            producers.Add(pr3);
            pr3.addPost(p1);
            pr3.addPost(p6);
            pr3.addPost(p11);
            actual = target.countDependencies(producers, oldDependencies);
            expected.Add(new Tuple<string, string>("login1", "prod3"), -1);
            expected.Add(new Tuple<string, string>("login2", "prod3"), -1);
            expected.Add(new Tuple<string, string>("login3", "prod3"), -1);
            expected.Add(new Tuple<string, string>("", "prod3"), -3);
            expected.Add(new Tuple<string, string>("11", ""), -1);
            expected.Remove(new Tuple<string, string>("login1", "login2"));
            expected.Add(new Tuple<string, string>("login1", "login2"), 3);
            expected.Add(new Tuple<string, string>("login1", "login3"), 1);
            expected.Add(new Tuple<string, string>("login2", "login3"), 1);
            expected.Add(new Tuple<string, string>("login3", "prod1"), 0);
            expected.Add(new Tuple<string, string>("login3", "prod2"), 0);
            //expected = target.countDependencies(producers);

            Assert.AreEqual(expected.Count, actual.Count);
            foreach (KeyValuePair<Tuple<string, string>, int> t in actual)
            {
               // Console.WriteLine(t.Key.Item1 + " " + t.Key.Item2+" "+ t.Value);
                Assert.IsTrue(expected.TryGetValue(t.Key, out k));
               // Console.WriteLine(t.Key.Item1 + " " + t.Key.Item2 + " " + t.Value +" " +k);
                Assert.AreEqual(k, t.Value);
            }
            oldDependencies = actual;
            producers.Add(pr4);
            producers.Add(pr5);
            producers.Add(pr6);
            producers.Add(pr7);
            producers.Add(pr8);
            producers.Add(pr9);
            producers.Add(pr10);
            producers.Add(pr11);
            pr4.addPost(p4);
            pr4.addPost(p6);
            pr4.addPost(p8);
            pr4.addPost(p10);
            pr4.addPost(p11);
            pr4.addPost(p15);
            pr4.addPost(p16);
            pr4.addPost(p20);
            pr4.addPost(p25);
            pr5.addPost(p1);
            pr5.addPost(p5);
            pr5.addPost(p7);
            pr5.addPost(p11);
            pr5.addPost(p13);
            pr5.addPost(p15);
            pr5.addPost(p16);
            pr5.addPost(p17);
            pr6.addPost(p3);
            pr6.addPost(p7);
            pr6.addPost(p11);
            pr6.addPost(p13);
            pr6.addPost(p15);
            pr6.addPost(p16);
            pr6.addPost(p18);
            pr6.addPost(p23);
            pr6.addPost(p25);
            pr7.addPost(p1);
            pr7.addPost(p2);
            pr7.addPost(p3);
            pr7.addPost(p4);
            pr7.addPost(p5);
            pr7.addPost(p8);
            pr7.addPost(p11);
            pr7.addPost(p12);
            pr7.addPost(p15);
            pr7.addPost(p17);
            pr7.addPost(p20);
            pr7.addPost(p21);
            pr7.addPost(p22);
            pr7.addPost(p23);
            pr7.addPost(p25);
            pr8.addPost(p1);
            pr8.addPost(p7);
            pr8.addPost(p8);
            pr8.addPost(p10);
            pr8.addPost(p11);
            pr8.addPost(p13);
            pr8.addPost(p15);
            pr8.addPost(p16);
            pr8.addPost(p17);
            pr8.addPost(p20);
            pr8.addPost(p22);
            pr9.addPost(p5);
            pr9.addPost(p6);
            pr9.addPost(p7);
            pr9.addPost(p10);
            pr9.addPost(p11);
            pr9.addPost(p13);
            pr9.addPost(p15);
            pr9.addPost(p17);
            pr9.addPost(p16);
            pr9.addPost(p22);
            pr10.addPost(p10);
            pr10.addPost(p11);
            pr10.addPost(p12);
            pr10.addPost(p13);
            pr10.addPost(p14);
            pr10.addPost(p17);
            pr10.addPost(p19);
            actual = target.countDependencies(producers, oldDependencies);
            expected = target.countDependencies(producers);

            Assert.AreEqual(expected.Count, actual.Count);
            foreach (KeyValuePair<Tuple<string, string>, int> t in actual)
            {
                Console.WriteLine(t.Key.Item1 + " " + t.Key.Item2+" "+ t.Value);
                Assert.IsTrue(expected.TryGetValue(t.Key, out k));
                Console.WriteLine(t.Key.Item1 + " " + t.Key.Item2 + " " + t.Value +" " +k);
                Assert.AreEqual(k, t.Value);
            }
        }
    }
}
