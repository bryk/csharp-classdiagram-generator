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
    class Program
    {
        static void Main(string[] args)
        {
            ClientServer c = new ClientServer(new Searcher.Searcher());

            List<dynamic> result = null;


            while (true)
            {
                Console.WriteLine("Select search mode: (s)ection, (t)hread, (p)ost");
                string searchMode = Console.ReadLine();

                if (searchMode.Equals("s"))
                    result = c.SendMessage(TypeOfDepth.Section, "whatever");
                else if (searchMode.Equals("t"))
                    result = c.SendMessage(TypeOfDepth.Thread, "whatever");
                else if (searchMode.Equals("p"))
                    result = c.SendMessage(TypeOfDepth.Post, "whatever");
                else
                {
                    Console.WriteLine("Wrong option. Exiting");
                    Environment.Exit(0);
                }


                foreach (object o in result)
                    Console.WriteLine(o.ToString());
            }


        }
    }
}
