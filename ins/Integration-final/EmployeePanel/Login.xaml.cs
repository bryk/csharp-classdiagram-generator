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

namespace EmployeePanel
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            loginBox.Focus();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            UIClient client = new EmployeeClient();

            if (client.Login(loginBox.Text, passBox.Password) == true)
            {
                this.Hide();
                new MainWindow(client, loginBox.Text).ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Nieprawidłowy login/hasło");
            }
        }
    }
}
