using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Server
{
    class WcfSessionIdProvider : ISessionIdProvider
    {
        public string GetSessionId()
        {
            return OperationContext.Current.SessionId;
        }
    }
}
