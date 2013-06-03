﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace ManagerPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //MainWindowViewModel _viewModel = new MainWindowViewModel();
        public MainWindow(String username)
        {
            InitializeComponent();
            MainWindowViewModel _viewModel = new MainWindowViewModel(username);
            UI.DataContext = _viewModel;
            //pie1.Data = _viewModel.PieCollection;
            chart1.Data = _viewModel.PieCollection;
            this.Name = username;
           // employeesList.DataContext = _viewModel;
        }
    }

      
}