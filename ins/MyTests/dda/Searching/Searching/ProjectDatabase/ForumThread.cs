using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSearching
{
    public class ForumThread
    {
        public ForumThread(string SectionName, string ThreadName)
        {
            this.SectionName = SectionName;
            this.ThreadName = ThreadName;
        }

        public string SectionName { get; set; }


        public string ThreadName { get; set; }
        
    }
}
