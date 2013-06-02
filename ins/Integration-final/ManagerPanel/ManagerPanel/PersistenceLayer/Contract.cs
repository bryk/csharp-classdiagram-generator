using System;

namespace PersistenceLayer
{
    public class Contract
    {
        public DateTime CreationDate { get; set; }
        public String Description { get; set; }
        public PublicUserInfo Employee { get; set; }
        public Project Project { get; set; }
        public Double Value { get; set; }
    }
}
