using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBModule;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Autofac;

namespace ProjectSearching
{
    public class Searcher:ISearcher
    {
        public EntityHandler DataBase {   get; set; }

        //private ClientServer Communication {get; set;}

        private Autofac.IContainer Container { get; set; }

        public Searcher()
        {
            Autofac.ContainerBuilder cb = new Autofac.ContainerBuilder();
        /*
            cb.Register(c => new ClientServer(this));
            cb.RegisterType<ClientServer>().As<ICommunication>();
            cb.RegisterInstance(new ClientServer(this));
            */
            cb.Register(c => new EntityHandler());
            cb.RegisterType<EntityHandler>().As<IDataBaseSearcher>();
            cb.RegisterInstance(new EntityHandler());

            Container = cb.Build();
          //  Communication = Container.Resolve<ClientServer>();
            DataBase = Container.Resolve<EntityHandler>();
        }

        public List<dynamic> ReceiveMessage(DBModule.TypeOfDepth depth, String value)
        {
            return this.DataBase.ReceiveResults(depth, value);
        }

        private DBModule.TypeOfDepth depth;
        private String value;
        public List<dynamic> ResultList { get; set; }
        private Dictionary<String, String> dictSections = new Dictionary<String, String>();
        private Dictionary<String, String> dictThreads = new Dictionary<String, String>();
        private Dictionary<String, String> dict = new Dictionary<String, String>();

        public string ChangeName(string name)
        {
            if (name.Length > 15)
                return name.Substring(0, 15) + "...";
            return name;
        }

        public List<dynamic> search(DBModule.TypeOfDepth depth, String value,TreeView treeView1)
        {
            //IDataBase dataBase = new DataBase();
            ResultList = DataBase.ReceiveResults(depth, value);
            addDataToTree(treeView1, new List<dynamic>(), ResultList, depth, value, 0);

            return ResultList;
        }
        /*public List<dynamic> searchFromAnotherNodes(TypeOfDepth depth, String value, TreeView treeView1)
        {
            //ICommunication communication = new Communication();
        //    List<dynamic> tmpList = Communication.SendMessage(depth, value);
            addDataToTree(treeView1,ResultList, tmpList, depth, value, ResultList.Count);

            ResultList.AddRange(tmpList);
            
            return ResultList;
        }
        */
        private bool isAdded(List<dynamic> list, dynamic value, DBModule.TypeOfDepth depth)
        {
            switch (depth)
            {
                case DBModule.TypeOfDepth.Section:
                    {
                        DBModule.Section temp = value;
                        foreach (DBModule.Section section in list)
                        {
                            if (section.SectionName.Equals(temp.SectionName))
                                return true;
                        }
                        return false;
                    }
                case DBModule.TypeOfDepth.ForumThread:
                    {
                        ForumThread temp = value;
                        foreach (ForumThread thread in list)
                        {
                            if ((thread.SectionName.Equals(temp.SectionName)) && (thread.ThreadName.Equals(temp.ThreadName)))
                                return true;
                        }
                        return false;
                    }
                case DBModule.TypeOfDepth.Post:
                    {
                        DBModule.Post temp = value;
                        foreach (Post post in list)
                        {
                            if ((post.SectionTitle.Equals(temp.SectionTitle)) && (post.TopicTitle.Equals(temp.TopicTitle)) && (post.Author.Equals(temp.Author)) && (post.Content.Equals(temp.Content)) && (post.Date.Equals(temp.Date)))
                                return true;
                        }
                        return false;
                    }
            }
            return false;
        }

