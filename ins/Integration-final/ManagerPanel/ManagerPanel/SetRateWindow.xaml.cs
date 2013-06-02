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
    /// Interaction logic for SetRateWindow.xaml
    /// </summary>
    public partial class SetRateWindow : Window
    {
        private  UIClient _db;
        //  AddBonusViewModel _viewModel = new AddBonusViewModel();
        public EmployeeDescription Employee { get; set; }
        public Double NewHourlyRate { get; set; }

        public SetRateWindow(EmployeeDescription Employee, UIClient db)
        {

            InitializeComponent();
            this.Employee = Employee;
            SetRateLayout.DataContext = this;
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

        private void OkAction()
        {
             _db.SetHourlyRate(Employee, NewHourlyRate);
             this.Close();
        }

        private void CancelAction()
        {
            this.Close();
        }


    }
}
