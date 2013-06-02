using System.Runtime.Serialization;

namespace Server.Shared.Exceptions
{
    [DataContract]
    public class PermissionDeniedForUser
    {
        [DataMember]
        public string Message { get; set; }
    }
}