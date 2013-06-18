using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectSearching;
using ProjectDatabase;

namespace ProjectCommunication
{
    public class Communication : ICommunication
    {
        public List<dynamic> SendMessage(TypeOfDepth depth, String value)
        {
            Searcher searcher = new Searcher();
            searcher.DataBase = new DatabaseCommunication();
            return searcher.ReceiveMessage(depth, value);
        }
    }
}
