using System;
using System.Runtime.Serialization;

namespace PersistenceLayer.Dto
{
    [DataContract]
    public class PublicUserInfo : Persistable
    {
        [DataMember]
        public String Login { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Surname { get; set; }

        public override void Merge(Persistable another)
        {
            Login = ((PublicUserInfo)another).Login;
            Name = ((PublicUserInfo)another).Name;
            Surname = ((PublicUserInfo)another).Surname;
        }

        public override string ToString()
        {
            return "{Login: " + Login + ", Name:" + Name + ", Surname:" + Surname + "}";
        }
    }
}
