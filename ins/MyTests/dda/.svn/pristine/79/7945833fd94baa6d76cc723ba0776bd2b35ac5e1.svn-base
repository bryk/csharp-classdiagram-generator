using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Server;
using System.ServiceModel;
using System.Net;
using Searcher;
using DBModule;

namespace Client
{
    public interface ICommunication
    {
        List<dynamic> SendMessage(TypeOfDepth depth, String value);
    }

    public class ClientServer : ICommunication
    {
        private String name;
        private InstanceContext instanceContext = null;
        private ServerClient communicate = null;
        private static ISearcher srch;

        public ClientServer(ISearcher srch)
        {
            ClientServer.srch = srch;
            this.name = Dns.GetHostName(); 
            instanceContext = new InstanceContext(new ServerCallback());
            communicate = new ServerClient(instanceContext);
            if (communicate.Register(name))
                Console.WriteLine("Registered");
            else
                Console.WriteLine("Already exists");
        }

        public static ISearcher getSearcher()
        {
            return srch;
        }

        public List<dynamic> SendMessage(TypeOfDepth depth, string value)
        {
            List<dynamic> srchresult =  this.communicate.SendMessageToServer(depth, value);
            Console.WriteLine("Sent");
            List<dynamic> result = new List<dynamic>();

            if (depth == TypeOfDepth.Post)
                foreach (List<dynamic> l in srchresult)
                    result.Add(new Post(l[0], l[1], l[2], l[3], l[4]));
            else if (depth == TypeOfDepth.Thread)
                foreach (List<dynamic> l in srchresult)
                    result.Add(new ForumThread(l[0], l[1]));
            else if (depth == TypeOfDepth.Section)
                foreach (List<dynamic> l in srchresult)
                    result.Add(new Section(l[0]));

            result.AddRange(srch.ReceiveMessage(depth, value));

            return result;
        }
    }
}
