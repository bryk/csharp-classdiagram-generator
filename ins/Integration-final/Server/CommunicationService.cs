using System;
using System.Collections.Generic;
using System.ServiceModel;
using PersistenceLayer.Dto;
using Server.Shared.Exceptions;
using PersistenceLayer;
using Server.Utils;

namespace Server
{
    class CommunicationService : ICommunication
    {
        private readonly ISessionContainer _sessionContainer;
        private readonly IWorkTimeAccountingPlatformDAO _db;
        private readonly ISessionIdProvider _sessionIdProvider;

        public CommunicationService() : this(new SessionContainer(), new VelocityDbWorkTimeAccountingPlatformDAO(), new WcfSessionIdProvider()) {}

        public CommunicationService(ISessionContainer sessionContainer, IWorkTimeAccountingPlatformDAO db, ISessionIdProvider sessionIdProvider)
        {
            _sessionContainer = sessionContainer;
            _db = db;
            _sessionIdProvider = sessionIdProvider;
        }

        public bool Login(string user, string passwd, List<Role> roles)
        {
            _sessionContainer.RemoveBySessionId(_sessionIdProvider.GetSessionId());
            Logger.Instance.Log(roles.ToString());
            return _sessionContainer.TryLogin(_sessionIdProvider.GetSessionId(), user, passwd, roles);
        }

        public List<Role> MultiLogin(string user, string passwd, List<Role> roles)
        {
            _sessionContainer.RemoveBySessionId(_sessionIdProvider.GetSessionId());
            Logger.Instance.Log(roles.ToString());
            return _sessionContainer.TryMultiLogin(_sessionIdProvider.GetSessionId(), user, passwd, roles);
        }

        public bool Logout()
        {
            return _sessionContainer.TryLogout(_sessionIdProvider.GetSessionId());
        }

        /*public User GetLoggedUserData()
        {
            string sessionId = _sessionIdProvider.GetSessionId();
            if (_sessionContainer.HasActiveSession(sessionId))
                return ((IManagerPanelDAO) _db).GetUser(_sessionContainer.UserLoginBySessionId(sessionId));
            else
                return null;
        }*/

        

        // **********************************************************
        // ********************* ADMIN ******************************
        // **********************************************************

        // ********************* Helpers ****************************
        private void EnsureAdmin()
        {
            string sessionId = _sessionIdProvider.GetSessionId();
            if (!_sessionContainer.IsAdmin(sessionId))
            {
                var permissionDenied = new PermissionDeniedForUser
                {
                    Message = string.Format("You do not have admin privileges")
                };
                throw new FaultException<PermissionDeniedForUser>(permissionDenied);
            }
        }

        // ********************* Service methods ********************
        public IList<User> GetUsers()
        {
            EnsureAdmin();
            return _db.GetUsers();
        }

        public IList<Project> GetProjects()
        {
            EnsureAdmin();
            return _db.GetProjects();
        }

        public IList<EmployeeDescription> GetEmployeesDescriptions()
        {
            EnsureAdmin();
            return _db.GetEmployeesDescriptions();
        }

        public IList<User> GetUsersF(FilterFunction<User> filter)
        {
            EnsureAdmin();
            return _db.GetUsers(filter);
        }

        public IList<Project> GetProjectsF(FilterFunction<Project> filter)
        {
            EnsureAdmin();
            return _db.GetProjects(filter);
        }

        public IList<EmployeeDescription> GetEmployeesDescriptionsF(FilterFunction<EmployeeDescription> filter)
        {
            EnsureAdmin();
            return _db.GetEmployeesDescriptions(filter);
        }

        public void SetUser(User user)
        {
            EnsureAdmin();
            _db.SetUser(user);
        }

        public void SetProject(Project project)
        {
            EnsureAdmin();
            _db.SetProject(project);
        }

        public void SetEmployeeDescription(EmployeeDescription employeeDescription)
        {
            EnsureAdmin();
            _db.SetEmployeeDescription(employeeDescription);
        }

