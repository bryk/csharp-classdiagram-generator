using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Searcher
{
    public class ForumThread
    {
        public string topicName{get; set;}
        public string sectionName { get; set; }

        public ForumThread(string topicName, string sectionName)
        {
            this.topicName = topicName;
            this.sectionName = sectionName;
        }

        public override string ToString()
        {
            return base.ToString() + ": "+ this.topicName+" "+this.sectionName;
        } 
    }

    public class Post
    {
        public string topicName{get; set;}
        public string sectionName{get; set;}

        public Post(string topicName, string sectionName)
        {
            this.topicName = topicName;
            this.sectionName = sectionName;
        }

        public override string ToString()
        {
            return base.ToString() + ": " + this.topicName + " " + this.sectionName;
        }
    }

    public interface ISearcher
    {
        List<dynamic> GetMessage(string depth, string attribute, string value);
    }

    public class Searcher : ISearcher
    {
        public List<dynamic> GetMessage(string depth, string attribute, string value)
        {
            List<dynamic> result = new List<dynamic>();

            int dpth = Convert.ToInt32(depth);
            if (dpth == 1)
            {
                result.Add("dzial1");
                result.Add("dzial2");
            }
            else if (dpth == 2)
            {
                result.Add(new ForumThread("topic1", "section1"));
                result.Add(new ForumThread("topic2", "section2"));
            }
            else if (dpth == 3)
            {
                result.Add(new Post("topic1", "section1"));
                result.Add(new Post("topic2", "section2"));
            }

            return result;
        }
    }
}
