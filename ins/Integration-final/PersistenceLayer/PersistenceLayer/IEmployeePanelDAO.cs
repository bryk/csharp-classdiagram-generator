using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceLayer.Dto;

namespace PersistenceLayer
{
    public interface IEmployeePanelDAO
    {
        User GetUser(String login);

        void SetPassword(String login, String hashPassword);

        /// <summary>
        /// Returns all employee descriptions for the given user.
        /// </summary>
        IList<EmployeeDescription> GetEmployeeDescriptions(User user);
        
        /// <summary>
        /// Returns all contracts for the given user.
        /// </summary>
        IList<Contract> GetContracts(User user);
        
        /// <summary>
        /// Returns all work records for the given user.
        /// </summary>
        IList<WorkRecord> GetRecords(User user);

        /// <summary>
        /// Returns all employee descriptions for the given user, filtered by the filter function.
        /// </summary>
        IList<EmployeeDescription> GetEmployeeDescriptions(User user, FilterFunction<EmployeeDescription> filter);

        /// <summary>
        /// Returns all contracts for the given user, filtered by the filter function.
        /// </summary>
        IList<Contract> GetContracts(User user, FilterFunction<Contract> filter);

        /// <summary>
        /// Returns all work records for the given user, filtered by the filter function.
        /// </summary>
        IList<WorkRecord> GetRecords(User user, FilterFunction<WorkRecord> filter);

        /// <summary>
        /// Returns all Summary objects for the given user.
        /// </summary>
        IList<Summary> GetSummaries(User user);

        /// <summary>
        /// Returns all Summary objects for the given user, filtered by the filter function.
        /// </summary>
        IList<Summary> GetSummaries(User user, FilterFunction<Summary> filter);

        /// <summary>
        /// Saves or updates the given Summary object.
        /// </summary>
        void SetSummary(Summary summary);

        /// <summary>
        /// Saves or updates work record.
        /// </summary>
        void SetRecord(WorkRecord record);

        /// <summary>
        /// Removes given work record from the database.
        /// </summary>
        void RemoveRecord(WorkRecord record);

        /// <summary>
        /// Removes given summary from the database.
        /// </summary>
        void RemoveSummary(Summary summary);

        DateTime GetPaymentHistoryRange(User user);
        DateTime GetWorkHistoryRange(User user);
        IList<WorkRecord> GetUserRecordsByDate(User user, DateTime dt);
        IList<Contract> GetUserContractsByDate(User user, DateTime dt);
    }
}
