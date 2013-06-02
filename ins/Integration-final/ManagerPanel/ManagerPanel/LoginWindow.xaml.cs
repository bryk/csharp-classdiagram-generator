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
using Client;

namespace ManagerPanel
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ManagerClient client = new ManagerClient();

            if (client.Login(loginBox.Text, passwordBox.Password) == true)
            {
                MainWindowViewModel.db = client;
                this.Hide();
                new MainWindow(loginBox.Text).ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Nieprawidłowy login/hasło");
            }
        }
    }
}
