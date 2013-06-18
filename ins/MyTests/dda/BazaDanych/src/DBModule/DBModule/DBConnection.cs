using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DBModule
{
    public class DBConnection
    {

        public static MongoDatabase db;

        public DBConnection()
        {
            try
            {
                var connectionString = "mongodb://localhost";
                var client = new MongoClient(connectionString);
                var mongo = client.GetServer();
                db = mongo.GetDatabase("DBModule");
            }
            catch (Exception)
            {
                Console.WriteLine("A problem with connection to DataBase occured");
            }

        }
    }
}