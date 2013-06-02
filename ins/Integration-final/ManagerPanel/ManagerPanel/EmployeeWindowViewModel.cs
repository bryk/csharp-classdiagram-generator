using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceLayer.Dto;
using System.ComponentModel;
using Client;

namespace ManagerPanel
{
    class EmployeeWindowViewModel : INotifyPropertyChanged
    {
        private EmployeeDescription _currentEmployee;
        private UIClient db = new ManagerClient();

        public EmployeeDescription CurrentEmployee
        {
            get { return _currentEmployee; }
            set { _currentEmployee = value; }
        }

        public EmployeeWindowViewModel(EmployeeDescription e, UIClient dbs)
        {
            _currentEmployee = e;
            db = dbs;
            NotifyPropertyChanged("Hoursx");
    
        }

        public IEnumerable<WorkRecord> Hoursx
        {
            get
            { 

                IList<WorkRecord> wrlst = db.GetRecordsOfEmployee(CurrentEmployee);
                List<WorkRecord> toReturn = new List<WorkRecord>();
                foreach (WorkRecord wr in wrlst.Where(wr => (wr.IsBonus == false)))
                {
                    toReturn.Add(wr);
                }
                return toReturn;
            }
        }

        public IEnumerable<Summary> Summaries
        {
            get
            {
                IList<Summary> toReturn = db.GetSummariesOfEmployee(CurrentEmployee);
                return toReturn;
            }
        }

        public IEnumerable<WorkRecord> Bonuses
        {
            get { 

            IList<WorkRecord> wrlst = db.GetRecordsOfEmployee(CurrentEmployee);
            List<WorkRecord> toReturn = new List<WorkRecord>();
            foreach (WorkRecord wr in wrlst.Where(wr => (wr.IsBonus == true)))
            {
                toReturn.Add(wr);
            }
            return toReturn;
            }
        }


/*
        public IEnumerable<Contract> Contracts
        {
            get { return (new List<Contract>()).Add(new Contract()); }
         //   get { return db.GetContractOfManagerWithUser(CurrentEmployee.Employee); }
        }
*/
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
