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
using System.Threading;
using DBModule;

namespace DependencyAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Analyzer.Analyzer analyzer;
        private Analyzer.DependencyAnalyzer dependencyAnalyzer;
        public MainWindow()
        {
            InitializeComponent();
            DBModule.DBConnection connection = new DBModule.DBConnection();       
            dependencyAnalyzer = new Analyzer.DependencyAnalyzer(10000, new AnalyserMap());
            analyzer = dependencyAnalyzer.AnalyzerProxy;
            Thread.Sleep(3000);
            Thread t = new Thread(dependencyAnalyzer.Run);
            t.Start();
        }

        private void ShowResults_Click(object sender, RoutedEventArgs e)
        {
            String login = Login.Text;

            int dependencyLevel = 0;
            if (DependencyLevel.Text.Length > 0)
            {
                try
                {
                    dependencyLevel = int.Parse(DependencyLevel.Text);
                }
                catch (System.FormatException)
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
            //MessageBox.Show(login + " " + dependencyLevel);
            Dictionary<Tuple<string, string>, int> x;
            Dictionary<Tuple<String, String>, int> dependencies;
            analyzer.FilterPositiveDependencies(analyzer.GetDependencies(), out dependencies, out x);
            Login1.Items.Clear();
            Login2.Items.Clear();
            Value.Items.Clear();
            
            SortedDictionary<long,Tuple<string,string>> dep = new SortedDictionary<long,Tuple<string,string>>();
            long i = 0;
            long Mult = 1000000000;
            foreach (KeyValuePair<Tuple<String, String>, int> element in dependencies)
                if (dependencyLevel <= element.Value && (login.Equals("") || login.CompareTo(element.Key.Item1) == 0 || login.CompareTo(element.Key.Item2) == 0))
                {
                    dep.Add(Mult * element.Value + i, element.Key);
                    i++;

                }
            i=0;
            foreach(KeyValuePair<long,Tuple<string, string>> element in dep.Reverse())
                if (i < 27)
                {
                    Login1.Items.Add(element.Value.Item1);
                    Login2.Items.Add(element.Value.Item2);
                    Value.Items.Add(element.Key/Mult);
                    i++;
                }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Login2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Login.Text=Login2.Items[Login2.SelectedIndex].ToString();


        }

        private void Value_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyLevel.Text = Value.Items[Value.SelectedIndex].ToString();
        }

        private void Login1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Login.Text = Login1.Items[Login1.SelectedIndex].ToString();
        }


    }
}
