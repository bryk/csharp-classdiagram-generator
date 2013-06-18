using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Server;
using Searcher;
using DBModule;


namespace Client
{
    class ServerCallback : IServerCallback
    {
        public List<dynamic> SendMessageToClient(TypeOfDepth depth, string value)
        {
            List<dynamic> result = new List<dynamic>();
            List<dynamic> srchresult = null;

            Console.WriteLine("Message from server: " + depth + " " + value);
            ISearcher srch = ClientServer.getSearcher();
            srchresult = srch.ReceiveMessage(depth, value);

            if (depth == TypeOfDepth.Post)
                foreach (Post p in srchresult)
                    result.Add(new List<dynamic>() { p.ThreadName, p.SectionName, p.Author, p.Content, p.dateTime});
            else if (depth == TypeOfDepth.Thread)
                foreach (ForumThread f in srchresult)
                    result.Add(new List<dynamic>() { f.ThreadName, f.SectionName });
            else if (depth == TypeOfDepth.Section)
                foreach (Section s in srchresult)
                    result.Add(new List<dynamic>() {s.SectionName});

            return result;
            
        }
    }
}
