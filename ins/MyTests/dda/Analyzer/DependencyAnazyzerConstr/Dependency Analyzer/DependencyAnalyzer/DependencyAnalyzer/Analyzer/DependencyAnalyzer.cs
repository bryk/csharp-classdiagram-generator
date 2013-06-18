using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DependencyAnalyzer.Analyzer;
using System.Threading;
using DBModule;

namespace DependencyAnalyzer.Analyzer
{
    class DependencyAnalyzer
    {
        private int SleepTime;
        public Analyzer AnalyzerProxy;
        
        public DependencyAnalyzer(int sleepTime, IDataBaseAnalyser i)
        {
            SleepTime = sleepTime;
            AnalyzerProxy = new Analyzer(i);
        }
        public void Run()
        {
            List<Producer> producers = null;
            Dictionary<Tuple<string,string>,int> dependencies;
            while (true)
            {

                producers = AnalyzerProxy.GetProducers();
                dependencies = AnalyzerProxy.GetDependencies();
                if (producers != null && dependencies != null)
                {
                    AnalyzerProxy.saveDependencies(countDependencies(producers, dependencies));
                }
                Thread.Sleep(SleepTime);
            }
        }
        public Dictionary<Tuple<String, String>, int> countDependencies(List<Producer> producers, Dictionary<Tuple<String, String>, int> oldDependencies)
        {
            if (oldDependencies == null)
                oldDependencies = new Dictionary<Tuple<string, string>, int>();
            System.Console.WriteLine("Counting dependencies");

            if (producers != null)
            {
                String[] producerNames = new String[producers.Count];
                HashSet<string> logins = new HashSet<string>();
                int i = 0;
                foreach (Producer p in producers)
                    producerNames[i++] = p.name;
                Array.Sort(producerNames);
                foreach (Producer p in producers)
                    foreach (Post ps in p.postList)
                        logins.Add(ps.Author);

                Dictionary<string, Dictionary<string, int>> loginProducerCount = new Dictionary<string, Dictionary<string, int>>();
                foreach (String loginName in logins)
                {
                    Dictionary<string, int> d = new Dictionary<string, int>();
                    foreach (string producerName in producerNames)
                        d.Add(producerName, 0);
                    loginProducerCount.Add(loginName, d);
                }
                Dictionary<string, int> dict;
                int count;
                foreach (Producer producer in producers)
                {
                    if (oldDependencies.TryGetValue(new Tuple<string, string>("", producer.name), out count) && (-count) == producer.postList.Count)
                    {
                        foreach (string login in logins)
                        {
                            loginProducerCount.TryGetValue(login, out dict);
                            dict.Remove(producer.name);
                            count = 0;
                            if (!oldDependencies.TryGetValue(new Tuple<string, string>(login, producer.name), out count))
                                count = 0;
                            dict.Add(producer.name, -count);
                        }
                    }
                    else
                        foreach (Post post in producer.postList)
                        {
                            if (!oldDependencies.TryGetValue(new Tuple<string, string>(post.Id, ""), out count))
                                oldDependencies.Add(new Tuple<string, string>(post.Id, ""), -1);
                            loginProducerCount.TryGetValue(post.Author, out dict);
                            dict.TryGetValue(producer.name, out count);
                            dict.Remove(producer.name);
                            dict.Add(producer.name, count + 1);
                        }
                }
                Dictionary<Tuple<String, String>, int> dependencies = new Dictionary<Tuple<string, string>, int>();
                foreach (string login1 in logins)
                    foreach (string login2 in logins)
                        if (0 > login1.CompareTo(login2))
                        {
                            int value = 0, a, b;

                            Dictionary<string, int> d1;
                            Dictionary<string, int> d2;
                            loginProducerCount.TryGetValue(login1, out d1);
                            loginProducerCount.TryGetValue(login2, out d2);
                            foreach (string producer in producerNames)
                            {
                                d1.TryGetValue(producer, out a);
                                d2.TryGetValue(producer, out b);
                                value += Math.Min(a, b);
                            }
                            if (value > 0)
                            {
                                oldDependencies.Remove(new Tuple<string, string>(login1, login2));
                                oldDependencies.Add(new Tuple<string, string>(login1, login2), value);
                            }
                        }
                foreach (string login in logins)
                {
                    Dictionary<string, int> d;
                    loginProducerCount.TryGetValue(login, out d);
                    foreach (string producer in producerNames)
                    {
                        int value = 0;
                        d.TryGetValue(producer, out value);
                        oldDependencies.Remove(new Tuple<string, string>(login, producer));
                        oldDependencies.Add(new Tuple<string, string>(login, producer), -value);
                    }
                }
                foreach (Producer p in producers)
                {
                    oldDependencies.Remove(new Tuple<string, string>("", p.name));
                    oldDependencies.Add(new Tuple<string, string>("", p.name), -p.postList.Count);

                }
            }
            return oldDependencies;
        }
        public Dictionary<Tuple<string, string>, int> countDependencies(List<Producer> producers)
        {
            System.Console.WriteLine("Counting dependencies");
            String[] producerNames = new String[producers.Count];
            HashSet<string> logins = new HashSet<string>();
            int i = 0;
            foreach (Producer p in producers)
                producerNames[i++] = p.name;
            Array.Sort(producerNames);
            foreach (Producer p in producers)
                foreach (Post ps in p.postList)
                    logins.Add(ps.Author);

            Dictionary<string, Dictionary<string, int>> loginProducerCount = new Dictionary<string, Dictionary<string, int>>();
            foreach (String loginName in logins)
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                foreach (string producerName in producerNames)
                    d.Add(producerName, 0);
                loginProducerCount.Add(loginName, d);
            }
            Dictionary<string, int> dict;
            int count;
            Dictionary<Tuple<String, String>, int> dependencies = new Dictionary<Tuple<string, string>, int>();
            foreach (Producer producer in producers)
            {
                foreach (Post post in producer.postList)
                {
                    if (!dependencies.TryGetValue(new Tuple<string, string>(post.Id, ""), out count))
                        dependencies.Add(new Tuple<string, string>(post.Id, ""), -1);
                    loginProducerCount.TryGetValue(post.Author, out dict);
                    dict.TryGetValue(producer.name, out count);
                    dict.Remove(producer.name);
                    dict.Add(producer.name, count + 1);
                }
            }
            
            foreach (string login1 in logins)
                foreach (string login2 in logins)
                    if (0 > login1.CompareTo(login2))
                    {
                        int value = 0, a, b;

                        Dictionary<string, int> d1;
                        Dictionary<string, int> d2;
                        loginProducerCount.TryGetValue(login1, out d1);
                        loginProducerCount.TryGetValue(login2, out d2);
                        foreach (string producer in producerNames)
                        {
                            d1.TryGetValue(producer, out a);
                            d2.TryGetValue(producer, out b);
                            value += Math.Min(a, b);
                        }
                        if (value > 0)
                            dependencies.Add(new Tuple<string, string>(login1, login2), value);
                    }
            foreach (string login in logins)
            {
                Dictionary<string, int> d;
                loginProducerCount.TryGetValue(login, out d);
                foreach (string producer in producerNames)
                {
                    int value = 0;
                    d.TryGetValue(producer, out value);
                    dependencies.Add(new Tuple<string, string>(login, producer), -value);
                }
            }
            foreach (Producer p in producers)
            {
                dependencies.Add(new Tuple<string, string>("", p.name), -p.postList.Count);
            }
            return dependencies;
        }
    }
}
