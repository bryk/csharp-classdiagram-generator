using System;
using System.Collections.Generic;
using System.Linq;
using PersistenceLayer.Dto;

using System.Windows.Input;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows;
using Client;

namespace ManagerPanel
{
    class SummaryWindowViewModel : INotifyPropertyChanged
    {

        UIClient _db;
        EmployeeDescription _employee;
        IEnumerable<WorkRecord> _selectedWorkRecords;
        IList<WorkRecord> _workRecords;
        DateTime _lastSummaryDate;
        private Summary _summary;

        public event EventHandler RequestClose = (s, e) => { };
        public event PropertyChangedEventHandler PropertyChanged;

        public SummaryWindowViewModel(UIClient db, EmployeeDescription employee)
        {
            _db = db;
            _summary = new Summary();
            _summary.Date = DateTime.Today.AddDays(-DateTime.Today.Day);
            NotifyPropertyChanged("SummaryDate");
            _employee = employee;
            _summary.EmployeeDescription = employee;
            _workRecords = db.GetRecordsOfEmployee(employee);
            _selectedWorkRecords = new List<WorkRecord>();
            SummaryDate = DateTime.Now;
            var lastSum = db.GetLastSummaries(employee, 1);
            if (lastSum != null)
                _lastSummaryDate = lastSum.Last().Date;
        }

        public DateTime SummaryDate{

            get{
                return _summary.Date;
            }
            set{
                _summary.Date = value.AddHours(23).AddMinutes(59);
                NotifyPropertyChanged("SummaryDate");
                NotifyPropertyChanged("WorkRecords");
                NotifyPropertyChanged("Value");
                NotifyPropertyChanged("HoursChecked");
            }
        }

        public IEnumerable<WorkRecord> WorkRecords
        {
            get
            {
                return _workRecords.Where(i=> i.WorkStartDate.CompareTo(SummaryDate) < 0);
            }
        }

        public Double HoursChecked
        {
            get
            {
                List<WorkRecord> toReturn = new List<WorkRecord>(WorkRecords);
                toReturn.FindAll(x => x.WorkStartDate < SummaryDate);
                _summary.MinutesWorked = (UInt32)toReturn.Sum(x => x.MinutesWorked);
                return toReturn.Sum(x=> x.MinutesWorked / 60.0);
            }
        }

        public Double Value
        {
            get
            {
                List<WorkRecord> toReturn = new List<WorkRecord>(WorkRecords);
                toReturn.FindAll(x => x.WorkStartDate < SummaryDate);
                Double value = toReturn.Sum(x => x.HourlyRate * x.MinutesWorked / 60);
                _summary.MoneyWorked = (UInt32)value;
                return value;
            }
        }




        public ICommand SummarizeCommand
        {
            get { return new RelayCommand(Summarize); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel); }
        }

        public void Summarize()
        {
            // communication layer folk fix :)
            _summary.EmployeeDescription = _employee;   // maybe redundant
            _db.SetSummary(_summary);

//            foreach (WorkRecord wr in WorkRecords)
//           {
//                wr.isCounted = true;
//                _db.SetWorkRecord(wr);
//            }
            RequestClose(this, EventArgs.Empty);
        }

        public void Cancel()
        {
            RequestClose(this, EventArgs.Empty);
        }

        public virtual void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


    }
}
