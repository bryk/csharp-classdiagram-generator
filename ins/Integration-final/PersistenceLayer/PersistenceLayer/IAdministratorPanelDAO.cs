using System.Collections.Generic;
using PersistenceLayer.Dto;

namespace PersistenceLayer
{
    public interface IAdministratorPanelDAO
    {
        /// <summary>
        /// Returns all users from the database.
        /// </summary>
        IList<User> GetUsers();

        /// <summary>
        /// Returns all projects from the database.
        /// </summary>
        /// <returns></returns>
        IList<Project> GetProjects();
         
        /// <summary>
        /// Returns all employee descriptions from the database.
        /// </summary>
        IList<EmployeeDescription> GetEmployeesDescriptions();

        /// <summary>
        /// Returns all users, filtered by the filter function.
        /// </summary>
        IList<User> GetUsers(FilterFunction<User> filter);

        /// <summary>
        /// Returns all projects, filtered by the filter function.
        /// </summary>
        IList<Project> GetProjects(FilterFunction<Project> filter);
        
        /// <summary>
        /// Returns all employee descriptions, filtered by the filter function.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<EmployeeDescription> GetEmployeesDescriptions(FilterFunction<EmployeeDescription> filter);

        /// <summary>
        /// Saves or updates a user in the database. Empty or repeated login not allowed.
        /// </summary>
        void SetUser(User user);
        
        /// <summary>
        /// Saves or updates a project in the database.
        /// </summary>
        void SetProject(Project project);
        
        /// <summary>
        /// Saves or updates an employee description in the database.
        /// </summary>
        void SetEmployeeDescription(EmployeeDescription employeeDescription);

        /// <summary>
        /// Removes user from the database.
        /// </summary>
        void RemoveUser(User user);
        
        /// <summary>
        /// Removes project from the database.
        /// </summary>
        void RemoveProject(Project project);
        
        /// <summary>
        /// Removes employee description from the database.
        /// </summary>
        void RemoveEmployeeDescription(EmployeeDescription employeeDescription);
    }
}
