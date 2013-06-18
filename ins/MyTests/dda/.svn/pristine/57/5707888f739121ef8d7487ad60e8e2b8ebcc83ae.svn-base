using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Server;
using System.ServiceModel;
using System.Net;

namespace Client
{
    public interface ICommunication
    {
        List<dynamic> SendMessage(String depth, String postAttribute, String value);
    }

    class ClientServer : ICommunication
    {
        private String name;
        private InstanceContext instanceContext = null;
        private ServerClient communicate = null;

        public ClientServer()
        {
            this.name = Dns.GetHostName(); 
            instanceContext = new InstanceContext(new ServerCallback());
            communicate = new ServerClient(instanceContext);
            if (communicate.Register(name))
                Console.WriteLine("Registered");
            else
                Console.WriteLine("Already exists");
        }

        public List<dynamic> SendMessage(string depth, string postAttribute, string value)
        {
            int dpth = Convert.ToInt32(depth);
            List<dynamic> srchresult =  this.communicate.SendMessageToServer(depth, postAttribute, value);
            List<dynamic> result = new List<dynamic>();

            if (dpth == 3)
                foreach (List<dynamic> l in srchresult)
                    result.Add(new Searcher.Post(l[0], l[1]));
            else if (dpth == 2)
                foreach (List<dynamic> l in srchresult)
                    result.Add(new Searcher.ForumThread(l[0], l[1]));
            else if (dpth == 1)
                return srchresult;

            return result;
        }
    }
}
