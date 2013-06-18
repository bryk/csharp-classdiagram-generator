using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBModule
{
    public class EntityLogin
    {
        string name;
        List<Post> postList;
        List<ForumThread> threadList;
        List<Section> sectionList;

        public EntityLogin(string name)
        {
            this.name = name;
            postList = new List<Post>();
            threadList = new List<ForumThread>();
            sectionList = new List<Section>();
        }

        public void addPost(Post post)
        {
            postList.Add(post);
        }

        public void addThread(ForumThread thread)
        {
            threadList.Add(thread);
        }

        public void addSection(Section section)
        {
            sectionList.Add(section);
        }
    }
}
