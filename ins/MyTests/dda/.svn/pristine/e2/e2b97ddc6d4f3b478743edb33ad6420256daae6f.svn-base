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
using DBModule;
using ProjectSearching;

namespace Project.GUI
{
    /// <summary>
    /// Interaction logic for SelectAttributesWindow.xaml
    /// </summary>
    public partial class SelectAttributesWindow : Window
    {
        private string depth = null;
        private String attributeName;
        private String value;
        private Searcher searcher;

        public SelectAttributesWindow()
        {
            InitializeComponent();

            //Searcher searcher = new Searcher();
            //searcher.addDataToTree();
        }

        public SelectAttributesWindow(string depth)
        {
            this.depth = depth;
            InitializeComponent();

            List<String> attributes = new List<String>();
            attributes.Add("Producer");
            listBox1.ItemsSource = attributes;
            this.searcher = new Searcher();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (label1.Content.ToString() == "Query")
            {
                if (MessageBox.Show(this, "Please, set query!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                    this.Show();
            }
            else
            {
                Result window = null;
                switch (depth)
                {
                    case "Section":
                        {
                            //window = new Result(TypeOfDepth.Section, attributeName, value);
                            window = Result.run(TypeOfDepth.Section, attributeName, value);
                            //window.showResultsFromCommunication(TypeOfDepth.Section, attributeName, value);
                            break;
                        }
                    case "Thread":
                        {
                            //window = new Result(TypeOfDepth.Thread, attributeName, value);
                            window = Result.run(TypeOfDepth.ForumThread, attributeName, value);
                            //window.showResultsFromCommunication(TypeOfDepth.ForumThread, attributeName, value);
                            break;
                        }
                    case "Post":
                        {
                            //window = new Result(TypeOfDepth.Post, attributeName, value);
                            window = Result.run(TypeOfDepth.Post, attributeName, value);
                            //window.showResultsFromCommunication(TypeOfDepth.Post, attributeName, value);
                            break;
                        }
                }
                this.Close();
                window.Show();
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want cancel this wizard?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ChooseTypesWindow window = new ChooseTypesWindow(depth);
            this.Close();
            window.Show();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            Object item = listBox1.SelectedItem;
            if (item == null)
            {
                if (MessageBox.Show(this, "Please, select attribute!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    this.Show();
                }
            }
            else
            {
                String attribute = item.ToString();
                value = textBox1.Text;
                if (value == "")
                {
                    if (MessageBox.Show(this, "Please, type value!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                    {
                        this.Show();
                    }
                }
                else
                {
                    label1.Content = attribute + " = '" + value + "' -> depth = '" + depth + "'";
                    
                    string Depth = depth;
                    string AttributeName = attribute;
                    string Value = value;
                }
            }  
        }
    }
}
