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
    /// Interaction logic for SummaryWindow.xaml
    /// </summary>
    public partial class SummaryWindow : Window
    {
        SummaryWindowViewModel viewModel;
        public SummaryWindow(UIClient db, EmployeeDescription employee){            
            InitializeComponent();
            viewModel = new SummaryWindowViewModel(db, employee);
            viewModel.RequestClose += (s, e) => this.Close();
            SummaryLayout.DataContext = viewModel;
        }
    }
}
