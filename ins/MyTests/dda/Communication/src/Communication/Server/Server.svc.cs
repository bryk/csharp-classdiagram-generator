using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DBModule;

namespace Server
{
    public class Client
    {
        public String name;
        public IServerCalback callback;
    }

    public class Server : IServer
    {

        private static readonly List<Client> clients = new List<Client>();
        public bool Register(String name)
        {
            IServerCalback callback = OperationContext.Current.GetCallbackChannel<IServerCalback>();

            foreach (Client c in clients)
            {
                if (c.name == name && c.callback.Equals(callback))
                    return false;
            }

            Client cl = new Client();
            cl.name = name;
            cl.callback = callback;
            clients.Add(cl);
            return true;
        }

        public List<dynamic> SendMessageToServer(TypeOfDepth depth, string value)
        {
            List<dynamic> result = new List<dynamic>();
            List<Client> toRemove = new List<Client>();
 
            IServerCalback callback = OperationContext.Current.GetCallbackChannel<IServerCalback>();
            foreach (Client c in clients)
            {
                if (c.callback != callback)
                {
                    if (((ICommunicationObject)c.callback).State == CommunicationState.Opened)
                        result.AddRange(c.callback.SendMessageToClient(depth, value));
                    else toRemove.Add(c);
                }
            }

            foreach (Client c in toRemove)
            {
                clients.Remove(c);
            }
            return result;
        }
    }
}


