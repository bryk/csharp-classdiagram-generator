using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceLayer.Dto;

namespace Client
{
    public class MultiClient : UIClient
    {
        public override bool Login(string user, string passwd)
        {
            throw new NotImplementedException();
        }

        public override Role[] MultiLogin(string user, string passwd)
        {
            var roles = new[] { Role.Administrator, Role.Manager, Role.NormalUser };
            return MultiLogin(user, PasswordHasher.ComputeHash(passwd), roles);
        }
    }
}
