using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DocumentExtractor.Model;
using NUnit.Framework;

namespace DocumentExtractor.Tests
{
    [TestFixture]
    class DocumentTest
    {
        [Test]
        public void ConstructorTest()
        {
            Document document = new Document();
            Assert.AreEqual(DocumentStatus.New, document.Status);
            Assert.IsNotNull(document.Posts);
            CollectionAssert.IsEmpty(document.Posts);
        }
    }
}
