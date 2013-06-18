using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBModule
{
    public class Post
    {
        public string ThreadName { get; set; }
        public string SectionName { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime dateTime { get; set; }

        public Post(string threadName, string sectionName, string author, string content, DateTime dateTime)
        {
            this.ThreadName = threadName;
            this.SectionName = sectionName;
            this.Author = author;
            this.Content = content;
            this.dateTime = dateTime;
        }

        public override string ToString()
        {
            return base.ToString() + ": " + this.ThreadName + " " + this.SectionName + " " + this.Author + " " + this.Content + " " + this.dateTime;
        }
    }
}
