using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Server;
using Searcher;


namespace Client
{
    class ServerCallback : IServerCallback
    {
        public List<dynamic> SendMessageToClient(string depth, string postAttribute, string value)
        {
            int dpth = Convert.ToInt32(depth);
            List<dynamic> result = new List<dynamic>();
            List<dynamic> srchresult = null;

            Console.WriteLine("Message from server: " + depth + " " + postAttribute + " " + value);
            Searcher.Searcher srch = new Searcher.Searcher();
            srchresult = srch.GetMessage(depth, postAttribute, value);

            if (dpth == 3)
                foreach (Post p in srchresult)
                    result.Add(new List<dynamic>() { p.sectionName, p.topicName });
            else if (dpth == 2)
                foreach (ForumThread f in srchresult)
                    result.Add(new List<dynamic>() { f.sectionName, f.topicName });
            else if (dpth == 1)
                return srchresult;

            return result;
            
        }
    }
}
