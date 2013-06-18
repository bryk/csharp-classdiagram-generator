using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectSearching;

namespace ProjectDatabase
{
    public class DataBase : IDataBase
    {
        public DataBase()
        {
        }

        public List<dynamic> ReceiveResults(TypeOfDepth depth, String value)
        {
            switch (depth)
            {
                case TypeOfDepth.Section:
                    {
                        List<dynamic> list = new List<dynamic>();

                        list.Add(new Section("TV Serwis"));
                        list.Add(new Section("Monitor Serwis"));
                        return list;
                        //break;
                    }
                case TypeOfDepth.Thread:
                    {
                        List<dynamic> list = new List<dynamic>();
                        list.Add(new ForumThread("TV Serwis", "Pilot"));
                        list.Add(new ForumThread("TV Serwis", "Sharp 40LE brak obrazu"));
                        list.Add(new ForumThread("Monitor Serwis", "Menu monitora pytanie"));
                        return list;
                    }

                case TypeOfDepth.Post:
                    {
                        List<dynamic> list = new List<dynamic>();
                        list.Add(new Post("Pilot", "TV Serwis", "andrzej91", "Mam problem", new DateTime(2012, 10, 1)));
                        list.Add(new Post("Pilot", "TV Serwis", "jola29", "Mam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tamMam rozwiazanie tego proble polegajace na tym, że coś tam, coś tam", new DateTime(2012, 10, 1)));
                        list.Add(new Post("Pilot", "TV Serwis", "91kazek", "Mozna inaczej", new DateTime(2012, 10, 1)));
                        list.Add(new Post("Menu monitora pytanie", "Monitor serwis", "kubus", "Pomocy", new DateTime(2012, 10, 1)));
                        list.Add(new Post("Sharp 40LE brak obrazu", "TV Serwis", "kubus", "Juz wiem", new DateTime(2012, 10, 1)));
                        return list;
                    }
            }
            return null;
        }
    }
}
