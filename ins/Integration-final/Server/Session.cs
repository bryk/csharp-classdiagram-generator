using System.Collections.Generic;
using PersistenceLayer;
using PersistenceLayer.Dto;

namespace Server
{
    public class Session
    {
        public string SessionId { get; private set; }

        public string Login { get; private set; }

        public List<Role> Roles { get; private set; }

        public System.DateTime Time { get; set; }

        public Session(string sessionId, string login, List<Role> roles, System.DateTime time)
        {
            SessionId = sessionId;
            Login = login;
            Roles = roles;
            Time = time;
        }
    }
}