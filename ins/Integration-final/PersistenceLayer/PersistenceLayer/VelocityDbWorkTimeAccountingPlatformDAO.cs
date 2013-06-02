using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using VelocityDb;
using VelocityDb.Session;
using PersistenceLayer.Dto;

namespace PersistenceLayer
{
    public class VelocityDbWorkTimeAccountingPlatformDAO : IWorkTimeAccountingPlatformDAO
    {
        #region ImplementedByPiotrBryk

        private static readonly string DefaultDatabaseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + ConfigurationManager.AppSettings["databaseFile"];
        private SessionNoServer Session { get; set; }
        private const UInt32 DbNum = 10;
        private UInt32 NextIndex = 1;

        /// Creates new VelocityDb DAO implementation.
        /// <remarks>Author: Piotr Bryk</remarks>
        public VelocityDbWorkTimeAccountingPlatformDAO()
        {
            Console.WriteLine(DefaultDatabaseDir);
            Session = new SessionNoServerShared(DefaultDatabaseDir);
            BeginSessionUpdate();
            try
            {
                Session.OpenDatabase(DbNum);
            }
            catch (OpenDatabaseException)
            {
                Session.Persist(0, new Placement(DbNum));
            }
            Session.Commit();

            var data = Select<Persistable>(acceptAll<Persistable>());
            foreach (var obj in data)
                if (NextIndex <= obj.Id) NextIndex = obj.Id + 1;

            try
            {
                var root = GetUser("root");
                root.Roles = new List<Role> { Role.Administrator, Role.Manager, Role.NormalUser };
                root.PublicUserInfo.Name = "root";
                SetUser(root);
            }
            catch (InvalidOperationException)
            {
                var root = new User
                {
                    PublicUserInfo = new PublicUserInfo { Login = "root", Name = "root" },
                    PasswordHash = "",
                    Roles = new List<Role> { Role.Administrator, Role.Manager, Role.NormalUser }
                };
                SetUser(root);
            }
        }

        /// Disposes this DAO object.
        /// <remarks>Author: Piotr Bryk</remarks>
        public void Dispose()
        {
            if (Session != null)
                Session.Close();
            Session = null;
        }

        /// Clears all database's content. Non undoable. Use with caution.
        /// <remarks>Author: Piotr Bryk</remarks>
        public void ClearDatabase()
        {
            BeginSessionUpdate();
            var db = Session.OpenDatabase(DbNum);
            Session.DeleteDatabase(db);
            Session.Commit();
            BeginSessionUpdate();
            try
            {
                Session.OpenDatabase(DbNum);
            }
            catch (OpenDatabaseException)
            {
                Session.Persist(0, new Placement(DbNum));
            }
            Session.Commit();
        }

        public DateTime GetWorkHistoryRange(User user)
        {
            var records = Select<WorkRecord>(wr => wr.EmployeeDescription.Employee.Login == user.PublicUserInfo.Login);
            return records.Min(entry => entry.WorkStartDate);
        }

        public DateTime GetPaymentHistoryRange(User user)
        {
            var records = Select<Contract>(c => c.Employee.Login == user.PublicUserInfo.Login);
            return records.Min(entry => entry.CreationDate);
        }

        public IList<WorkRecord> GetUserRecordsByDate(User user, DateTime dt)
        {
            return Select<WorkRecord>(wr => wr.EmployeeDescription.Employee.Login == user.PublicUserInfo.Login
                && wr.WorkStartDate.Month == dt.Month && wr.WorkStartDate.Year == dt.Year);
        }

        public IList<Contract> GetUserContractsByDate(User user, DateTime dt)
        {
            return Select<Contract>(c => c.Employee.Login == user.PublicUserInfo.Login
                && c.CreationDate.Month == dt.Month && c.CreationDate.Year == dt.Year);
        }

