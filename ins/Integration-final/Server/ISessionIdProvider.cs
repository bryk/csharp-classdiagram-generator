using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    // needed for mocking in unit tests
    public interface ISessionIdProvider
    {
        string GetSessionId();
    }
}
