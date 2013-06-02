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
using PersistenceLayer;
using PersistenceLayer.Dto;
using Client;

namespace ManagerPanel
{
    /// <summary>
    /// Interaction logic for EmployeeWindows.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private static EmployeeDescription _currentEmployee;
        private static UIClient _db;
        
        
        public EmployeeWindow(EmployeeDescription e, UIClient db)
        {
            InitializeComponent();

            _currentEmployee = e;
            _db = db;
            //EmployeeWindowViewModel _viewModel = new EmployeeWindowViewModel(_currentEmployee);
            //LayoutRoot.DataContext = _viewModel;
            empUI.DataContext = new EmployeeWindowViewModel(_currentEmployee, _db);
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

        }
    }
}
