using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PersistenceLayer;
using PersistenceLayer.Dto;
using System.Windows.Input;
using PieControls;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Client;


namespace ManagerPanel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
       // private PersistenceFacade _persistenceFacade = new PersistenceFacade();
        public static ManagerClient db;

        public EmployeeDescription _selectedEmployee;
        public Project _selectedProject;
        private User _user;
        private ObservableCollection<PieSegment> pieCollection = new ObservableCollection<PieSegment>();

        public DateTime LastSummaryDate
        {
            get
            {
                if (_selectedEmployee == null) return DateTime.Now;
                return db.GetLastSummaries(_selectedEmployee, 1).Last().Date;
            }
        }
        
        public double UncountedHours
        {
           // get
            //{
               // if (_selectedEmployee == null) return 0.0;
               // return db.GetRecordsOfEmployeeUnchecked(_selectedEmployee).Sum(item => item.MinutesWorked) / 60;

            get
            {
                List<WorkRecord> toReturn = new List<WorkRecord>(db.GetRecordsOfEmployee(_selectedEmployee));
                toReturn.FindAll(x => x.WorkStartDate < LastSummaryDate);
                //_summary.MinutesWorked = (UInt32)toReturn.Sum(x => x.MinutesWorked);
                return toReturn.Sum(x=> x.MinutesWorked / 60.0);
            }
            
        }


        public MainWindowViewModel(String userlogin)
        {
            _user = db.GetUserInfo(userlogin); //sprawdzić, czy to działa, bo to metoda z api employee
            SelectedProject = db.GetProjecsOfManager(_user).First();
            

        }

        public ObservableCollection<PieSegment> PieCollection
        {
            get { pieCollection = new ObservableCollection<PieSegment>();
            pieCollection.Add(new PieSegment { Color = Colors.Green, Value = (SelectedProject.Budget - sumContracts(SelectedProject)), Name = "Remaining" });
            pieCollection.Add(new PieSegment { Color = Colors.Red, Value = (sumContracts(SelectedProject)), Name = "Spent" });
            return pieCollection;
            }
        }

        public ICommand SummarizeCommand
        {
            get { return new RelayCommand(Summarize, i => _selectedEmployee != null); }
        }



        public ICommand AddBonusCommand
        {
            get { return new RelayCommand(AddBonus, i => _selectedEmployee != null); }
        }

        public ICommand RegisterContractCommand
        {
            get { return new RelayCommand(RegisterContract, i => _selectedEmployee != null); }
        }

        public ICommand SetRateCommand
        {
            get { return new RelayCommand(SetRate, i => _selectedEmployee != null); }
        }



        public void AddBonus()
        {
            (new AddBonusWindow(_selectedEmployee, db)).ShowDialog();
        }

        public void Summarize()
        {
            (new SummaryWindow(db, _selectedEmployee)).ShowDialog();
        }

        public void RegisterContract()
        {
            (new RegisterContractWindow(_selectedEmployee.Employee, db)).ShowDialog();
        }

        public void SetRate()
        {
            (new SetRateWindow(_selectedEmployee, db)).ShowDialog();
            NotifyPropertyChanged("Employees");
            NotifyPropertyChanged("SelectedEmployee");
        }

        public EmployeeDescription SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; NotifyPropertyChanged("SelectedEmployee");
            NotifyPropertyChanged("UncountedHours");
                CommandManager.InvalidateRequerySuggested(); }
        }

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set { _selectedProject = value;
                    NotifyPropertyChanged("SelectedProject");
                    NotifyPropertyChanged("Employees");
                    NotifyPropertyChanged("PieCollection");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        public IEnumerable<Project> Projects
        {
            get { return db.GetProjecsOfManager(_user); }
        }


        public IEnumerable<EmployeeDescription> Employees
        {
            get {  return db.GetEmployeesOfProject(SelectedProject); }
        }

        public void OpenEmployeeWindow()
        {
            EmployeeWindow newWindow = new EmployeeWindow(SelectedEmployee, db);
            newWindow.Show();
        }

        public ICommand EmployeeCommand
        {
            get { return new RelayCommand(OpenEmployeeWindow, i => _selectedEmployee != null); }
        }

        private Double sumContracts(Project pr)
        {

            Double toReturn = 0;
            foreach (Contract c in db.GetContractOfManagerWithProject(SelectedProject))
                toReturn+=c.Value;
            return toReturn;
            
        }

    }


}
