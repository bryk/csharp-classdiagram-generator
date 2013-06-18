using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using DBModule; 
using DocumentExtractor;
using System.IO;

namespace IndexerLogic
{
    public class Indexer
    {
        private int SleepTime = 1000;
        private IDataBaseIndexer Db;
        private IDataBaseGetDocument Fifo;
        private List<string> Producers;

        
        public void ReadProducersList()
        {
            try
            {
                FileStream file = new FileStream("producers.conf", FileMode.Open, FileAccess.Read);
                StreamReader read = new StreamReader(file);
                Producers = new List<string>();
                string tmp;
                while ((tmp = read.ReadLine()) != null)
                    Producers.Add(tmp.ToUpper().Trim());

                read.Close();
                file.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }

        public Indexer(IDataBaseIndexer database,IDataBaseGetDocument fifo)
        {
            Db = database;
            Fifo = fifo;
        }

        public void Run()
        {
            DBModule.Document document;

            while (true)
            {
                document = null;
                document= Fifo.GetDocument();
                if (document == null){
                    Console.WriteLine("Waiting for data...");
                    Thread.Sleep(SleepTime);
                    }
                else
                {
                    Index(document);
                    Console.WriteLine("Document has been indexed.");
                }

            }

        }

        private void Index(DBModule.Document document)
        {

            List<string> foundKeywords;
            //sprawdzanie czy w nazwie sekcji wystepuje producent
            foundKeywords = ParsePost(document.SectionTitle);
            if (foundKeywords.Count > 0)
            {
                foreach (string keyword in foundKeywords)
                {
                    Db.AddSection(keyword, new Section(document.SectionTitle));
                }
            }

            //sprawdzanie czy w nazwie watku wystepuje producent
            foundKeywords = ParsePost(document.TopicTitle);
            if (foundKeywords.Count > 0)
            {
                foreach (string keyword in foundKeywords)
                {
                    Db.AddThread(keyword, new ForumThread(document.TopicTitle, document.SectionTitle));
                }
            }

            foreach (DBModule.Post post in document.Posts)
            {
                //dodawanie ze wzgledu na autora postu
                /*string login = post.author;                
                db.add(DataType.Login, login,  post);
                db.add(DataType.Login, login, new ForumThread(document.threadName, document.sectionName));
                db.add(DataType.Login, login,  new Section(document.sectionName));
                */

                foundKeywords = ParsePost(post.Content);
                foreach (string keyword in foundKeywords)
                {
                    Db.AddPost(keyword, post);
                    Db.AddThread(keyword, new ForumThread(document.TopicTitle, document.SectionTitle));
                    Db.AddSection(keyword, new Section(document.SectionTitle));
                    
                }


            }
        
        }

        private List<string> ParsePost(string content)
        {
            List<string> foundKeywords = new List<string>();

            foreach (string producerName in Producers)
            {
                if (content.ToUpper().Contains(producerName))
                    foundKeywords.Add(producerName);
            }

            return foundKeywords;
        }


    }

}
