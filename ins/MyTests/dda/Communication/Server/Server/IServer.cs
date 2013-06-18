using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Server
{

    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IServerCalback))]
    public interface IServer
    {
        [OperationContract]
        bool Register(String name);

        [OperationContract]
        List<dynamic> SendMessageToServer(string depth, string postAttribute, string value);
    }

    public interface IServerCalback
    {
        [OperationContract]
        List<dynamic> SendMessageToClient(string depth, string postAttribute, string value);
    }
}

