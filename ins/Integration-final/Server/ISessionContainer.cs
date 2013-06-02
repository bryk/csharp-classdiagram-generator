using System.Collections.Generic;
using PersistenceLayer;
using PersistenceLayer.Dto;

namespace Server
{
    // needed for mocking in unit tests
    public interface ISessionContainer
    {
        void RemoveByLogin(string login);
        Session FindSession(string sessionId);
        string UserLoginBySessionId(string sessionId);
        bool HasActiveSession(string sessionId);
        bool IsAdmin(string sessionId);
        bool IsNormalUser(string sessionId);
        bool IsNormalUserWithSpecifiedLogin(string sessionId, string login);
        bool IsManagerWithSpecifiedLogin(string sessionId, string login);
        bool IsManager(string sessionId);
        void CheckSessionTimeout(Session session);
        bool TryLogout(string sessionId);
        bool TryLogin(string sessionId, string login, string passwd, List<Role> roles);
        List<Role> TryMultiLogin(string sessionId, string login, string passwd, List<Role> roles);
        void RemoveBySessionId(string sessionId);
        void EnsureHasActiveSession(string sessionId);
    }
}