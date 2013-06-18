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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DDA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = e.Source as Button;
            //b.Foreground = Brushes.Red;
        }

        void Close_Me(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