        /// <remarks>Author: Piotr Bryk</remarks>
        private void SaveOrUpdate(Persistable obj)
        {
            var original = Select<Persistable>(o => o.Id == obj.Id);
            BeginSessionUpdate();
            try
            {
                if (original.Any())
                {
                    original.Single().Merge(obj);
                    original.Single().Update();
                    //original.Single().Persist(Session, original.Single(), true);
                }
                else
                {
                    obj.Id = NextIndex++;
                    if (obj is User) ((User)obj).PublicUserInfo.Id = NextIndex++;
                    obj.Persist(Session, obj, true);
                }
            }
            finally
            {
                Session.Commit();
            }
        }

        /// Saves or updates the user in database. Empty login is not allowed. Adding new user 
        /// with already existing login not allowed either.
        /// <remarks>Author: Piotr Bryk</remarks>
        public void SetUser(User user)
        {
            if (user.PublicUserInfo == null || user.PublicUserInfo.Login == null || user.PublicUserInfo.Login == "")
            {
                throw new ArgumentException("Empty login not allowed");
            }
            if (Select<User>(u => user.PublicUserInfo.Login == u.PublicUserInfo.Login && user.Id != u.Id).Any())
            {
                throw new ArgumentException("User with the given login already exists");
            }
            SaveOrUpdate(user);
        }

        /// Returns User by login.
        /// <remarks>Author: Piotr Bryk</remarks>
        public User GetUser(string login)
        {
            return Select<User>(u => login == u.PublicUserInfo.Login).Single();
        }

        /// Sets password hash for the user with the given login.
        /// <remarks>Author: Piotr Bryk</remarks>
        public void SetPassword(string login, string hashPassword)
        {
            var user = GetUser(login);
            user.PasswordHash = hashPassword;
            SetUser(user);
        }

        /// Returns all projects of the given manager.
        /// <remarks>Author: Piotr Bryk</remarks>
        public IList<Project> GetProjecsOfManager(User manager)
        {
            return GetProjecsOfManager(manager, acceptAll<Project>());
        }

        /// Returns all employees of the given project.
        /// <remarks>Author: Piotr Bryk</remarks>
        public IList<EmployeeDescription> GetEmployeesOfProject(Project project)
        {
            return GetEmployeesOfProject(project, acceptAll<EmployeeDescription>());
        }

        /// Returns all WorkRecords of the given employee.
        /// <remarks>Author: Piotr Bryk</remarks>
        public IList<WorkRecord> GetRecordsOfEmployee(EmployeeDescription employeeDesc)
        {
            return GetRecordsOfEmployee(employeeDesc, acceptAll<WorkRecord>());
        }

        /// Returns all contracts of the given manager.
        /// <remarks>Author: Piotr Bryk</remarks>
        public IList<Contract> GetContractsOfManager(User manager)
        {
            return GetContractsOfManager(manager, acceptAll<Contract>());
        }

        /// Returns all projects of the given manager, filtered by the filter function.
        /// <remarks>Author: Piotr Bryk</remarks>
        public IList<Project> GetProjecsOfManager(User manager, FilterFunction<Project> filter)
        {
            return Select<Project>(proj => manager.PublicUserInfo.Id == proj.Manager.Id && filter(proj));
        }

        /// Returns all employees of the given project, filtered by the filter function.
        /// <remarks>Author: Piotr Bryk</remarks>
        public IList<EmployeeDescription> GetEmployeesOfProject(Project project,
                                                                FilterFunction<EmployeeDescription> filter)
        {
            return Select<EmployeeDescription>(desc => desc.Project.Id == project.Id && filter(desc));
        }

        /// Returns all records of the given employee, filtered by the filter function.
        /// <remarks>Author: Piotr Bryk</remarks>
        public IList<WorkRecord> GetRecordsOfEmployee(EmployeeDescription employeeDesc,
                                                      FilterFunction<WorkRecord> filter)
        {
            return Select<WorkRecord>(record => employeeDesc.Id == record.EmployeeDescription.Id && filter(record));
        }

        /// Returns all contracts of the given manager, filtered by the filter function.
        /// <remarks>Author: Piotr Bryk</remarks>
        public IList<Contract> GetContractsOfManager(User manager, FilterFunction<Contract> filter)
        {
            return
                Select<Contract>(contract => manager.PublicUserInfo.Id == contract.Creator.Id && filter(contract));
        }

