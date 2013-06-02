using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Server;

namespace ServerHost
{
    class ServerHost
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(CommunicationService)))
            {
                host.Open();

                Console.WriteLine("Service host running......");
                Console.ReadLine();

                host.Close();
            }
        }
    }
}
