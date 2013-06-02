using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Client;
using PersistenceLayer.Dto;
using System.Linq;

namespace EmployeePanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Project[] _projects;
        private List<WorkRecord> _workRecords { get; set; }
        private List<Contract> _contracts { get; set; }
        private List<DateTime> _dates { get; set; }
        private EmployeeDescription[] _employeeDescriptions;
        private UIClient _client { get; set; }
        private User _currentUser { get; set; }
        private String _currentUserLogin;


        public MainWindow(UIClient EmployeeClient, String CurrentUserLogin)
        {
            
            
            _client = EmployeeClient;
            _currentUserLogin = CurrentUserLogin;
            _currentUser = _client.GetUserInfo(CurrentUserLogin);

            _workRecords = new List<WorkRecord>();
            _workRecords.AddRange(_client.GetRecords(_currentUser));
            

            /*
             * Get date range for WorkRecords
             */
            _dates = new List<DateTime>();
            DateTime startDate = new DateTime(2012,1,1); //change to _client.GetWorkHistoryRange(_currentUser) when implemented
            for (int i = 0; i < 13; i++)
            {
                _dates.Add(startDate.AddMonths(i));
            }


            /*
             * Get current users projects list
             */
            IList<Project> _tmpProjects = new List<Project>();
            _employeeDescriptions = _client.GetEmployeeDescriptions(_currentUser);
            foreach (var ed in _employeeDescriptions)
            {
                _tmpProjects.Add(ed.Project);
            }


            DataContext = this;
            InitializeComponent();

            _contracts = new List<Contract>();
            _contracts.AddRange(_client.GetContracts(_currentUser));

            monthComboBox.ItemsSource = _dates;
            monthComboBox.DisplayMemberPath = "Date";
            monthComboBox.ItemStringFormat = "MM yyyy";
            monthComboBox.SelectedIndex = 0;

            monthContractsComboBox.ItemsSource = _dates;
            monthContractsComboBox.DisplayMemberPath = "Date";
            monthContractsComboBox.ItemStringFormat = "MM yyyy";
            monthContractsComboBox.SelectedIndex = 0;

            _projects = _tmpProjects.ToArray();
            projectInputComboBox.ItemsSource = _projects;
            projectInputComboBox.DisplayMemberPath = "Name";
            projectInputComboBox.SelectedIndex = 0;

            dateInputDatePicker.SelectedDate = DateTime.Now;
            hoursWorkedTextBox.Text = "hh:mm";

            balance.Text = countBalance(_workRecords, _contracts).ToString("N");
        }

        private void SaveWorkRecord_Btn_Click(object sender, RoutedEventArgs e)
        {
            Project project = (Project) projectInputComboBox.SelectedValue;
            EmployeeDescription employeeDescription = _employeeDescriptions.Where(x => x.Project == project).Single();
            DateTime date = (DateTime) dateInputDatePicker.SelectedDate;
            String[] time = hoursWorkedTextBox.Text.Split(':');
            Int32 minutes = 0;
            try
            {
                minutes = Int32.Parse(time[0]) * 60 + Int32.Parse(time[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Niepoprawny format minutek! Wpisz w formacie hh:mm");
                return;
            }

            String description = descriptionBox.Text;

            if((project == null) || (date == null) || (time == null) || (description == null) || description.Equals(""))
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione!");
                return;
            }

            var wr = new WorkRecord
            {
                CreationDate = DateTime.Now,
                EmployeeDescription = employeeDescription,
                HourlyRate = employeeDescription.HourlyRate,
                MinutesWorked = (uint) minutes,
                WorkStartDate = date,
                Description = description
            };

            _client.SetRecord(wr);  //save to db
            _workRecords.Add(wr);   //update local data
            

            balance.Text = countBalance(_workRecords, _contracts).ToString("N");

            MessageBox.Show("Dodano wpis");
            dateInputDatePicker.SelectedDate = DateTime.Now;
            hoursWorkedTextBox.Text = "hh:mm";
            descriptionBox.Text = "";
        }

        private void monthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime date = (DateTime) monthComboBox.SelectedValue;
            List<WorkRecord> selected = new List<WorkRecord>(_workRecords.Where(x => ((date.Month == x.WorkStartDate.Month) && (date.Year == x.WorkStartDate.Year))));
            dataGridWork.ItemsSource = selected;
        }

        private void monthContractsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime date = (DateTime)monthContractsComboBox.SelectedValue;
            List<Contract> selected = _contracts.FindAll(x => ((date.Month == x.CreationDate.Month) && (date.Year == x.CreationDate.Year)));
            dataGridContracts.ItemsSource = selected;
        }

        private double countBalance(List<WorkRecord> wr, List<Contract> c)
        {
            double wrSum = 0;
            foreach (var rec in wr)
            {
                wrSum += rec.HourlyRate * rec.MinutesWorked / 60;
            }

            double prSum = 0;
            foreach (var rec in c)
            {
                prSum += rec.Value;
            }

            return wrSum - prSum;
        }
    }
}
