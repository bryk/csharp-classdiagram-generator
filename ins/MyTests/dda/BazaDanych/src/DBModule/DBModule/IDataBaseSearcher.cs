using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Interfejs dla Wyszukiwarki
 */

namespace DBModule
{
    public interface IDataBaseSearcher
    {
        List<dynamic> ReceiveResults(TypeOfDepth depth, String value);
    }
}
