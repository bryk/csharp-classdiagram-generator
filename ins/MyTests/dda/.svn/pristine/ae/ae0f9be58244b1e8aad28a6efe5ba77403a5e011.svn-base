using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBModule;

namespace Searcher
{
    public interface ISearcher
    {
        List<dynamic> ReceiveMessage(TypeOfDepth depth, string value);
    }

    public class Searcher : ISearcher
    {
        public List<dynamic> ReceiveMessage(TypeOfDepth depth, string value)
        {
            List<dynamic> result = new List<dynamic>();

            if (depth == TypeOfDepth.Section)
            {
                result.Add(new Section("section1"));
                result.Add(new Section("section2"));
            }
            else if (depth == TypeOfDepth.Thread)
            {
                result.Add(new ForumThread("section1", "thread1"));
                result.Add(new ForumThread("section2", "thread2"));
            }
            else if (depth == TypeOfDepth.Post)
            {
                result.Add(new Post("thread1", "section1", "author1", "content1", DateTime.Now));
                result.Add(new Post("thread2", "section2", "author2", "content2", DateTime.Now));
            }

            return result;
        }
    }
}