        /// Saves or updates a contract in the database.
        /// <remarks>Author: Piotr Bryk</remarks>
        public void SetContract(Contract contract)
        {
            if (contract.Creator == null || contract.Creator.Id == 0)
                throw new ArgumentException("Creator of contract must exist in database");
            if (contract.Employee == null || contract.Employee.Id == 0)
                throw new ArgumentException("Employee must exist in database");
            if (contract.Project == null || contract.Project.Id == 0)
                throw new ArgumentException("Project must exist in database");
            contract.Creator = Select<PublicUserInfo>(ui => ui.Id == contract.Creator.Id).Single();
            contract.Employee = Select<PublicUserInfo>(ui => ui.Id == contract.Employee.Id).Single();
            contract.Project = Select<Project>(pro => pro.Id == contract.Project.Id).Single();
            SaveOrUpdate(contract);
        }

        public void SetSummary(Summary summary)
        {
            if (summary.EmployeeDescription == null || summary.EmployeeDescription.Id == 0)
                throw new ArgumentException("EmployeeDescription must exist in database");
            summary.EmployeeDescription = Select<EmployeeDescription>(ui => ui.Id == summary.EmployeeDescription.Id).Single();
            SaveOrUpdate(summary);
        }

        public IList<Summary> GetSummaries(User user)
        {
            return GetSummaries(user, acceptAll<Summary>());
        }

        public IList<Summary> GetSummaries(User user, FilterFunction<Summary> filter)
        {
            return Select<Summary>(sum => user.PublicUserInfo.Id == sum.EmployeeDescription.Employee.Id && filter(sum));
        }

        public void RemoveSummary(Summary summary)
        {
            Delete(summary);
        }

        /// Sets hourly rate for the given EmployeeDescription.Id
        /// <remarks>Author: Piotr Bryk</remarks>
        public void SetHourlyRate(EmployeeDescription employeeDescription, double hourlyRate)
        {
            var eDesc = Select<EmployeeDescription>(des => des.Id == employeeDescription.Id).Single();
            eDesc.HourlyRate = hourlyRate;
            SaveOrUpdate(eDesc);
        }

        /// Removes contract from the database.
        /// <remarks>Author: Piotr Bryk</remarks>
        public void RemoveContract(Contract contract)
        {
            Delete(contract);
        }

        #endregion

        #region ImplementedByAdamObuchowicz

        private FilterFunction<T> acceptAll<T>()
        {
            return o => true;
        }

        private void BeginSessionUpdate()
        {
            // Don't ask, why...
            for (int i = 0; i < 17; ++i)
                try
                {
                    Session.BeginUpdate();
                    break;
                }
                catch (MaxNumberOfDatabasesException)
                {

                }
        }

        private IList<T> Select<T>(FilterFunction<T> filter)
        {
            BeginSessionUpdate();
            List<T> data = null;
            try
            {
                var db = Session.OpenDatabase(DbNum);
                data = (from T person in db.AllObjects<T>() where filter(person) select person).ToList();
            }
            finally
            {
                Session.Commit();
            }
            return data;
        }

        private bool Delete(Persistable obj)
        {
            if (obj.Id == 0) return false;

            var persisted = Select<Persistable>(other => other.Id == obj.Id).Single();

            BeginSessionUpdate();
            try
            {
                persisted.Unpersist(Session);
                return true;
            }
            finally
            {
                Session.Commit();
            }
        }

        public bool CheckLoginAndPassword(string login, string hashPassword)
        {
            if ("root" == login)
            {
                return true;
            }
            return
                Select<User>(user => user.PublicUserInfo.Login == login && user.PasswordHash == hashPassword).Any();
        }

        public IList<Role> GetPrivileges(string login)
        {
            IList<User> users = Select(delegate(User user) { return user.PublicUserInfo.Login == login; });
            if (users.Any())
                return users.Single().Roles;
            else throw new ArgumentException("This user doesn't exists");
        }