        public void RemoveUser(User user)
        {
            // !!! Probably we have to logout active sessions of this user
            // not necessarily
            EnsureAdmin();
            _sessionContainer.RemoveByLogin(user.PublicUserInfo.Login);
            _db.RemoveUser(user);
        }

        public void RemoveProject(Project project)
        {
            EnsureAdmin();
            _db.RemoveProject(project);
        }

        public void RemoveEmployeeDescription(EmployeeDescription employeeDescription)
        {
            EnsureAdmin();
            _db.RemoveEmployeeDescription(employeeDescription);
        }

        // **********************************************************
        // ********************* Employee ***************************
        // **********************************************************

        // ********************* Helpers ****************************
        private void EnsureEmployeeWithSpecifiedLogin(string login)
        {
            string sessionId = _sessionIdProvider.GetSessionId();
            if (!_sessionContainer.IsNormalUserWithSpecifiedLogin(sessionId, login))
            {
                var permissionDenied = new PermissionDeniedForUser
                {
                    Message = string.Format("You do not have employee privileges")
                };
                throw new FaultException<PermissionDeniedForUser>(permissionDenied);
            }
        }

        private void EnsureEmployee()
        {
            string sessionId = _sessionIdProvider.GetSessionId();
            if (!_sessionContainer.IsNormalUser(sessionId))
            {
                var permissionDenied = new PermissionDeniedForUser
                {
                    Message = string.Format("You do not have employee privileges")
                };
                throw new FaultException<PermissionDeniedForUser>(permissionDenied);
            }
        }

        // ********************* Service methods ********************
        public User GetUserInfo(String login)
        {
            string sessionId = _sessionIdProvider.GetSessionId();
            if (!_sessionContainer.IsNormalUserWithSpecifiedLogin(sessionId, login) && !_sessionContainer.IsManagerWithSpecifiedLogin(sessionId, login))
            {
                var permissionDenied = new PermissionDeniedForUser
                {
                    Message = string.Format("You do not have employee privilige")
                };
                throw new FaultException<PermissionDeniedForUser>(permissionDenied);
            }
            return ((IManagerPanelDAO) _db).GetUser(login);
           
        }

        public void ChangeMyPassword(string oldPasswordHash, string newPasswordHash)
        {
            string sessionId = _sessionIdProvider.GetSessionId();
            _sessionContainer.EnsureHasActiveSession(sessionId);
            User user = ((IManagerPanelDAO) _db).GetUser(_sessionContainer.UserLoginBySessionId(sessionId));
            if (!user.PasswordHash.Equals(oldPasswordHash))
                throw new FaultException<IncorrectOldPassword>(new IncorrectOldPassword
                    {
                    Message = string.Format("Incorrect old password")
                });
            user.PasswordHash = newPasswordHash;
            _db.SetUser(user);
        }

        public void SetPassword(string login, string newPasswordHash)
        {
            EnsureAdmin();
            User user = ((IManagerPanelDAO) _db).GetUser(login);
            if (user != null)
            {
                user.PasswordHash = newPasswordHash;
                _db.SetUser(user);
            }
        }

        public IList<EmployeeDescription> GetEmployeeDescriptions(User user)
        {
            EnsureEmployeeWithSpecifiedLogin(user.PublicUserInfo.Login);
            return _db.GetEmployeeDescriptions(user);
        }

        public IList<Contract> GetContracts(User user)
        {
            EnsureEmployeeWithSpecifiedLogin(user.PublicUserInfo.Login);
            return _db.GetContracts(user);
        }

        public IList<WorkRecord> GetRecords(User user)
        {

            EnsureEmployeeWithSpecifiedLogin(user.PublicUserInfo.Login);
            return _db.GetRecords(user);
        }

