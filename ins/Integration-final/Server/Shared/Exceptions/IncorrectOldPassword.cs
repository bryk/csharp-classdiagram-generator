using System.Runtime.Serialization;

namespace Server.Shared.Exceptions
{
    [DataContract]
    public class IncorrectOldPassword
    {
        [DataMember]
        public string Message { get; set; }
    }
}
