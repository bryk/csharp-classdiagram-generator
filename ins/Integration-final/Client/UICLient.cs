using System;
using System.Collections.Generic;
using Client.CommunicationServiceReference;
using PersistenceLayer.Dto;

namespace Client
{
    public abstract class UIClient : CommunicationClient
    {
        public abstract bool Login(string user, string passwd);

        public new void SetPassword(string login, string passwdPlain)
        {
            base.SetPassword(login, PasswordHasher.ComputeHash(passwdPlain));
        }

        public new void ChangeMyPassword(string oldPasswordPlain, string newPasswdPlain)
        {
            base.ChangeMyPassword(PasswordHasher.ComputeHash(oldPasswordPlain), PasswordHasher.ComputeHash(newPasswdPlain));
        }

        public abstract Role[] MultiLogin(string user, string passwd);

    }

}
