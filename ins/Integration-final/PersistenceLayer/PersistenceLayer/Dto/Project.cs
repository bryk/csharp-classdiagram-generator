using System;
using System.Runtime.Serialization;

namespace PersistenceLayer.Dto
{
    [DataContract]
    public class Project : Persistable
    {
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public PublicUserInfo Manager { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public Double Budget { get; set; }

        public override void Merge(Persistable another)
        {
            Description = ((Project)another).Description;
            Name = ((Project)another).Name;
            Budget = ((Project)another).Budget;
        }

        public override string ToString()
        {
            return "{Id:" + Id + ", Name:" + Name +
                   ", Manager:" + Manager + ", Budget:" + Budget + "}";
        }
    }
}
