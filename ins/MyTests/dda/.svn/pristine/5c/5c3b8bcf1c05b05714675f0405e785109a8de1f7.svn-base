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

namespace GuiAnalyzer
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

        private void ShowResults_Click(object sender, RoutedEventArgs e)
        {
            String login = Login.Text;
            
            int dependencyLevel=0;
            if (DependencyLevel.Text.Length > 0)
            {
                try
                {
                    dependencyLevel = int.Parse(DependencyLevel.Text);
                }
                catch (System.FormatException )
                {
                    MessageBox.Show("Invalid format of Dependency Level: nonnegativ number required");
                    return;
                }
                if (dependencyLevel < 0)
                {
                    MessageBox.Show("Invalid format of Dependency Level: nonnegativ number required");
                    return;
                }
            }
            MessageBox.Show(login +" "+ dependencyLevel);
            Dictionary<Tuple<String, String>, int> dependencies = DependencyAnalyzer.DependencyAnalyzer.GetDependencies();
            Login1.Items.Clear();
            Login2.Items.Clear();
            Value.Items.Clear();
            int i = 0;
            foreach (KeyValuePair<Tuple<String, String>, int> element in dependencies)
                if (dependencyLevel <= element.Value && (login.Equals("") || login.CompareTo(element.Key.Item1)==0 || login.CompareTo(element.Key.Item2)==0))
                {
                    i++;
                Login1.Items.Add(element.Key.Item1);
                Login2.Items.Add(element.Key.Item2);
                Value.Items.Add(element.Value);
            }
            MessageBox.Show(""+i);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }




    }
}
