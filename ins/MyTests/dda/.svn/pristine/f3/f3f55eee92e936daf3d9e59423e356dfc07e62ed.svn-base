using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using DocumentExtractor.Core;
using DocumentExtractor.Model;
using NUnit.Framework;

namespace DocumentExtractor.Tests
{
    [TestFixture]
    class ExtractorTest
    {
        private const int numberOfTestFiles = 3;

        private void AssertPosts(List<Post> posts1, List<Post> posts2)
        {
            Assert.AreEqual(posts1.Count, posts2.Count);
            Post[] p1 = posts1.OrderBy(e => e.Id).ToArray<Post>();
            Post[] p2 = posts2.OrderBy(e => e.Id).ToArray<Post>();
            for (int i = 0; i < p1.Length; ++i)
            {
                Assert.AreEqual(p1[i].Author, p2[i].Author);
                Assert.AreEqual(p1[i].Date, p2[i].Date);
                Assert.AreEqual(p1[i].Content, p2[i].Content);
                Assert.AreEqual(p1[i].Id, p2[i].Id);
            }
        }

        [Test]
        public void ReadStreamTest()
        {
            for (int i = 1; i <= numberOfTestFiles; ++i)
            {
                StreamReader htmlStreamReader = new StreamReader(".\\..\\..\\TestData\\" + i.ToString() + ".html");
                Document generatedDocument = Extractor.ReadStream(htmlStreamReader);
                Document readDocument = new Document();
                using (StreamReader outStreamReader = new StreamReader(".\\..\\..\\TestData\\" + i.ToString() + ".txt"))
                {
                    readDocument.SectionTitle = outStreamReader.ReadLine();
                    readDocument.TopicTitle = outStreamReader.ReadLine();
                    string line;
                    while ((line = outStreamReader.ReadLine()) != null)
                    {
                        Post post = new Post();
                        post.Author = line;
                        post.Date = DateTime.Parse(outStreamReader.ReadLine());
                        post.Content = "";
                        post.Id = post.Author + post.Date;
                        while ((line = outStreamReader.ReadLine()) != "<next>") post.Content += (line + "\r");
                        post.Content = post.Content.TrimEnd();
                        readDocument.Posts.Add(post);
                    }
                }
                Assert.AreEqual(readDocument.SectionTitle, generatedDocument.SectionTitle);
                Assert.AreEqual(readDocument.TopicTitle, generatedDocument.TopicTitle);
                AssertPosts(readDocument.Posts, generatedDocument.Posts);
            }
        }
    }
}
