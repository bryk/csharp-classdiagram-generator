using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectSearching;

namespace ProjectCommunication
{
    public interface ICommunication
    {
        List<dynamic> SendMessage(TypeOfDepth depth, String value);
    }
}
