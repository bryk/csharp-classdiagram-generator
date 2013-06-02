using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceLayer.Dto;

namespace PersistenceLayer
{
    public interface IManagerPanelDAO
    {
        /// <summary>
        /// Gets user from the database.
        /// </summary>
        User GetUser(String login);
        
        /// <summary>
        /// Sets password hash for the user with the given login.
        /// </summary>
        void SetPassword(String login, String hashPassword);

        /// <summary>
        /// Returns all projects of the given manager.
        /// </summary>
        IList<Project> GetProjecsOfManager(User manager);
        
        /// <summary>
        /// Returns all employees of the given project
        /// </summary>
        IList<EmployeeDescription> GetEmployeesOfProject(Project project);
        
        /// <summary>
        /// Returns all work records of the given employee.
        /// </summary>
        IList<WorkRecord> GetRecordsOfEmployee(EmployeeDescription employeeDesc);
        
        /// <summary>
        /// Returns all contracts of the given manager.
        /// </summary>
        IList<Contract> GetContractsOfManager(User manager);


        /// Returns all projects of the given manager, filtered by the filter function.
        /// </summary>
        IList<Project> GetProjecsOfManager(User manager, FilterFunction<Project> filter);

        /// <summary>
        /// Returns all employees of the given project, filtered by the filter function.
        /// </summary>
        IList<EmployeeDescription> GetEmployeesOfProject(Project project, FilterFunction<EmployeeDescription> filter);

        /// <summary>
        /// Returns all work records of the given employee, filtered by the filter function.
        /// </summary>
        IList<WorkRecord> GetRecordsOfEmployee(EmployeeDescription employeeDesc, FilterFunction<WorkRecord> filter);

        /// <summary>
        /// Returns all contracts of the given manager, filtered by the filter function.
        /// </summary>
        IList<Contract> GetContractsOfManager(User manager, FilterFunction<Contract> filter);

        /// <summary>
        /// Saves or updates the given contract.
        /// </summary>
        void SetContract(Contract contract);
        
        /// <summary>
        /// Sets hourly rate for the employee with the given EmployeeDescription.Id
        /// </summary>
        void SetHourlyRate(EmployeeDescription employeeDescription, double hourlyRate);

        /// <summary>
        /// Removec the given contract from the database.
        /// </summary>
        void RemoveContract(Contract contract);

        IList<Summary> GetSummariesOfEmployee(EmployeeDescription employee);
        IList<Summary> GetLastSummaries(EmployeeDescription employee, int ammount);
        IList<WorkRecord> GetRecordsOfEmployeeBonuses(EmployeeDescription employee);
        IList<WorkRecord> GetRecordsOfEmployeeNonBonuses(EmployeeDescription employee);
        IList<Contract> GetContractsOfManagerWithUser(User manager, PublicUserInfo User);
        IList<Contract> GetContractsOfManagerWithProject(User manager, Project project);

    }
}
