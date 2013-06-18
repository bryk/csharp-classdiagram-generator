using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DBModule;

namespace Server
{

    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IServerCalback))]
    public interface IServer
    {
        [OperationContract]
        bool Register(String name);

        [OperationContract]
        List<dynamic> SendMessageToServer(TypeOfDepth depth, string value);
    }

    public interface IServerCalback
    {
        [OperationContract]
        List<dynamic> SendMessageToClient(TypeOfDepth depth, string value);
    }
}

