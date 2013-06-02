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
    /// Interaction logic for RegisterContractWindow.xaml
    /// </summary>
    public partial class RegisterContractWindow : Window
    {
        private UIClient _db;
        public PublicUserInfo Employee { get; set; }
        public Contract Contract { get; set; }

        public RegisterContractWindow(PublicUserInfo Employee, UIClient db)
        {
            InitializeComponent();
            this.Employee = Employee;
            RegisterContractLayout.DataContext = this;
            Contract = new Contract();
            Contract.Employee = Employee;
            Contract.CreationDate = System.DateTime.Now;
            Contract.Value = 3240;

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
            _db.SetContract(Contract);
            this.Close();
        }

        private void CancelAction()
        {
            this.Close();
        }
    }
}