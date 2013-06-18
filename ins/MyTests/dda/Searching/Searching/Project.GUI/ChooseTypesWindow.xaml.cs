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

namespace Project.GUI
{
    /// <summary>
    /// Interaction logic for ChooseTypesWindow.xaml
    /// </summary>
    public partial class ChooseTypesWindow : Window
    {
        private String depth;

        public ChooseTypesWindow()
        {
            InitializeComponent();
        }

        public ChooseTypesWindow(string depth)
        {
            InitializeComponent();
            switch (depth)
            {
                case "Section":
                    {
                        radioButton1.IsChecked = true;
                        break;
                    }
                case "Thread":
                    {
                        radioButton2.IsChecked = true;
                        break;
                    }
                case "Post":
                    {
                        radioButton3.IsChecked = true;
                        break;
                    }
            }
            this.depth = depth;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            window.Show();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to cancel this wizard?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (radioButton1.IsChecked == true || radioButton2.IsChecked == true || radioButton3.IsChecked == true)
            {
                SelectAttributesWindow window = new SelectAttributesWindow(depth);
                this.Close();
                window.Show();
            }
            else
                if (MessageBox.Show(this, "Please, select depth!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    this.Show();
                }
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            depth=radioButton1.Content.ToString();
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            depth = radioButton2.Content.ToString();
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            depth = radioButton3.Content.ToString();
        }
        /*
        public String Depth
        {
            get { return depth; }
            set { depth = value; }
        }
         */
    }
}
