using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using PersistenceLayer;
using PersistenceLayer.Dto;
using Server.Shared.Exceptions;

namespace Server
{
    public class SessionContainer : ISessionContainer
    {
        private int _timeout;
        public SessionContainer()
        {
            _timeout = Int32.Parse(ConfigurationManager.AppSettings["autologgoutTimeoutSeconds"]);
        }
        
        public SessionContainer(IWorkTimeAccountingPlatformDAO db) : this()
        {
            _db = db;
        }

        private readonly IWorkTimeAccountingPlatformDAO _db = new VelocityDbWorkTimeAccountingPlatformDAO();
        private readonly List<Session> _sessions = new List<Session>();

        public bool TryLogin(string sessionId, string login, string passwd, List<Role> roles)
        {
            if (roles == null)
                return false;
            if (_db.CheckLoginAndPassword(login, passwd))
            {
                User user = ((IManagerPanelDAO) _db).GetUser(login);
                if (roles.Except(user.Roles).Any())
                    return false;

                _sessions.Add(new Session(sessionId, login, roles, DateTime.Now));
                return true;
            }
            return false;
        }

        public List<Role> TryMultiLogin(string sessionId, string login, string passwd, List<Role> roles)
        {
            var newRoles = new List<Role>();
            if (_db.CheckLoginAndPassword(login, passwd))
            {
                User user = ((IManagerPanelDAO)_db).GetUser(login);

                newRoles.AddRange(roles.Where(role => user.Roles.Contains(role)));

                if(newRoles.Count > 0)
                    _sessions.Add(new Session(sessionId, login, newRoles, DateTime.Now));
               
            }
            return newRoles;
        }

        public bool TryLogout(string sessionId)
        {
            if (!HasActiveSession(sessionId))
            {
                var noActive = new NoActiveSession
                {
                    Message = string.Format("You do not have active session")
                };
                throw new FaultException<NoActiveSession>(noActive);
            }
            return _sessions.RemoveAll(s => s.SessionId == sessionId) > 0;
        }

        public void RemoveBySessionId(string sessionId)
        {
            _sessions.RemoveAll(s => s.SessionId.Equals(sessionId));
        }

        public void RemoveByLogin(string login)
        {
            _sessions.RemoveAll(s => s.Login.Equals(login));
        }

        public Session FindSession(string sessionId)
        {
            return _sessions.Find(s => s.SessionId.Equals(sessionId));
        }

        public string UserLoginBySessionId(string sessionId)
        {
            Session soleSesssion = FindSession(sessionId);
            return (soleSesssion == null) ? null : soleSesssion.Login;
        }

        public bool HasActiveSession(string sessionId)
        {
            return FindSession(sessionId) != null;
        }

        public void EnsureHasActiveSession(string sessionId)
        {
            if (!HasActiveSession(sessionId))
            {
                var noActive = new NoActiveSession
                {
                    Message = string.Format("You do not have active session")
                };
                throw new FaultException<NoActiveSession>(noActive);
            }
        }

        public bool IsAdmin(string sessionId)
        {
            EnsureHasActiveSession(sessionId);
            Session session = FindSession(sessionId);
            CheckSessionTimeout(session);
            return session.Roles.Contains(Role.Administrator);
        }

        public bool IsNormalUser(string sessionId)
        {
            EnsureHasActiveSession(sessionId);
            Session session = FindSession(sessionId);
            CheckSessionTimeout(session);
            return session.Roles.Contains(Role.NormalUser);
        }

        public bool IsNormalUserWithSpecifiedLogin(string sessionId, string login)
        {
            EnsureHasActiveSession(sessionId);
            Session session = FindSession(sessionId);
            CheckSessionTimeout(session);
            return (session.Roles.Contains(Role.NormalUser) && session.Login.Equals(login));
        }

        public bool IsManagerWithSpecifiedLogin(string sessionId, string login)
        {
            EnsureHasActiveSession(sessionId);
            Session session = FindSession(sessionId);
            CheckSessionTimeout(session);
            return (session.Roles.Contains(Role.Manager) && session.Login.Equals(login));
        }

        public bool IsManager(string sessionId)
        {
            EnsureHasActiveSession(sessionId);
            Session session = FindSession(sessionId);
            CheckSessionTimeout(session);
            return (session.Roles.Contains(Role.Manager));
        }

        public void CheckSessionTimeout(Session session)
        {
            TimeSpan ts = DateTime.Now - session.Time;
            if (ts.TotalSeconds > _timeout)
            {
                _sessions.RemoveAll(s => s.SessionId == session.SessionId);
                
                var noActive = new NoActiveSession
                {
                    Message = string.Format("Your session has expired.")
                };
                throw new FaultException<NoActiveSession>(noActive);
                
            } 
            
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                
            session.Time = dt;
        }
    }
}