        public IList<EmployeeDescription> GetEmployeeDescriptionsF(User user, FilterFunction<EmployeeDescription> filter)
        {
            EnsureEmployeeWithSpecifiedLogin(user.PublicUserInfo.Login);
            return _db.GetEmployeeDescriptions(user, filter);
        }

        public IList<Contract> GetContractsF(User user, FilterFunction<Contract> filter)
        {
            EnsureEmployeeWithSpecifiedLogin(user.PublicUserInfo.Login);
            return _db.GetContracts(user, filter);
        }

        public IList<WorkRecord> GetRecordsF(User user, FilterFunction<WorkRecord> filter)
        {
            EnsureEmployeeWithSpecifiedLogin(user.PublicUserInfo.Login);
            return _db.GetRecords(user, filter);
        }

        public void SetRecord(WorkRecord record)
        {
            // TO ASK: shall we set a field of the record?
            // We need sucha method:
//>>>>>>>>>>// User UserOfWorkRecord(WorkRecord record)
            EnsureEmployeeWithSpecifiedLogin(record.EmployeeDescription.Employee.Login);
            _db.SetRecord(record);
        }

        public void SetSummary(Summary summary)
        {
            EnsureEmployeeWithSpecifiedLogin(summary.EmployeeDescription.Employee.Login);
            _db.SetSummary(summary);
        }

        public void RemoveRecord(WorkRecord record)
        {
            // TO ASK: shall we set a field of the record?
            EnsureEmployeeWithSpecifiedLogin(record.EmployeeDescription.Employee.Login);
            _db.RemoveRecord(record);
        }


        public DateTime GetPaymentHistoryRange(User user)
        {
            EnsureEmployeeWithSpecifiedLogin(user.PublicUserInfo.Login);
            return _db.GetPaymentHistoryRange(user);
        }

        public DateTime GetWorkHistoryRange(User user)
        {
            EnsureEmployeeWithSpecifiedLogin(user.PublicUserInfo.Login);
            return _db.GetWorkHistoryRange(user);
        }

        public IList<WorkRecord> GetRecordsByDate(User user, DateTime dt)
        {
            EnsureEmployeeWithSpecifiedLogin(user.PublicUserInfo.Login);
            return _db.GetUserRecordsByDate(user, dt);
        }

        public IList<Contract> GetContractsByDate(User user, DateTime dt)
        {
            EnsureEmployeeWithSpecifiedLogin(user.PublicUserInfo.Login);
            return _db.GetUserContractsByDate(user, dt);
        }

        // **********************************************************
        // ********************* Manager ****************************
        // **********************************************************

        // ********************* Helpers ****************************
        private void EnsureManagerWithSpecifiedLogin(string login)
        {
            string sessionId = _sessionIdProvider.GetSessionId();
            if (!_sessionContainer.IsManagerWithSpecifiedLogin(sessionId, login))
            {
                var permissionDenied = new PermissionDeniedForUser
                {
                    Message = string.Format("You do not have manager privileges")
                };
                throw new FaultException<PermissionDeniedForUser>(permissionDenied);
            }
        }

        private void EnsureManager()
        {
            string sessionId = _sessionIdProvider.GetSessionId();
            if (!_sessionContainer.IsManager(sessionId))
            {
                var permissionDenied = new PermissionDeniedForUser
                {
                    Message = string.Format("You do not have manager privileges")
                };
                throw new FaultException<PermissionDeniedForUser>(permissionDenied);
            }
        }

        // ********************* Service methods ********************

        public IEnumerable<Project> GetProjecsOfManager(User manager)
        {
            EnsureManagerWithSpecifiedLogin(manager.PublicUserInfo.Login);
            return _db.GetProjecsOfManager(manager);
        }

        public IEnumerable<EmployeeDescription> GetEmployeesOfProject(Project project)
        {
//>>>>>>>>>>// does the project always have Manager and his login?
            EnsureManagerWithSpecifiedLogin(project.Manager.Login);
            return _db.GetEmployeesOfProject(project);
        }