        private void addDataToTree(TreeView treeView1, List<dynamic> actualList, List<dynamic> resultList, DBModule.TypeOfDepth depth, String value, int count)
        {
            // if (sendQuery != null)
            // {
            switch (depth)
            {
                case DBModule.TypeOfDepth.Section:
                    {
                        int i = count;
                        foreach (DBModule.Section sectionName in resultList)
                        {
                            if (!isAdded(actualList, sectionName, depth))
                            {
                                TreeViewItem mainItem = new TreeViewItem() { Header = "Section: " + sectionName.SectionName, Name = "item" + i };
                                treeView1.Items.Add(mainItem);
                                mainItem.RegisterName("regName" + i, mainItem);
                                i++;
                            }
                        }
                        break;
                    }
                case DBModule.TypeOfDepth.ForumThread:
                    {
                        int i = count;
                        
                        foreach (ForumThread forumThread in resultList)
                        {
                            if (!isAdded(actualList, forumThread, depth))
                            {
                                if (!dict.ContainsKey(forumThread.SectionName))
                                {
                                    TreeViewItem mainItem = new TreeViewItem() { Header = "Section: " + forumThread.SectionName, Name = "item" + i };
                                    treeView1.Items.Add(mainItem);
                                    dict.Add(forumThread.SectionName, "regName" + i);
                                    mainItem.RegisterName("regName" + i, mainItem);
                                    TreeViewItem subItem = new TreeViewItem() { Header = "Thread: " + forumThread.ThreadName, Name = "subItem" + i };
                                    mainItem.Items.Add(subItem);
                                    subItem.RegisterName("regSubName" + i, subItem);
                                }
                                else
                                {
                                    TreeViewItem searchItem = treeView1.FindName(dict[forumThread.SectionName]) as TreeViewItem;
                                    TreeViewItem subItem = new TreeViewItem() { Header = "Thread: " + forumThread.ThreadName, Name = "subItem" + i };
                                    searchItem.Items.Add(subItem);
                                    subItem.RegisterName("regSubName" + i, subItem);
                                }
                                i++;
                            }
                        }
                        break;
                    }
                case DBModule.TypeOfDepth.Post:
                    {
                        int i = count;
                        
                        foreach (Post post in resultList)
                        {
                            if (!isAdded(actualList, post, depth))
                            {
                                if (!dictSections.ContainsKey(post.SectionTitle))
                                {
                                    TreeViewItem mainItem = new TreeViewItem() { Header = "Section: " + post.SectionTitle, Name = "item" + i };
                                    treeView1.Items.Add(mainItem);
                                    dictSections.Add(post.SectionTitle, "regName" + i);
                                    mainItem.RegisterName("regName" + i, mainItem);
                                    if (!dictThreads.ContainsKey(post.TopicTitle))
                                    {
                                        TreeViewItem subItem = new TreeViewItem() { Header = "Thread: " + post.TopicTitle, Name = "subItem" + i };
                                        mainItem.Items.Add(subItem);
                                        dictThreads.Add(post.TopicTitle, "regSubName" + i);
                                        subItem.RegisterName("regSubName" + i, subItem);
                                        TreeViewItem postItem = new TreeViewItem() { Header = "Post: " + ChangeName(post.Content), Name = "postItem" + i };
                                        subItem.Items.Add(postItem);
                                        postItem.RegisterName("regPostName" + i, postItem);
                                    }
                                    else
                                    {
                                        TreeViewItem searchThreadItem = treeView1.FindName(dictThreads[post.TopicTitle]) as TreeViewItem;
                                        TreeViewItem postItem = new TreeViewItem() { Header = "Post: " + ChangeName(post.Content), Name = "postItem" + i };
                                        searchThreadItem.Items.Add(postItem);
                                        postItem.RegisterName("regPostName" + i, postItem);
                                    }
                                }
                                else
                                {
                                    TreeViewItem searchItem = treeView1.FindName(dictSections[post.SectionTitle]) as TreeViewItem;
                                    if (!dictThreads.ContainsKey(post.TopicTitle))
                                    {
                                        TreeViewItem subItem = new TreeViewItem() { Header = "Thread: " + post.TopicTitle, Name = "subItem" + i };
                                        searchItem.Items.Add(subItem);
                                        subItem.RegisterName("regSubName" + i, subItem);
                                        TreeViewItem postItem = new TreeViewItem() { Header = "Post: " + ChangeName(post.Content), Name = "postItem" + i };
                                        subItem.Items.Add(postItem);
                                        postItem.RegisterName("regPostName" + i, postItem);
                                    }
                                    else
                                    {
                                        TreeViewItem searchThreadItem = treeView1.FindName(dictThreads[post.TopicTitle]) as TreeViewItem;
                                        TreeViewItem postItem = new TreeViewItem() { Header = "Post: " + ChangeName(post.Content), Name = "postItem" + i };
                                        searchThreadItem.Items.Add(postItem);
                                        postItem.RegisterName("regPostName" + i, postItem);
                                    }
                                }
                                i++;
                            }
                        }
                        break;
                    }
            }
        }

        public List<Data> LoadCollectionData(Post post, TextBox textBlock1)
        {
            List<Data> data = new List<Data>();
            data.Add(new Data()
            {
                Attribute = "Section",
                Value = post.SectionTitle
            });
            data.Add(new Data()
            {
                Attribute = "Thread",
                Value = post.TopicTitle
            });

            data.Add(new Data()
            {
                Attribute = "Author",
                Value = post.Author
            });
            data.Add(new Data()
            {
                Attribute = "Date",
                Value = post.Date.ToString()
            });
            textBlock1.Text = post.Content;
            return data;
        }
    }
}
