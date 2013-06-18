using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuiAnalyzer.DependencyAnalyzer.DBModule;

namespace GuiAnalyzer.DependencyAnalyzer
{
    class Analyzer
    {

        /* Methods */
        public static void countDependencies(List<EntityLogin> logins, List<EntityProducer> producers)
        {
            Dictionary<Tuple<String, String>, int> dictionary = new Dictionary<Tuple<String, String>, int>();
            string[] authorList = null;
            int i;
            foreach (EntityProducer producer in producers)
            { // każdy producent zapisuje nazwy autorów postów do tablicy stringów - są to autorzy którzy wypowiadali się na temat danego producenta
                authorList = new string[producer.postList.Count];
                i = 0;
                foreach (Post post in producer.postList)
                    if (!authorList.Contains(post.Author))
                        authorList[i++] = post.Author;
                evaluateDependencies(authorList, i, dictionary); // biorę tablicę z autorami i pakuję ich do funkcji zliczającej zależności
                authorList = null;
            }

            DependencyAnalyzer.saveDependencies(dictionary);
        } // end countDependencies(List<EntityLogin>,List<EntityProducer>);

        private static void evaluateDependencies(string[] authorList, int size, Dictionary<Tuple<String, String>, int> dictionary)
        { // każdy autor zostanie zewaluowany z każdym dokładnie jeden raz
            for (int i = 0; i < size; ++i)
                for (int j = i + 1; j < size; ++j)
                {
                    Tuple<String, String> tuple = new Tuple<String, String>(authorList[i], authorList[j]);
                    if (dictionary.ContainsKey(tuple)) ++dictionary[tuple];
                    else dictionary.Add(tuple, 1);
                }
        } // end evaluateDependencies(string[],Dictionary<Tuple<String,String>,int>);

    }
}
