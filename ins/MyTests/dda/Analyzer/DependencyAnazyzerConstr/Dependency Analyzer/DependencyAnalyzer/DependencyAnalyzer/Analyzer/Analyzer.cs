using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBModule;

namespace DependencyAnalyzer.Analyzer
{
    class Analyzer
    {
        public IDataBaseAnalyser requestHandler;
        public Analyzer(IDataBaseAnalyser iDataBaseAnalyser)
        {
            requestHandler = iDataBaseAnalyser;
        }
        /// <summary>
        /// Retrieves dependencies from database 
        /// </summary>
        /// <returns>a dataset, 
        /// that contains all counted dependencies (and additional informations)
        /// </returns>
        //
        public Dictionary<Tuple<string, string>, int> GetDependencies()
        {
            try
            {
                if (requestHandler == null) return new Dictionary<Tuple<string, string>, int>();
                else
                {
                    Dictionary<Tuple<string, string>, int> dictionary = requestHandler.GetDependencies();
                    return dictionary;
                }
            }
            catch (Exception e) { Console.WriteLine("NULL"); }
            return new Dictionary<Tuple<string,string>,int> ();
        }

        public void FilterPositiveDependencies(Dictionary<Tuple<string, string>, int> dictionary, out Dictionary<Tuple<string, string>, int> positive, out Dictionary<Tuple<string, string>, int> notPositive)
        {
            Dictionary<Tuple<string, string>, int> d = new Dictionary<Tuple<string, string>, int>();
            Dictionary<Tuple<string, string>, int> n = new Dictionary<Tuple<string, string>, int>();
            int value;
            foreach (Tuple<string, string> t in dictionary.Keys)
            {
                value = 0;
                dictionary.TryGetValue(t, out value);
                if (value > 0)
                    d.Add(t, value);
                else
                    n.Add(t, value);
            }
            positive = d;
            notPositive = n;
        }

        /// <summary>
        ///  Connects to the database and attempts to save dependencies
        /// </summary>
        /// <param name="dictionary">a dataset
        /// that contains all counted dependencies (and additional informations)</param>
        public void saveDependencies(Dictionary<Tuple<string, string>, int> dictionary)
        {
            if (requestHandler == null) return;
            requestHandler.SaveDependencies(dictionary);
        }
        /// <summary>
        /// Retrieves List of all producers from database
        /// </summary>
        public List<Producer> GetProducers()
        {
            if (requestHandler == null) return new List<Producer>();
            return requestHandler.GetProducers();
        }
    }
}
