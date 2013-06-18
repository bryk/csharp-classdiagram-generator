using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyAnalyzer.Analyzer.DBModule
{   
    /// <summary>
    /// Imitates database behavior 
    /// </summary>
    class DataBaseHandler
    {
        private static Dictionary<Tuple<String, String>, int> dependencies = null;
        private static List<EntityProducer> producers;
        public static void InitDependencies()
        {
            dependencies = new Dictionary<Tuple<String, String>, int>();
            dependencies.Add(new Tuple<string, string>("login1", "login2"), 53);
            dependencies.Add(new Tuple<string, string>("login1", "login3"), 64);
            dependencies.Add(new Tuple<string, string>("login1", "login4"), 5);
            dependencies.Add(new Tuple<string, string>("login1", "login5"), 625);
            dependencies.Add(new Tuple<string, string>("login2", "login3"), 452);
            dependencies.Add(new Tuple<string, string>("login2", "login4"), 7);
            dependencies.Add(new Tuple<string, string>("login2", "login5"), 4);
            dependencies.Add(new Tuple<string, string>("login3", "login4"), 0);
            dependencies.Add(new Tuple<string, string>("login3", "login5"), 67);
            dependencies.Add(new Tuple<string, string>("login4", "login5"), 1);
        }
        public static void InitProducers()
        {
            producers = new List<EntityProducer>();
            EntityProducer pr1 = new EntityProducer("prod1");
            EntityProducer pr2 = new EntityProducer("prod2");
            EntityProducer pr3 = new EntityProducer("prod3");
            EntityProducer pr4 = new EntityProducer("prod4");
            EntityProducer pr5 = new EntityProducer("prod5");
            EntityProducer pr6 = new EntityProducer("prod6");
            EntityProducer pr7 = new EntityProducer("prod7");
            EntityProducer pr8 = new EntityProducer("prod8");
            EntityProducer pr9 = new EntityProducer("prod9");
            EntityProducer pr10 = new EntityProducer("prod10");
            EntityProducer pr11 = new EntityProducer("prod11");
            EntityProducer pr12 = new EntityProducer("prod12");
            EntityProducer pr13 = new EntityProducer("prod13");
            EntityProducer pr14 = new EntityProducer("prod14");
            EntityProducer pr15 = new EntityProducer("prod15");
            producers.Add(pr1);
            producers.Add(pr2);
            producers.Add(pr3);
            producers.Add(pr4);
            producers.Add(pr5);
            producers.Add(pr6);
            producers.Add(pr7);
            producers.Add(pr8);
            producers.Add(pr9);
            producers.Add(pr10);
            producers.Add(pr11);
            producers.Add(pr12);
            producers.Add(pr13);
            producers.Add(pr14);
            producers.Add(pr15);
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
            pr1.addPost(p1);
            pr2.addPost(p1);
            pr3.addPost(p1);
            pr4.addPost(p1);
            pr5.addPost(p1);
            pr1.addPost(p21);
            pr1.addPost(p22);
            pr1.addPost(p23);
            pr1.addPost(p24);
            pr1.addPost(p25);

            pr1.addPost(p6);
            pr2.addPost(p6);

            pr1.addPost(p11);
            pr2.addPost(p11);
            pr3.addPost(p11);

            pr1.addPost(p16);
            pr2.addPost(p16);
            pr3.addPost(p16);
            pr4.addPost(p16);
            
            pr10.addPost(p16);
            pr10.addPost(p15);

            pr11.addPost(p16);
            pr11.addPost(p8);
            
            pr12.addPost(p16);
            pr12.addPost(p3);
        }

        /// <summary>
        /// Retrieves dependencies from database 
        /// </summary>
        /// <returns>a dataset, 
        /// that contains all counted dependencies (and additional informations)
        /// </returns>
        public static Dictionary<Tuple<String, String>, int> GetDependencies()
        {
            if (dependencies == null) dependencies = new Dictionary<Tuple<string, string>, int>();//InitDependencies();
            return dependencies;
        }


        /// <summary>
        ///  Connects to the database and attempts to save dependencies
        /// </summary>
        /// <param name="dictionary">a dataset
        /// that contains all counted dependencies (and additional informations)</param>
        public static void saveDependencies(Dictionary<Tuple<String, String>, int> dictionary)
        {
            dependencies = dictionary;
        }
        /// <summary>
        /// Retrieves List of all producers from database
        /// </summary>
        /// <returns></returns>
        public static List<EntityProducer> getProducers()
        {
            if (producers == null) InitProducers();
            return  producers;
        }
    }
}