        public IEnumerable<WorkRecord> GetRecordsOfEmployee(EmployeeDescription employeeDesc)
        {
            // TO ASK: does a manager have access to records of all employees?
            // otherwise, we have to get all manager's projects, employees of these projects etc...
            EnsureManager();
            return _db.GetRecordsOfEmployee(employeeDesc);
        }

        public IEnumerable<Contract> GetContractsOfManager(User manager) 
        {
            EnsureManagerWithSpecifiedLogin(manager.PublicUserInfo.Login);
            return _db.GetContractsOfManager(manager);
        }

        public IEnumerable<Project> GetProjecsOfManagerF(User manager, FilterFunction<Project> filter)
        {
            EnsureManagerWithSpecifiedLogin(manager.PublicUserInfo.Login);
            return _db.GetProjecsOfManager(manager, filter);
        }

        public IEnumerable<EmployeeDescription> GetEmployeesOfProjectF(Project project, FilterFunction<EmployeeDescription> filter)
        {
            EnsureManagerWithSpecifiedLogin(project.Manager.Login);
            return _db.GetEmployeesOfProject(project, filter);
        }

        public IEnumerable<WorkRecord> GetRecordsOfEmployeeF(EmployeeDescription employeeDesc, FilterFunction<WorkRecord> filter)
        {
            EnsureManager();
            return _db.GetRecordsOfEmployee(employeeDesc, filter);
        }

        public IEnumerable<Contract> GetContractsOfManagerF(User manager, FilterFunction<Contract> filter)
        {
            EnsureManagerWithSpecifiedLogin(manager.PublicUserInfo.Login);
            return _db.GetContractsOfManager(manager, filter);
        }

        public void SetContract(Contract contract)
        {
//>>>>>>>>>>// AGAIN: WHAT IS ACCESS POLICY?
            EnsureManager();
            _db.SetContract(contract);
        }

        public void SetHourlyRate(EmployeeDescription employeeDescription, double hourlyRate)
        {
//>>>>>>>>>>// AGAIN: WHAT IS ACCESS POLICY?
// HERE: we could get a metohde:
// User ManagerForEmployeeDescription(UInt64 employeeDescription);
            EnsureManager();
            _db.SetHourlyRate(employeeDescription, hourlyRate);
        }

        public void RemoveContract(Contract contract)
        {
//>>>>>>>>>>// AGAIN: WHAT IS ACCESS POLICY?
            EnsureManager();
            _db.RemoveContract(contract);
        }

        public IList<Summary> GetSummariesOfEmployee(EmployeeDescription employee)
        {
            EnsureManager();
            return _db.GetSummariesOfEmployee(employee);
        }

        public IList<Summary> GetLastSummaries(EmployeeDescription employee, int ammount)
        {
            EnsureManager();
            return _db.GetLastSummaries(employee, ammount);
        }

        public IList<WorkRecord> GetRecordsOfEmployeeBonuses(EmployeeDescription employee)
        {
            EnsureManager();
            return _db.GetRecordsOfEmployeeBonuses(employee);
        }

        public IList<WorkRecord> GetRecordsOfEmployeeNonBonuses(EmployeeDescription employee)
        {
            EnsureManager();
            return _db.GetRecordsOfEmployeeNonBonuses(employee);
        }

        public IList<Contract> GetContractOfManagerWithUser(PublicUserInfo employee)
        {
            EnsureManager();
            return ((IManagerPanelDAO)_db).GetContractsOfManager(((IManagerPanelDAO) _db).GetUser(employee.Login));
        }

        public IList<Contract> GetContractOfManagerWithProject(Project project)
        {
            EnsureManager();
            return
                _db.GetContractsOfManagerWithProject(((IManagerPanelDAO)_db).GetUser(
                    _sessionContainer.FindSession(_sessionIdProvider.GetSessionId()).Login), project);
        }
    }
}
