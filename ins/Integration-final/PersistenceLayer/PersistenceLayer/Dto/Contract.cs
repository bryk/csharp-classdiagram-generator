using System;
using System.Runtime.Serialization;

namespace PersistenceLayer.Dto
{
    [DataContract]
    public class Contract : Persistable
    {
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public PublicUserInfo Creator { get; set; }
        [DataMember]
        public Project Project { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public PublicUserInfo Employee { get; set; }
        [DataMember]
        public Double Value { get; set; }
        [DataMember]
        public Boolean IsCounted { get; set; }

        public override void Merge(Persistable another)
        {
            CreationDate = ((Contract)another).CreationDate;
            Description = ((Contract)another).Description;
            Value = ((Contract)another).Value;
            IsCounted = ((Contract)another).IsCounted;
        }

        public override string ToString()
        {
            return "{Id: " + Id + "DateTime:" + CreationDate + ", Creator:" + Creator + ", Description:" + Description +
                ", Employee:" + Employee + ", Value:" + Value + ", IsCounted: " + IsCounted;
        }
    }
}
