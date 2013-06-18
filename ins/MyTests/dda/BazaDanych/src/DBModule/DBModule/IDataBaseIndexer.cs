using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Interfejs dla Indeksera
 */

namespace DBModule
{
    public interface IDataBaseIndexer
    {
        Boolean AddPost(string key, Post post);
        Boolean AddThread(string key, ForumThread thread);
        Boolean AddSection(string key, Section section);
    }
}
