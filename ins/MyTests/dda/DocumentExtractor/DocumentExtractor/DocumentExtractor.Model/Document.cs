using System.Collections.Generic;

namespace DocumentExtractor.Model
{
    public class Document
    {
        public int Id { get; set; }
        public string SectionTitle { get; set; }
        public string TopicTitle { get; set; }
        public List<Post> Posts { get; set; }
        public DocumentStatus Status { get; set; }

        public Document()
        {
            Posts = new List<Post>();
            Status = DocumentStatus.New;
        }
    }
}
