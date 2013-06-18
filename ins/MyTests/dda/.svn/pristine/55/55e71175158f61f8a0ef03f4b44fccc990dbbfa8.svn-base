using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyAnalyzer
{
	class DependencyAnalyser
	{
		/* Methods */
		public void countDependencies ( List<EntityLogin> logins, List<EntityProducer> producers )
		{
			Dictionary<Tuple<String, String>, int> dictionary = new Dictionary<Tuple<String, String>, int>();
			string [] authorList = null;
			int i;
			foreach ( EntityProducer producer in producers )
			{ // każdy producent zapisuje nazwy autorów postów do tablicy stringów - są to autorzy którzy wypowiadali się na temat danego producenta
				authorList = new string [producer.postList.Count];
				i = 0;
				foreach ( Post post in producer.postList )
					if ( !authorList.Contains(post.Author) ) 
						authorList [i++] = post.Author;
				evaluateDependencies(authorList, i, dictionary); // biorę tablicę z autorami i pakuję ich do funkcji zliczającej zależności
				authorList = null;
			}

			saveDependencies(dictionary);
		} // end countDependencies(List<EntityLogin>,List<EntityProducer>);

		private void evaluateDependencies ( string [] authorList, int size, Dictionary<Tuple<String, String>, int> dictionary )
		{ // każdy autor zostanie zewaluowany z każdym dokładnie jeden raz
			for ( int i = 0; i < size; ++i )
				for ( int j = i + 1; j < size; ++j )
				{
					Tuple<String, String> tuple = new Tuple<String, String>(authorList [i], authorList [j]);
					if ( dictionary.ContainsKey(tuple) ) ++dictionary [tuple];
					else dictionary.Add(tuple, 1);
				}
		} // end evaluateDependencies(string[],Dictionary<Tuple<String,String>,int>);

		private static void saveDependencies ( Dictionary<Tuple<String, String>, int> dictionary )
		{
			foreach ( KeyValuePair<Tuple<String, String>, int> pair in dictionary )
				Console.WriteLine("User {0} and user {1} : {2}", pair.Key.Item1, pair.Key.Item2, pair.Value);
		} // end saveDependencies(Dictionary<Tuple<String,String>,int>);
	}
}
