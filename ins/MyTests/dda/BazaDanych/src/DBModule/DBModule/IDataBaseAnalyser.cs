using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
* Interfejs dla Analizatora
*/
namespace DBModule
{
    public  interface IDataBaseAnalyser
    {
        Dictionary<Tuple<String, String>, int> GetDependencies();
        void SaveDependencies(Dictionary<Tuple<String, String>, int> dependencies);
        List<Producer> GetProducers();
    }
}