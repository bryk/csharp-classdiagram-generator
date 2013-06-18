using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectSearching;
using ProjectDatabase;

namespace ProjectCommunication
{
    public class ClientServer:ICommunication
    {
        private ISearcher Searcher { get; set; }

        public ClientServer(ISearcher searcher)
        {
            Searcher = searcher;
        }

        public List<dynamic> SendMessage(TypeOfDepth depth, String value)
        {
            return Searcher.ReceiveMessage(depth, value);
        }
    }
}
