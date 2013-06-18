using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuiAnalyzer.DependencyAnalyzer.DBModule;

namespace GuiAnalyzer.DependencyAnalyzer
{
    class DependencyAnalyzer
    {
        public static Dictionary<Tuple<String, String>, int> dependencies;
        static bool inited=false;
        public List<EntityLogin> Logins { get; set; }
        public List<EntityProducer> Producers { get; set; }
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
        static void init(){
            inited = true;
            DataGenerator generator = new DataGenerator();
            Analyzer.countDependencies(generator.Logins, generator.Producers);
        }
        public static Dictionary<Tuple<String, String>, int> GetDependencies()
        {
            //DependencyAnalyzer.InitDependencies();
            if (!inited) init(); ;
            return DependencyAnalyzer.dependencies;
        }


        public static void saveDependencies(Dictionary<Tuple<String, String>, int> dictionary)
        {
            dependencies = dictionary;
        }
    }
}
