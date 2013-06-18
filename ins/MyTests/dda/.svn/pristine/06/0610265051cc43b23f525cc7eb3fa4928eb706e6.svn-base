using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Forms;
using ProjectDatabase;
using ProjectSearching;
using Autofac;


namespace Project.GUI
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : Window
    {
        private TypeOfDepth depth;
        private String attributeName;
        private String value;
        private List<dynamic> resultList;
        private Searcher searcher;

        static List<dynamic> list;
        static System.Windows.Controls.TreeView tree;
        static Searcher s;

        private delegate void DummyDelegate();

        public Result()
        {
            InitializeComponent();           
        }

        public Result(TypeOfDepth depth, String attributeName, String value)
        {
            InitializeComponent();
            if (depth != TypeOfDepth.Post)
            {
                dataGrid1.Visibility = System.Windows.Visibility.Hidden;
                label1.Visibility = System.Windows.Visibility.Hidden;
                textBlock1.Visibility = System.Windows.Visibility.Hidden;
            }
            dataGrid1.AutoGenerateColumns = true;
            /*
            DataGridTextColumn dgtc = new DataGridTextColumn();
            dgtc.Header = "Attribute";
            dataGrid1.Columns.Add(dgtc);
            DataGridTextColumn dgtc2 = new DataGridTextColumn();
            dgtc2.Header = "Value";
            dataGrid1.Columns.Add(dgtc2);
            */
            this.depth = depth;
            this.attributeName = attributeName;
            this.value = value;
            this.searcher=new Searcher();
            this.resultList = new List<dynamic>();

            s = searcher;
            list = resultList;
            tree = treeView1;
       }
        internal static Result run(TypeOfDepth depth, String attributeName, String value)
        {
            Result result = new Result(depth, attributeName, value);
            list = s.search(depth, value, tree);

            System.Threading.Thread.Sleep(5000);

            list = s.searchFromAnotherNodes(depth, value, tree);

            return result;
        }
        public void showResultsFromCommunication(TypeOfDepth depth, String attributeName, String value)
        {
            //Result result = new Result(depth, attributeName, value);
            //s.search(depth, value, tree);
            var timer=new System.Timers.Timer(3000);
            timer.Enabled = true;

            timer.Elapsed += delegate
            {
                this.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                (DummyDelegate)
                delegate { list=s.searchFromAnotherNodes(this.depth, value, tree); });
            };

            timer.Start(); 
        }


        
        private void treeView1_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tmp = (TreeViewItem)e.Source;
            if (String.CompareOrdinal(tmp.Name, 0, "p", 0, 1) == 0)
            {
                int i=0;
                bool m=false;
                while (i < list.Count && !m)
                {
                    Post post=list[i];
                    if (("Post: " + s.ChangeName(post.Content)) == (string)tmp.Header)
                    {
                        dataGrid1.ItemsSource = s.LoadCollectionData(post, textBlock1);
                        m = true;
                    }
                    i++;
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ChooseTypesWindow Window = new ChooseTypesWindow();
            Window.Show();
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show(this, "Are you sure you want cancel this wizard?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        public System.Windows.Controls.TreeView getTreeView()
        {
            return tree;
        }
    }
}
