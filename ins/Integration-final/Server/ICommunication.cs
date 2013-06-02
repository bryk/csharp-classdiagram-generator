using System;
using System.Collections.Generic;
using System.ServiceModel;
using PersistenceLayer.Dto;
using Server.Shared.Exceptions;
using PersistenceLayer;

namespace Server
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ICommunication
    {
        // **********************************************************
        // ********************* Common *****************************
        // **********************************************************

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        bool Login(string user, string passwd, List<Role> roles);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        List<Role> MultiLogin(string user, string passwd, List<Role> roles);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        bool Logout();

        /*[OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        User GetLoggedUserData();*/
         
        [OperationContract]
        [FaultContract(typeof(NoActiveSession))]
        [FaultContract(typeof(IncorrectOldPassword))]
        void ChangeMyPassword(String oldPasswordHash, String newPasswordHash);

        // **********************************************************
        // ********************* ADMIN ******************************
        // **********************************************************

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<User> GetUsers();

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<Project> GetProjects();

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<EmployeeDescription> GetEmployeesDescriptions();

        /*[OperationContract]
        IList<User> GetUsersF(FilterFunction<User> filter);
        [OperationContract]
        IList<Project> GetProjectsF(FilterFunction<Project> filter);
        [OperationContract]
        IList<EmployeeDescription> GetEmployeesDescriptionsF(FilterFunction<EmployeeDescription> filter);
        */

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void SetUser(User user);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void SetPassword(string login, string newPasswordHash);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void SetProject(Project project);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void SetEmployeeDescription(EmployeeDescription employeeDescription);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void RemoveUser(User user);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void RemoveProject(Project project);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void RemoveEmployeeDescription(EmployeeDescription employeeDescription);

        // **********************************************************
        // ********************* Employee ***************************
        // **********************************************************

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        User GetUserInfo(String login);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<EmployeeDescription> GetEmployeeDescriptions(User user);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<Contract> GetContracts(User user);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<WorkRecord> GetRecords(User user);

        /*[OperationContract]
        IList<EmployeeDescription> GetEmployeeDescriptionsF(User user, FilterFunction<EmployeeDescription> filter);
        [OperationContract]
        IList<Contract> GetContractsF(User user, FilterFunction<Contract> filter);
        [OperationContract]
        IList<WorkRecord> GetRecordsF(User user, FilterFunction<WorkRecord> filter);
        */
        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void SetRecord(WorkRecord record);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void SetSummary(Summary summary);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void RemoveRecord(WorkRecord record);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        DateTime GetPaymentHistoryRange(User user);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        DateTime GetWorkHistoryRange(User user);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<WorkRecord> GetRecordsByDate(User user, DateTime dt);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<Contract> GetContractsByDate(User user, DateTime dt);
        // **********************************************************
        // ********************* Manager ****************************
        // **********************************************************

        // User GetUser(String login);
        // void SetPassword(String login, String hashPassword);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IEnumerable<Project> GetProjecsOfManager(User manager);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IEnumerable<EmployeeDescription> GetEmployeesOfProject(Project project);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IEnumerable<WorkRecord> GetRecordsOfEmployee(EmployeeDescription employeeDesc);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IEnumerable<Contract> GetContractsOfManager(User manager);

       /* [OperationContract]
        IEnumerable<Project> GetProjecsOfManagerF(User manager, FilterFunction<Project> filter);
        [OperationContract]
        IEnumerable<EmployeeDescription> GetEmployeesOfProjectF(Project project, FilterFunction<EmployeeDescription> filter);
        [OperationContract]
        IEnumerable<WorkRecord> GetRecordsOfEmployeeF(EmployeeDescription employeeDesc, FilterFunction<WorkRecord> filter);
        [OperationContract]
        IEnumerable<Contract> GetContractsOfManagerF(User manager, FilterFunction<Contract> filter);
        */

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void SetContract(Contract contract);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void SetHourlyRate(EmployeeDescription employeeDescription, double hourlyRate);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        void RemoveContract(Contract contract);
        
        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<Summary> GetSummariesOfEmployee(EmployeeDescription employee);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<Summary> GetLastSummaries(EmployeeDescription employee, int ammount);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<WorkRecord> GetRecordsOfEmployeeBonuses(EmployeeDescription employee);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<WorkRecord> GetRecordsOfEmployeeNonBonuses(EmployeeDescription employee);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<Contract> GetContractOfManagerWithUser(PublicUserInfo manager);

        [OperationContract]
        [FaultContract(typeof(PermissionDeniedForUser))]
        [FaultContract(typeof(NoActiveSession))]
        IList<Contract> GetContractOfManagerWithProject(Project project);
    }
}