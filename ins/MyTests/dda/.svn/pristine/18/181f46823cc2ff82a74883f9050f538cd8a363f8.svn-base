using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuiAnalyzer.DependencyAnalyzer.DBModule;

namespace GuiAnalyzer.DependencyAnalyzer
{

    public class DataGenerator
    {
        /* Fields */
        private string[] postName = { "Pomocy", "[Ważne]", "Kolokwium", "Kartkówki", "Prowadzący", "Rozwiązania" };
        private string[] sectionName = { "Studia", "Śmieszne", "Wykłady", "Terminy" };
        private string[] authorFirst = { "Mi", "La", "No", "Wa", "Mich", "On", "Ten", "Lett" };
        private string[] authorMid = { "la", "nek", "no", "we", "ist", "rek", "lok", "lo" };
        private string[] authorLast = { "666", "2000", "5000", "_1991", "69", "_1980", "19830" };
        private string[] producerName = { "Tele", "Info", "Int", "Void", "Krak" };
        internal List<EntityProducer> Producers { get; set; }
        internal List<EntityLogin> Logins { get; set; }
        internal List<Post> Posts { get; set; }
        private Random random = new Random();

        /* Constructors */
        public DataGenerator()
        {
            Producers = new List<EntityProducer>();
            Logins = new List<EntityLogin>();
            Posts = new List<Post>();

            EntityProducer producer = null;
            EntityLogin login = null;
            string name;

            // Generation of posts
            for (int i = 0; i < 100; ++i)
                Posts.Add(new Post(GeneratePostName(), GenerateSectionName(), null, null, new DateTime()));

            // Generation of logins
            for (int i = 10; i > 0; --i)
            {
                login = new EntityLogin(name = GenerateAuthorName());
                foreach (Post post in Posts)
                {
                    if (post.Author == null && (random.Next(0, 5) == 0 || i == 1))
                    {
                        login.addPost(post);
                        post.Author = name;
                    }
                }
                Logins.Add(login);
            }

            // Generation of producers
            for (int i = 10; i > 0; --i)
            {
                producer = new EntityProducer(name = GenerateProducerName());
                foreach (Post post in Posts)
                    if (random.Next(0, 5) == 0 || i == 1)
                    {
                        producer.addPost(post);
                        post.Content = post.Content + ", " + name;
                    }
                Producers.Add(producer);
            }
        } // end DataGenerator();

        /* Methods */
        private string GeneratePostName() { return postName[random.Next(0, postName.Length - 1)] + " " + postName[random.Next(0, postName.Length - 1)]; }
        private string GenerateSectionName() { return sectionName[random.Next(0, sectionName.Length - 1)]; }
        private string GenerateProducerName() { return producerName[random.Next(0, producerName.Length - 1)] + producerName[random.Next(0, producerName.Length - 1)]; }
        private string GenerateAuthorName()
        {
            string result = authorFirst[random.Next(0, authorFirst.Length - 1)];
            for (int i = 0; i < random.Next(1, 3); ++i)
                result += authorMid[random.Next(0, authorMid.Length - 1)];
            if (random.Next(0, 2) == 0) result += authorLast[random.Next(0, authorLast.Length - 1)];
            return result;
        } // end GenerateAuthorName();

        internal void printLogins(EntityLogin login)
        {
            if (login == null)
                foreach (EntityLogin loopLogin in Logins)
                    if (loopLogin.postList != null)
                    {
                        Console.WriteLine("\n\n\n\nPosts of user '{0}':", loopLogin.name);
                        foreach (Post post in loopLogin.postList)
                            Console.WriteLine("Thread: {0} Section: {1} Author: {2} Content: {3}", post.ThreadName, post.SectionName, post.Author, post.Content);
                    }
                    else Console.WriteLine("No posts for user {0}.", loopLogin.name);
            else if (login.postList != null)
            {
                Console.WriteLine("\n\n\n\nPosts of user '{0}':", login.name);
                foreach (Post post in login.postList)
                    Console.WriteLine("Thread: {0} Section: {1} Author: {2} Content: {3}", post.ThreadName, post.SectionName, post.Author, post.Content);
            }
            else Console.WriteLine("No posts for user {0}.", login.name);
        } // end printLogins(EntityLogin);

        internal void printProducers(EntityProducer producer)
        {
            if (producer == null)
                foreach (EntityProducer loopProducer in Producers)
                    if (loopProducer.postList != null)
                    {
                        Console.WriteLine("\n\n\n\nPosts containing producer '{0}':", loopProducer.name);
                        foreach (Post post in loopProducer.postList)
                            Console.WriteLine("Thread: {0} Section: {1} Author: {2} Content: {3}", post.ThreadName, post.SectionName, post.Author, post.Content);
                    }
                    else Console.WriteLine("No posts containing producer {0}.", loopProducer.name);
            else if (producer.postList != null)
            {
                Console.WriteLine("\n\n\n\nPosts containing producer '{0}':", producer.name);
                foreach (Post post in producer.postList)
                    Console.WriteLine("Thread: {0} Section: {1} Author: {2} Content: {3}", post.ThreadName, post.SectionName, post.Author, post.Content);
            }
            else Console.WriteLine("No posts containing producer {0}.", producer.name);
        } // end printLogins(EntityProducer);
    }
}
