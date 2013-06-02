using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagerPanel.PersistenceLayer;

namespace PersistenceLayer
{
    public interface IManagerPanelDAO
    {
        User GetUserInfo(String login);
        void SetPassword(String login, String hashPassword);

        IList<Project> GetProjectsOfManager(User manager);
        IList<EmployeeDescription> GetEmployeesOfProject(Project project);
        IList<WorkRecord> GetRecordsOfEmployee(EmployeeDescription employeeDesc);
        IList<Contract> GetContractsOfManager(User manager);

        void SetContract(Contract contract);
        void SetHourlyRate(EmployeeDescription employee, double hourlyRate);
        void SetWorkRecord(WorkRecord workRecord);
        void RemoveContract(Contract contract);


        //added Recently (10.01.2013):
        void SetSummary(Summary summary, EmployeeDescription employee);
        void AddToBallance(PublicUserInfo employee, Double Ammount);

        IList<Summary> GetSummariesOfEmployee(EmployeeDescription employee);
        IList<Summary> GetLastSummaries(EmployeeDescription employee, int ammount);

        IList<WorkRecord> GetRecordsOfEmployeeBonuses(EmployeeDescription employee);
        IList<WorkRecord> GetRecordsOfEmployeeNonBonuses(EmployeeDescription employee);
        IList<WorkRecord> GetRecordsOfEmployeeUnchecked(EmployeeDescription employee);
        IList<WorkRecord> GetRecordsOfEmployeeChecked(EmployeeDescription employee);

        IList<Contract> GetContractOfManagerWithUser(PublicUserInfo employee);
        IList<Contract> GetContractOfManagerWithProject(Project project);
        

    }
}
