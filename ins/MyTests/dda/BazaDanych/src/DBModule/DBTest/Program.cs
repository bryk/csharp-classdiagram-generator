using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DBModule
{

    class Program
    {

        static void Main(string[] args)
        {
            DBConnection connection = new DBConnection();
            var collection = DBConnection.db.GetCollection<Document>("Entity");

            int i = 0;

            Post post1 = new Post("0", "Gienek", "Jakis tam content", "Section1", "Topic1", DateTime.Today, PostStatus.New);
            Post post2 = new Post("1", "Autor2", "Jakis tam content", "Section1", "Topic2", DateTime.Today, PostStatus.New);
            Post post3 = new Post("2", "Autor3", "Jakis tam content", "Section2", "Topic1", DateTime.Today, PostStatus.New);
            Post post4 = new Post("3", "Autor1", "Jakis tam content", "Section2", "Topic2", DateTime.Today, PostStatus.New);
            Post post5 = new Post("4", "Autor2", "Jakis tam content", "Section3", "Topic1", DateTime.Today, PostStatus.New);
            Post post6 = new Post("5", "Autor3", "Jakis tam content", "Section3", "Topic2", DateTime.Today, PostStatus.New);

            Section section1 = new Section("Section1");
            Section section2 = new Section("Section2");
            Section section3 = new Section("Section3");

            ForumThread thread1 = new ForumThread("Section1", "Topic1");
            ForumThread thread2 = new ForumThread("Section1", "Topic2");
            ForumThread thread3 = new ForumThread("Section2", "Topic1");
            ForumThread thread4 = new ForumThread("Section2", "Topic2");
            ForumThread thread5 = new ForumThread("Section3", "Topic1");
            ForumThread thread6 = new ForumThread("Section3", "Topic2");

            Document doc1 = new Document();
            Document doc2 = new Document();
            Document doc3 = new Document();
            Document docTest = new Document();

            DocumentFromFifo dff = new DocumentFromFifo();


            /*------------------------------------------------------------------*/
            System.Console.WriteLine("Test dla modułu Ekstraktor");

            doc1.Status = DocumentStatus.New;
            doc1.Posts.Add(post1);
            doc1.Posts.Add(post2);
            doc2.Status = DocumentStatus.New;
            doc2.Posts.Add(post3);
            doc2.Posts.Add(post4);

            dff.AddDocument(doc1);
            dff.AddDocument(doc2);

            docTest = dff.GetDocument();
            while (docTest != null)
            {
                System.Console.WriteLine(docTest.Status);
                docTest = dff.GetDocument();
                i++;

            }

            if (i == 2)
            {
                System.Console.WriteLine("Zapisaliśmy i odczytaliśmy dwa dokumenty");
                i = 0;
            }
            else { i = 0; }

            doc3.Status = DocumentStatus.New;
            doc3.Posts.Add(post5);
            doc3.Posts.Add(post6);

            dff.AddDocument(doc3);

            docTest = dff.GetDocument();
            while (docTest != null)
            {
                docTest = dff.GetDocument();
                i++;
            }
            if (i == 1)
            {
                System.Console.WriteLine("Zapisaliśmy i odczytaliśmy jeden dokument. Ściąganie dookumentów z kolejki działa OK");
                i = 0;
            }
            else
            {
                System.Console.WriteLine("Bład przy ścąganiu dokumentów z kolejki");
            }
            /*------------------------------------------------------------------*/
            System.Console.WriteLine("Test dla modułu Indekser");

            EntityHandler producerList = new EntityHandler();

            producerList.AddPost("Gienek", post1);

            producerList.AddThread("Gienek", thread1);
            producerList.AddSection("Gienek", section1);

            producerList.AddPost("Czesio", post2);
            producerList.AddPost("Czesio", post3);
            producerList.AddThread("Czesio", thread2);
            producerList.AddThread("Czesio", thread3);
            producerList.AddSection("Czesio", section1);
            producerList.AddSection("Czesio", section2);

            producerList.AddPost("Kapitan Bomba", post4);
            producerList.AddPost("Kapitan Bomba", post5);
            producerList.AddPost("Kapitan Bomba", post6);
            producerList.AddThread("Kapitan Bomba", thread4);
            producerList.AddThread("Kapitan Bomba", thread5);
            producerList.AddThread("Kapitan Bomba", thread6);
            producerList.AddSection("Kapitan Bomba", section2);
            producerList.AddSection("Kapitan Bomba", section3);

            if (producerList.IsInList("Czesio") == true && producerList.IsInList("Gienek") == true && producerList.IsInList("Kapitan Bomba") == true)
            {
                System.Console.WriteLine("Zapisalismy do bazy trzech producentów");
                System.Console.Read();
            }
            else
            {
                System.Console.WriteLine("Błąd przy zapisywaniu producentów");
            }

            /*------------------------------------------------------------------*/
            System.Console.WriteLine("Test dla modułu Wyszukiwarka");

            List<dynamic> listPosts = producerList.ReceiveResults(TypeOfDepth.Post, "Gienek");
            List<dynamic> listThreads = producerList.ReceiveResults(TypeOfDepth.ForumThread, "Gienek");
            List<dynamic> listSections = producerList.ReceiveResults(TypeOfDepth.Section, "Gienek");

            int postCounter = listPosts.Count;
            int threadCounter = listPosts.Count;
            int sectionCounter = listSections.Count;

            Post p = listPosts.First();
            if (p.Author.Equals("Gienek"))
            {
                System.Console.WriteLine("Do listy są poprawnie dodawane i ściągane dane");
            }

            System.Console.WriteLine("Gienek ma:");
            System.Console.WriteLine(postCounter +" postow");
            System.Console.WriteLine(threadCounter + " tematow");
            System.Console.WriteLine(sectionCounter + " dzialow");
            System.Console.ReadKey();
        }
    } 
}