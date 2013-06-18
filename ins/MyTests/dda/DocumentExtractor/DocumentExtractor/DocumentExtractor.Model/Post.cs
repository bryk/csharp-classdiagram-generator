using System;

namespace DocumentExtractor.Model
{
    public class Post
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string SectionTitle { get; set; }
        public string TopicTitle { get; set; }
        public PostStatus Status { get; set; }
    }
}
