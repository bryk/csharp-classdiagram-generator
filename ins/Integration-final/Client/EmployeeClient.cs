using System;
using System.Collections.Generic;
using PersistenceLayer.Dto;

namespace Client
{
    public class EmployeeClient : UIClient
    {
        public override bool Login(string user, string passwd)
        {
            var roles = new[] {Role.NormalUser};
            return Login(user, PasswordHasher.ComputeHash(passwd), roles); //prot: WTF? hash po stronie klienta? // dobrze jest - Wy podajecie plaintext i tutaj się haszuje - do serwera jest przesyłana już postać zahaszowana
        }

        public override Role[] MultiLogin(string user, string passwd)
        {
            throw new NotImplementedException();
        }
    }
}
