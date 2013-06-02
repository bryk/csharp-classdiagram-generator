using System;
using System.Collections.Generic;

namespace PersistenceLayer
{
    [Serializable]
    public class User
    {
        public String PasswordHash { get; set; }
        public PublicUserInfo PublicUserInfo { get; set; }
        public IList<Role> Roles { get; set; }

        public override string ToString()
        {
            return "{PasswordHash:" + PasswordHash + ", PublicUserInfo:" + PublicUserInfo +
                   ", Roles:" + Roles + "}";
        }
    }
}