using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProjectSearching
{
    public interface ISearcher
    {
        List<dynamic> ReceiveMessage(TypeOfDepth depth, String value);
        
        string ChangeName(string name);

        List<dynamic> search(TypeOfDepth depth, String value,TreeView treeView1);
        List<dynamic> searchFromAnotherNodes(TypeOfDepth depth, String value, TreeView treeView1);

        List<Data> LoadCollectionData(Post post, TextBox textBlock1);

    }
}
