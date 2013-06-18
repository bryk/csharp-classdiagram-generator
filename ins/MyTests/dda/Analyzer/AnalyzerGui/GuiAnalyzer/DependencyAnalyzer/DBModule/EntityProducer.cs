using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuiAnalyzer.DependencyAnalyzer.DBModule
{
    class EntityProducer
    {
        public string name;
        public List<Post> postList;
        List<ForumThread> threadList;
        List<Section> sectionList;

        public EntityProducer(string name)
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
