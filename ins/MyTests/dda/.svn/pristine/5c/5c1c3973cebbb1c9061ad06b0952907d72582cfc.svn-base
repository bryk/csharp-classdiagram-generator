using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Database;
using DocumentExtractor.Core;
using DocumentExtractor.Model;
using Microsoft.Win32;
using Spring.Context.Support;

namespace DocumentExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Document _document;

        public MainWindow()
        {
            InitializeComponent();
            Path.Text = "Click here to choose the file";
        }

        private void AnalyzeClick(object sender, RoutedEventArgs e)
        {
            if(Path.Text.Length > 0)
            {
                _document = Extractor.ReadStream(new StreamReader(File.OpenRead(Path.Text), Encoding.GetEncoding(28592)));  //ISO-8859-2
                Header.Content = _document.SectionTitle + " - " + _document.TopicTitle;
                foreach (var post in _document.Posts)
                {
                    var item = new ListBoxItem
                                   {
                                       Content = string.Format("{0}, date: {1}", post.Author, post.Date)
                                   };
                    var content = post.Content;
                    item.MouseDoubleClick += (o, args) => MessageBox.Show(content);
                    Posts.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Choose file firstly");
            }
        }

        private void SubmitClick(object sender, RoutedEventArgs e)
        {
            if (_document == null)
            {
                MessageBox.Show("Nothing to send");
            } 
            else
            {
                try
                {
                    var context = ContextRegistry.GetContext();
                    var dbDocumentSender = (IDocumentSender) context.GetObject("DocumentSender");
                    MessageBox.Show(dbDocumentSender.SendDocument(_document) ? "Done!" : "Error!");
                    CancelClick(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                }
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Path.Text = "Click here to choose the file";
            Posts.Items.Clear();
            _document = null;
            Header.Content = "";
        }

        private void PathGotFocus(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".html",
                Filter = "HTML document|*.html"
            };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                Path.Text = dialog.FileName;
            }
        }
    }
}
