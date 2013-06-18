using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Server;
using System.ServiceModel;
using System.Net;


namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientServer c = new ClientServer();

            List<dynamic> result = null;



            while (true)
            {
                Console.WriteLine("Select search mode: (s)ection, (t)hread, (p)ost");
                string searchMode = Console.ReadLine();

                if (searchMode.Equals("s"))
                    result = c.SendMessage("1", "whatever", "whatever");
                else if (searchMode.Equals("t"))
                    result = c.SendMessage("2", "whatever", "whatever");
                else if (searchMode.Equals("p"))
                    result = c.SendMessage("3", "whatever", "whatever");
                else
                {
                    Console.WriteLine("Wrong option. Exiting");
                    Environment.Exit(0);
                }

                Console.WriteLine("Message sent");

                foreach (object o in result)
                    Console.WriteLine(o.ToString());
            }


        }
    }
}
