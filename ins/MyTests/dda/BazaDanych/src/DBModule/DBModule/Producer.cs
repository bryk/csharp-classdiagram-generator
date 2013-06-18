using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace DBModule
{
    public class Producer
    {
        public ObjectId _id;
        public string name;
        public List<Post> postList;
        public List<ForumThread> threadList;
        public List<Section> sectionList;

        public Producer(string name)
        {
            this.name = name;
            postList = new List<Post>();
            threadList = new List<ForumThread>();
            sectionList = new List<Section>();
        }

        public void AddPost(Post post)
        {
            postList.Add(post);
        }

        public void AddPost2(Post post)
        {
            postList = new List<Post>();
            postList.Add(post);
        }

        public void AddThread(ForumThread thread)
        {
            threadList.Add(thread);
        }

        public void AddThread2(ForumThread thread)
        {
            threadList = new List<ForumThread>();
            threadList.Add(thread);
        }

        public void AddSection(Section section)
        {
            sectionList.Add(section);
        }

        public void AddSection2(Section section)
        {
            sectionList = new List<Section>();
            sectionList.Add(section);
        }

        public List<Post> GetPosts()
        {
            return postList;
        }

        public List<ForumThread> GetThreads()
        {
            return threadList;
        }

        public List<Section> GetSections()
        {
            return sectionList;
        }

    }
}