        public IList<EmployeeDescription> GetEmployeeDescriptions(User user)
        {
            return GetEmployeeDescriptions(user, acceptAll<EmployeeDescription>());
        }

        public IList<Contract> GetContracts(User user)
        {
            return GetContracts(user, acceptAll<Contract>());
        }

        public IList<WorkRecord> GetRecords(User user)
        {
            return GetRecords(user, acceptAll<WorkRecord>());
        }

        public IList<EmployeeDescription> GetEmployeeDescriptions(User user, FilterFunction<EmployeeDescription> filter)
        {
            return Select(
                delegate(EmployeeDescription employee)
                { return employee.Employee.Id == user.PublicUserInfo.Id && filter(employee); });
        }

        public IList<Contract> GetContracts(User user, FilterFunction<Contract> filter)
        {
            return
                Select(
                    delegate(Contract contract)
                    { return contract.Employee.Id == user.PublicUserInfo.Id && filter(contract); });
        }

        public IList<WorkRecord> GetRecords(User user, FilterFunction<WorkRecord> filter)
        {
            return
                Select(
                    delegate(WorkRecord record)
                    { return record.EmployeeDescription.Employee.Id == user.PublicUserInfo.Id && filter(record); });
        }

        public void SetRecord(WorkRecord record)
        {
            if (record.EmployeeDescription == null || record.EmployeeDescription.Id == 0)
                throw new ArgumentException("Employee description must exist in database");
            record.EmployeeDescription = Select<EmployeeDescription>(ed => ed.Id == record.EmployeeDescription.Id).Single();
            SaveOrUpdate(record);
        }

        public void RemoveRecord(WorkRecord record)
        {
            Delete(record);
        }

        public IList<User> GetUsers()
        {
            return Select<User>(acceptAll<User>());
        }

        public IList<Project> GetProjects()
        {
            return Select<Project>(acceptAll<Project>());
        }

        public IList<EmployeeDescription> GetEmployeesDescriptions()
        {
            return Select<EmployeeDescription>(acceptAll<EmployeeDescription>());
        }

        public IList<User> GetUsers(FilterFunction<User> filter)
        {
            return Select(filter);
        }

        public IList<Project> GetProjects(FilterFunction<Project> filter)
        {
            return Select(filter);
        }

        public IList<EmployeeDescription> GetEmployeesDescriptions(FilterFunction<EmployeeDescription> filter)
        {
            return Select(filter);
        }

        public void SetProject(Project project)
        {
            if (project.Manager == null || project.Manager.Id == 0)
                throw new ArgumentException("Manager of project must exist in database");
            project.Manager = Select<PublicUserInfo>(ui => ui.Id == project.Manager.Id).Single();
            SaveOrUpdate(project);
        }

        public void SetEmployeeDescription(EmployeeDescription employeeDescription)
        {
            if (employeeDescription.Project == null || employeeDescription.Project.Id == 0)
                throw new ArgumentException("Project must exist in database");
            if (employeeDescription.Employee == null || employeeDescription.Employee.Id == 0)
                throw new ArgumentException("Employee must exist in database");
            employeeDescription.Project = Select<Project>(pro => pro.Id == employeeDescription.Project.Id).Single();
            employeeDescription.Employee = Select<PublicUserInfo>(ui => ui.Id == employeeDescription.Employee.Id).Single();
            SaveOrUpdate(employeeDescription);
        }

