using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PersistenceLayer.Dto;
using Client;

namespace ManagerPanel
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddBonusWindow : Window
    {
        private UIClient _db;
      //  AddBonusViewModel _viewModel = new AddBonusViewModel();
        public EmployeeDescription Employee { get; set; }
        public WorkRecord Bonus { get; set; }

        public AddBonusWindow(EmployeeDescription Employee, UIClient db)
        {

            InitializeComponent();
            this.Employee = Employee;
            AddBonusLayout.DataContext = this;
            Bonus = new WorkRecord();
            Bonus.EmployeeDescription = Employee;
            Bonus.CreationDate = DateTime.Now;
            Bonus.HourlyRate = 1000;
       //     Bonus.isBonus = true;
            Bonus.Description = null;
            Bonus.MinutesWorked = 60;
            System.Console.WriteLine(Bonus.CreationDate);
            _db = db;
        }

        public ICommand OkCommand
        {
            get
            {
                return (new RelayCommand(OkAction));
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                return (new RelayCommand(CancelAction));
            }
        }
        
        private void OkAction(){
            _db.SetRecord(Bonus);
            this.Close();
        }

        private void CancelAction(){
            this.Close();
        }


    }
}
