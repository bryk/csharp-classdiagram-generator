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
    public class PackedMap
    {
        public ObjectId id;
        public Dictionary<Tuple<String, String>, int> map;
    }

    public class AnalyserMap:IDataBaseAnalyser
    {
        public MongoCollection<PackedMap> dictionary;

        public PackedMap PackMap(Dictionary<Tuple<String, String>, int> newMap)
        {
            PackedMap packedMap = new PackedMap { map = newMap };
            return packedMap;
        }

        public List<Producer> GetProducers()
        {
            return null;
        }

        public Dictionary<Tuple<String, String>, int> UnpackMap(PackedMap packedMap)
        {
            Dictionary<Tuple<String, String>, int> unpackedMap = packedMap.map;
            return unpackedMap;
        }

        public AnalyserMap()
        {
            dictionary = DBConnection.db.GetCollection<PackedMap>("AnalyserMap");
        }

        public Dictionary<Tuple<String, String>, int> GetDependencies()
        {
            PackedMap packed = dictionary.FindOne();
            if (packed != null) return UnpackMap(packed); else return null;
        }

        public void SaveDependencies(Dictionary<Tuple<String, String>, int> dependencies)
        {
            PackedMap packed = dictionary.FindOne();
            if (packed != null)
            {
                var _id1 = packed.id;
                var query = Query.EQ("_id", _id1);
                dictionary.Remove(query);
            }
            PackedMap packedMap = PackMap(dependencies);
            dictionary.Insert(packedMap);
        }
    }
}
