using ProjectSearching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ProjectDatabase;
using NMock2;
using System.Windows.Controls;
using Project.GUI;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for SearcherTest and is intended
    ///to contain all SearcherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SearcherTest
    {

        Mockery mock = new Mockery();

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
        ///A test for Searcher Constructor
        ///</summary>
        [TestMethod()]
        public void SearcherConstructorTest()
        {
            Searcher target = new Searcher();
            target.DataBase = mock.NewMock<DataBase>();
        }

        /// <summary>
        ///A test for ReceiveMessage
        ///</summary>
        [TestMethod()]
        public void ReceiveMessageTest()
        {
            Searcher target = new Searcher();
            target.DataBase = mock.NewMock<DataBase>();
            string value = string.Empty;

            //TypeOfDepth=Section
            TypeOfDepth depth = TypeOfDepth.Section;
            List<dynamic> expected = new List<dynamic>();
            expected.Add(new Section("TV Serwis"));
            expected.Add(new Section("Monitor Serwis"));

            List<dynamic> actual = target.ReceiveMessage(depth, value);

            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Section section1 = expected[i];
                Section section2 = actual[i];

                Assert.AreEqual(section1.SectionName, section2.SectionName);
            }

            //TypeOfDepth=Thread
            depth = TypeOfDepth.Thread;

            expected.Clear();
            expected.Add(mock.NewMock<ForumThread>("TV Serwis", "Pilot"));
            expected.Add(mock.NewMock<ForumThread>("TV Serwis", "Sharp 40LE brak obrazu"));
            expected.Add(mock.NewMock<ForumThread>("Monitor Serwis", "Menu monitora pytanie"));

            actual.Clear();
            actual = target.ReceiveMessage(depth, value);

            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                ForumThread thread1 = expected[i];
                ForumThread thread2 = actual[i];
                Assert.AreEqual(thread1.SectionName, thread2.SectionName);
                Assert.AreEqual(thread1.ThreadName, thread2.ThreadName);
            }

            //TypeOfDepth=Post
            depth = TypeOfDepth.Post;

            expected.Clear();
            expected.Add(mock.NewMock<Post>("Pilot", "TV Serwis", "andrzej91", "Mam problem", new DateTime(2012, 10, 1)));
            expected.Add(mock.NewMock<Post>("Pilot", "TV Serwis", "jola29", "Mam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tam", new DateTime(2012, 10, 1)));
            expected.Add(mock.NewMock<Post>("Pilot", "TV Serwis", "91kazek", "Mozna inaczej", new DateTime(2012, 10, 1)));
            expected.Add(mock.NewMock<Post>("Menu monitora pytanie", "Monitor serwis", "kubus", "Pomocy", new DateTime(2012, 10, 1)));
            expected.Add(mock.NewMock<Post>("Sharp 40LE brak obrazu", "TV Serwis", "kubus", "Juz wiem", new DateTime(2012, 10, 1)));

            actual.Clear();
            actual = target.ReceiveMessage(depth, value);

            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Post post1 = expected[i];
                Post post2 = actual[i];
                Assert.AreEqual(post1.Author, post2.Author);
                Assert.AreEqual(post1.Content, post2.Content);
                Assert.AreEqual(post1.dateTime, post2.dateTime);
                Assert.AreEqual(post1.SectionName, post2.SectionName);
                Assert.AreEqual(post1.ThreadName, post2.ThreadName);
            }
        }

        /// <summary>
        ///A test for DataBase
        ///</summary>
       /* [TestMethod()]
        [DeploymentItem("ProjectSearching.dll")]
        public void DataBaseTest()
        {
            Searcher_Accessor target = new Searcher_Accessor();
            IDataBase expected = mock.NewMock<DataBase>();
            IDataBase actual;
            target.DataBase = expected;
            actual = target.DataBase;
            Assert.AreEqual(expected, actual);
        }*/

        /// <summary>
        ///A test for LoadCollectionData
        ///</summary>
        [TestMethod()]
        public void LoadCollectionDataTest()
        {
            Searcher target = new Searcher(); 
            Post post = mock.NewMock<Post> ("ThreadName", "SectionName", "author", "Lorem Ipsum", new DateTime (2013, 1, 1)); 
            TextBox textBlock1 = new TextBox(); // TODO: Initialize to an appropriate value
            List<Data> expected = new List<Data>();
            expected.Add(new Data ()
            {
                Attribute = "Section",
                Value = post.SectionName
            });
            expected.Add(new Data()
            {
                Attribute = "Thread",
                Value = post.ThreadName
            });
            expected.Add(new Data()
            {
                Attribute = "Author",
                Value = post.Author
            });
            expected.Add(new Data()
            {
                Attribute = "Date",
                Value = post.dateTime.ToString()
            });

            textBlock1.Text = post.Content;

            List<Data> actual;
            actual = target.LoadCollectionData(post, textBlock1);
            
            Assert.AreEqual(expected.Count, actual.Count);

            for (int i=0; i<expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Attribute, actual[i].Attribute);
                Assert.AreEqual(expected[i].Value, actual[i].Value);
            }
        }

        /// <summary>
        ///A test for search
        ///</summary>
        [TestMethod()]
        public void searchTest()
        {
            Searcher target = new Searcher();
            DataBase database = mock.NewMock<DataBase>();
            string value = string.Empty;
            Result result = new Result(TypeOfDepth.Section,value,value);
            TreeView treeView1 = result.getTreeView();

            TypeOfDepth depth = TypeOfDepth.Section;
            List<dynamic> expected = new List<dynamic>();
            expected.Add(new Section("TV Serwis"));
            expected.Add(new Section("Monitor Serwis"));

            List<dynamic> actual;
            actual = target.search(depth, value, treeView1);

            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Section section1 = expected[i];
                Section section2 = actual[i];

                Assert.AreEqual(section1.SectionName, section2.SectionName);
            }

            //TypeOfDepth=Thread
            depth = TypeOfDepth.Thread;
            result = new Result(TypeOfDepth.Section, value, value);
            treeView1 = result.getTreeView();

            expected.Clear();
            expected.Add(mock.NewMock<ForumThread>("TV Serwis", "Pilot"));
            expected.Add(mock.NewMock<ForumThread>("TV Serwis", "Sharp 40LE brak obrazu"));
            expected.Add(mock.NewMock<ForumThread>("Monitor Serwis", "Menu monitora pytanie"));

            actual.Clear();
            actual = target.search(depth, value, treeView1);

            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                ForumThread thread1 = expected[i];
                ForumThread thread2 = actual[i];
                Assert.AreEqual(thread1.SectionName, thread2.SectionName);
                Assert.AreEqual(thread1.ThreadName, thread2.ThreadName);
            }

            //TypeOfDepth=Post
            depth = TypeOfDepth.Post;
            result = new Result(TypeOfDepth.Section, value, value);
            treeView1 = result.getTreeView();

            expected.Clear();
            expected.Add(mock.NewMock<Post>("Pilot", "TV Serwis", "andrzej91", "Mam problem", new DateTime(2012, 10, 1)));
            expected.Add(mock.NewMock<Post>("Pilot", "TV Serwis", "jola29", "Mam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tam", new DateTime(2012, 10, 1)));
            expected.Add(mock.NewMock<Post>("Pilot", "TV Serwis", "91kazek", "Mozna inaczej", new DateTime(2012, 10, 1)));
            expected.Add(mock.NewMock<Post>("Menu monitora pytanie", "Monitor serwis", "kubus", "Pomocy", new DateTime(2012, 10, 1)));
            expected.Add(mock.NewMock<Post>("Sharp 40LE brak obrazu", "TV Serwis", "kubus", "Juz wiem", new DateTime(2012, 10, 1)));

            actual.Clear();
            actual = target.search(depth, value, treeView1);

            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Post post1 = expected[i];
                Post post2 = actual[i];
                Assert.AreEqual(post1.Author, post2.Author);
                Assert.AreEqual(post1.Content, post2.Content);
                Assert.AreEqual(post1.dateTime, post2.dateTime);
                Assert.AreEqual(post1.SectionName, post2.SectionName);
                Assert.AreEqual(post1.ThreadName, post2.ThreadName);
            }
        }

        /// <summary>
        ///A test for searchFromAnotherNodes
        ///</summary>
        [TestMethod()]
        public void searchFromAnotherNodesTest()
        {
            Searcher target = new Searcher();
            target.ResultList = new List<dynamic>();
            string value = string.Empty;
            Result result = new Result(TypeOfDepth.Section, value, value);
            TreeView treeView1 = result.getTreeView();

            TypeOfDepth depth = TypeOfDepth.Section;
            List<dynamic> expected = new List<dynamic>();
            expected.Add(new Section("TV Serwis [FROM COMMUNICATION]"));
            expected.Add(new Section("Monitor Serwis"));

            List<dynamic> actual;
            actual = target.searchFromAnotherNodes(depth, value, treeView1);

            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Section section1 = expected[i];
                Section section2 = actual[i];

                Assert.AreEqual(section1.SectionName, section2.SectionName);
            }

            //TypeOfDepth=Thread
            depth = TypeOfDepth.Thread;
            result = new Result(TypeOfDepth.Section, value, value);
            treeView1 = result.getTreeView();

            expected.Clear();
            expected.Add(new ForumThread("TV Serwis [FROM COMMUNICATION]", "Pilot [FROM COMMUNICATION]"));
            expected.Add(new ForumThread("TV Serwis [FROM COMMUNICATION]", "Sharp 40LE brak obrazu [FROM COMMUNICATION]"));
            expected.Add(new ForumThread("Monitor Serwis", "Menu monitora pytanie"));

            actual.Clear();
            actual = target.searchFromAnotherNodes(depth, value, treeView1);

            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                ForumThread thread1 = expected[i];
                ForumThread thread2 = actual[i];
                Assert.AreEqual(thread1.SectionName, thread2.SectionName);
                Assert.AreEqual(thread1.ThreadName, thread2.ThreadName);
            }

            //TypeOfDepth=Post
            depth = TypeOfDepth.Post;
            result = new Result(TypeOfDepth.Section, value, value);
            treeView1 = result.getTreeView();

            expected.Clear();
            expected.Add(new Post("Pilot [FROM COMMUNICATION]", "TV Serwis [FROM COMMUNICATION]", "andrzej91", "Mam problem [FROM COMMUNICATION]", new DateTime(2012, 10, 1)));
            expected.Add(new Post("Pilot", "TV Serwis", "jola29", "Mam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tam", new DateTime(2012, 10, 1)));
            expected.Add(new Post("Pilot [FROM COMMUNICATION]", "TV Serwis [FROM COMMUNICATION]", "91kazek", "Mozna inaczej [FROM COMMUNICATION]", new DateTime(2012, 10, 1)));
            expected.Add(new Post("Menu monitora pytanie", "Monitor serwis", "kubus", "Pomocy", new DateTime(2012, 10, 1)));
            expected.Add(new Post("Sharp 40LE brak obrazu [FROM COMMUNICATION]", "TV Serwis [FROM COMMUNICATION]", "kubus", "Juz wiem [FROM COMMUNICATION]", new DateTime(2012, 10, 1)));
            expected.Add(new Post("Pilot", "TV Serwis", "dodane", "dodane", new DateTime(2012, 10, 1)));
            actual.Clear();
            actual = target.searchFromAnotherNodes(depth, value, treeView1);

            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Post post1 = expected[i];
                Post post2 = actual[i];
                Assert.AreEqual(post1.Author, post2.Author);
                Assert.AreEqual(post1.Content, post2.Content);
                Assert.AreEqual(post1.dateTime, post2.dateTime);
                Assert.AreEqual(post1.SectionName, post2.SectionName);
                Assert.AreEqual(post1.ThreadName, post2.ThreadName);
            }
        }

        /// <summary>
        ///A test for ChangeName
        ///</summary>
        [TestMethod()]
        public void ChangeNameTest()
        {
            Searcher target = new Searcher(); 
            string name = "Coś bardzo, bardzo długiego, co ma być krótsze";
            string expected = name.Substring(0, 15) + "..."; 
            string actual;
            actual = target.ChangeName(name);
            Assert.AreEqual(expected, actual);
        }
    }
}
