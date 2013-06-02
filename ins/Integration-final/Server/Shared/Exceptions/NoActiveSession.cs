using System.Runtime.Serialization;

namespace Server.Shared.Exceptions
{
    [DataContract]
    public class NoActiveSession
    {
        [DataMember]
        public string Message { get; set; }

    }
}