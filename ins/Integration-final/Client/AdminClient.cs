using System;
using System.Collections.Generic;
using PersistenceLayer.Dto;

namespace Client
{
    public class AdminClient: UIClient
    {
        public override bool Login(string user, string passwd)
        {
            var roles = new[] { Role.Administrator };
            return Login(user, PasswordHasher.ComputeHash(passwd), roles);
        }

        public override Role[] MultiLogin(string user, string passwd)
        {
            throw new NotImplementedException();
        }
    }
}
