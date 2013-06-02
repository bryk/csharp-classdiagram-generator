using System;
using PersistenceLayer.Dto;

namespace Client
{
    public class ManagerClient : UIClient //jczyzews - changed to public
    {
        public override bool Login(string user, string passwd)
        {
            var roles = new[] { Role.Manager };
            return Login(user, PasswordHasher.ComputeHash(passwd), roles);
        }

        public override Role[] MultiLogin(string user, string passwd)
        {
            throw new NotImplementedException();
        }
    }
}
