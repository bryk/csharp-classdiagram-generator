using System;
using System.Collections.Generic;
using PersistenceLayer.Dto;

namespace PersistenceLayer
{
    public delegate bool FilterFunction<T>(T data);

    public interface IWorkTimeAccountingPlatformDAO : IEmployeePanelDAO, IManagerPanelDAO, IAdministratorPanelDAO, IDisposable
    {
        /// <summary>
        /// Checks if the given pair of login and password hash are correct.
        /// </summary>
        /// <returns>True if yes, false otherwise.</returns>
        bool CheckLoginAndPassword(String login, String hashPassword);
        
        /// <summary>
        /// Returns all roles of the user with the given login.
        /// </summary>
        IList<Role> GetPrivileges(String login);

        /// <summary>
        /// Removes everything from the database. Non undoable. Use with caution.
        /// </summary>
        void ClearDatabase();
    }
}
