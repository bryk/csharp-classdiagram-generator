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

    public enum TypeOfDepth
    {
        Post, ForumThread, Section
    }

    public class EntityHandler:IDataBaseSearcher
    {
        MongoCollection<Producer> producerList;

        public List<dynamic> ReceiveResults(TypeOfDepth type, String value)
        {
            if (type == TypeOfDepth.ForumThread)
            {
                var _value = value;
                var query = Query.EQ("name", value);
                Producer temp = producerList.FindOne(query);
                if (temp == null)
                {
                    return null;
                }
                else
                {
                    if (temp.GetThreads() == null)
                    {
                        return null;
                    }
                    else
                    {
                        return temp.GetThreads().ToList<dynamic>();
                    }
                }
            }
            else if (type == TypeOfDepth.Post)
            {
                var _value = value;
                var query = Query.EQ("name", _value);
                Producer temp = producerList.FindOne(query);
                if (temp == null)
                {
                    return null;
                }
                else
                {
                    if (temp.GetPosts() == null)
                    {
                        return null;
                    }
                    else
                    {
                        return temp.GetPosts().ToList<dynamic>();
                    }
                }
            }
            else if (type == TypeOfDepth.Section)
            {
                var _value = value;
                var query = Query.EQ("name", value);
                Producer temp = producerList.FindOne(query);
                if (temp == null)
                {
                    return null;
                }
                else
                {
                    if (temp.GetSections() == null)
                    {
                        return null;
                    }
                    else
                    {
                        return temp.GetSections().ToList<dynamic>();
                    }
                }
            }
            else return null;
        }

        public Boolean IsInList(string name)
        {
            var _name = name;
            var query = Query.EQ("name", _name);
            Producer temporary = producerList.FindOne(query);
            if (temporary != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean AddPost(string key, Post post)
        {
            try
            {
                if (IsInList(key))
                {
                    var _name = key;
                    var query = Query.EQ("name", _name);
                    Producer temp = producerList.FindOne(query);
                    producerList.Remove(query);
                    if (temp.GetPosts() == null) temp.AddPost2(post); else temp.AddPost(post);
                    producerList.Insert(temp);
                }
                else
                {
                    Producer temp = new Producer(key);
                    temp.AddPost(post);
                    producerList.Insert(temp);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean AddThread(string key, ForumThread thread)
        {
            try
            {
                if (IsInList(key))
                {
                    var _name = key;
                    var query = Query.EQ("name", _name);
                    Producer temp = producerList.FindOne(query);
                    producerList.Remove(query);
                    if (temp.GetThreads() == null) temp.AddThread2(thread); else temp.AddThread(thread);
                    producerList.Insert(temp);
                }
                else
                {
                    Producer temp = new Producer(key);
                    temp.AddThread(thread);
                    producerList.Insert(temp);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean AddSection(string key, Section section)
        {
            try
            {
                if (IsInList(key))
                {
                    var _name = key;
                    var query = Query.EQ("name", _name);
                    Producer temp = producerList.FindOne(query);
                    producerList.Remove(query);
                    if (temp.GetSections() == null) temp.AddSection2(section); else temp.AddSection(section);
                    producerList.Insert(temp);
                }
                else
                {
                    Producer temp = new Producer(key);
                    temp.AddSection(section);
                    producerList.Insert(temp);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public EntityHandler()
        {
            producerList = DBConnection.db.GetCollection<Producer>("producerList");
        }
    }

}
