using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectSearching;

namespace ProjectDatabase
{
    public class DatabaseCommunication : IDataBase
    {
        public DatabaseCommunication()
        {
        }

        public List<dynamic> ReceiveResults(TypeOfDepth depth, String value)
        {
            switch (depth)
            {
                case TypeOfDepth.Section:
                    {
                        List<dynamic> list = new List<dynamic>();

                        list.Add(new Section("TV Serwis [FROM COMMUNICATION]"));
                        list.Add(new Section("Monitor Serwis"));
                        return list;
                        //break;
                    }
                case TypeOfDepth.Thread:
                    {
                        List<dynamic> list = new List<dynamic>();
                        list.Add(new ForumThread("TV Serwis [FROM COMMUNICATION]", "Pilot [FROM COMMUNICATION]"));
                        list.Add(new ForumThread("TV Serwis [FROM COMMUNICATION]", "Sharp 40LE brak obrazu [FROM COMMUNICATION]"));
                        list.Add(new ForumThread("Monitor Serwis", "Menu monitora pytanie"));
                        return list;
                    }

                case TypeOfDepth.Post:
                    {
                        List<dynamic> list = new List<dynamic>();
                        list.Add(new Post("Pilot [FROM COMMUNICATION]", "TV Serwis [FROM COMMUNICATION]", "andrzej91", "Mam problem [FROM COMMUNICATION]", new DateTime(2012, 10, 1)));
                        list.Add(new Post("Pilot", "TV Serwis", "jola29", "Mam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tam", new DateTime(2012, 10, 1)));
                        list.Add(new Post("Pilot [FROM COMMUNICATION]", "TV Serwis [FROM COMMUNICATION]", "91kazek", "Mozna inaczej [FROM COMMUNICATION]", new DateTime(2012, 10, 1)));
                        list.Add(new Post("Menu monitora pytanie", "Monitor serwis", "kubus", "Pomocy", new DateTime(2012, 10, 1)));
                        list.Add(new Post("Sharp 40LE brak obrazu [FROM COMMUNICATION]", "TV Serwis [FROM COMMUNICATION]", "kubus", "Juz wiem [FROM COMMUNICATION]", new DateTime(2012, 10, 1)));
                        list.Add(new Post("Pilot", "TV Serwis", "dodane", "dodane", new DateTime(2012, 10, 1)));
                        return list;
                    }
            }
            return null;
        }
    }
}
