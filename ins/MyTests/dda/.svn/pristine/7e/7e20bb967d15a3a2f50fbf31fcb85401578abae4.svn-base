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
        List<dynamic> sendMessage(String depth, String postAttribute, String value);
    }

    class ClientServer : ICommunication
    {
        private String name;
        private InstanceContext instanceContext = null;
        public ServerClient communicate = null;

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

        public List<dynamic> sendMessage(string depth, string postAttribute, string value)
        {
            int dpth = Convert.ToInt32(depth);
            List<dynamic> srchresult =  this.communicate.sendMessageToServer(depth, postAttribute, value);
            List<dynamic> result = new List<dynamic>();

            if (dpth == 3)
                foreach (List<dynamic> l in srchresult)
                    result.Add(new Post(l[0], l[1]));
            else if (dpth == 2)
                foreach (List<dynamic> l in srchresult)
                    result.Add(new ForumThread(l[0], l[1]));
            else if (dpth == 1)
                return srchresult;

            return result;
        }
    }
}