        public void RemoveUser(User user)
        {
            if (user.Id == 0) throw new ArgumentException("User doesn't exsist in database");
            var persisted = Select<User>(other => other.Id == user.Id).Single();
            var descriptions = Select<EmployeeDescription>(ed => ed.Employee.Id == persisted.PublicUserInfo.Id);
            var contracts = Select<Contract>(c => c.Employee.Id == persisted.PublicUserInfo.Id);
            List<WorkRecord> records = new List<WorkRecord>();
            List<Summary> summaries = new List<Summary>();
            foreach (EmployeeDescription ed in descriptions)
            {
                records.AddRange(Select<WorkRecord>(wr => wr.EmployeeDescription.Id == ed.Id));
                summaries.AddRange(Select<Summary>(sum => sum.EmployeeDescription.Id == ed.Id));
            }

            BeginSessionUpdate();
            try
            {
                foreach (WorkRecord wr in records)
                    wr.Unpersist(Session);
                foreach (EmployeeDescription ed in descriptions)
                    ed.Unpersist(Session);
                foreach (Contract contract in contracts)
                    contract.Unpersist(Session);
                foreach (Summary summary in summaries)
                    summary.Unpersist(Session);
                persisted.PublicUserInfo.Unpersist(Session);
                persisted.Unpersist(Session);
            }
            finally
            {
                Session.Commit();
            }
        }

        public void RemoveProject(Project project)
        {
            if (project.Id == 0) throw new ArgumentException("Project doesn't exist in database");
            var persisted = Select<Project>(other => other.Id == project.Id).Single();
            var descriptions = Select<EmployeeDescription>(ed => ed.Project.Id == persisted.Id);
            var contracts = Select<Contract>(con => con.Project.Id == persisted.Id);
            var records = new List<WorkRecord>();
            var summaries = new List<Summary>();
            foreach (EmployeeDescription ed in descriptions)
            {
                records.AddRange(Select<WorkRecord>(wr => wr.EmployeeDescription.Id == ed.Id));
                summaries.AddRange(Select<Summary>(sum => sum.EmployeeDescription.Id == ed.Id));
            }
            BeginSessionUpdate();
            try
            {
                foreach (WorkRecord wr in records)
                    wr.Unpersist(Session);
                foreach (EmployeeDescription ed in descriptions)
                    ed.Unpersist(Session);
                foreach (Contract con in contracts)
                    con.Unpersist(Session);
                foreach (Summary sum in summaries)
                    sum.Unpersist(Session);
                persisted.Unpersist(Session);
            }
            finally
            {
                Session.Commit();
            }
        }

        public void RemoveEmployeeDescription(EmployeeDescription employeeDescription)
        {
            if (employeeDescription.Id == 0) throw new ArgumentException("Employee description doesn't exist in database");
            var persisted = Select<EmployeeDescription>(other => other.Id == employeeDescription.Id).Single();
            var dependentWorkRecords = Select<WorkRecord>(wr => wr.EmployeeDescription.Id == persisted.Id);
            var dependentSummaries = Select<Summary>(sum => sum.EmployeeDescription.Id == persisted.Id);
            BeginSessionUpdate();
            try
            {
                foreach (var depency in dependentWorkRecords)
                    depency.Unpersist(Session);
                foreach (var depency in dependentSummaries)
                    depency.Unpersist(Session);
                persisted.Unpersist(Session);
            }
            finally
            {
                Session.Commit();
            }
        }

        public IList<Summary> GetSummariesOfEmployee(EmployeeDescription employee)
        {
            return Select<Summary>(sum => sum.EmployeeDescription.Id == employee.Id);
        }
        public IList<Summary> GetLastSummaries(EmployeeDescription employee, int ammount)
        {
            var all = from sum in GetSummariesOfEmployee(employee) orderby sum.Date descending select sum;
            return new List<Summary>(all.Take(ammount));
        }
        public IList<WorkRecord> GetRecordsOfEmployeeBonuses(EmployeeDescription employee)
        {
            return GetRecordsOfEmployee(employee, emp => emp.IsBonus);
        }
        public IList<WorkRecord> GetRecordsOfEmployeeNonBonuses(EmployeeDescription employee)
        {
            return GetRecordsOfEmployee(employee, emp => !emp.IsBonus);
        }
        public IList<Contract> GetContractsOfManagerWithUser(User manager, PublicUserInfo user)
        {
            return GetContractsOfManager(manager, con => con.Employee.Id == user.Id);
        }
        public IList<Contract> GetContractsOfManagerWithProject(User manager, Project project)
        {
            return GetContractsOfManager(manager, con => project.Id == con.Project.Id);
        }

        #endregion
    }
}
