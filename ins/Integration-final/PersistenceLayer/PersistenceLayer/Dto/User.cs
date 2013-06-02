using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PersistenceLayer.Dto
{
    [DataContract]
    public class User : Persistable
    {
        [DataMember]
        public String PasswordHash { get; set; }
        [DataMember]
        public PublicUserInfo PublicUserInfo { get; set; }
        [DataMember]
        public IList<Role> Roles { get; set; }

        public override void Merge(Persistable another)
        {
            PasswordHash = ((User)another).PasswordHash;
            Roles = ((User)another).Roles;
        }

        public override string ToString()
        {
            return "{PasswordHash:" + PasswordHash + ", PublicUserInfo:" + PublicUserInfo +
                   ", Roles:" + Roles + "}";
        }
    }
}