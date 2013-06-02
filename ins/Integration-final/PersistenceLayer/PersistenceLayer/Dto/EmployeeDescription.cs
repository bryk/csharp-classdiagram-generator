using System;
using System.Runtime.Serialization;

namespace PersistenceLayer.Dto
{
    [DataContract]
    public class EmployeeDescription : Persistable
    {
        [DataMember]
        public PublicUserInfo Employee { get; set; }
        [DataMember]
        public Double HourlyRate { get; set; }
        [DataMember]
        public Project Project { get; set; }
        [DataMember]
        public UInt32 SumMinutesWorked { get; set; }

        public override void Merge(Persistable another)
        {
            HourlyRate = ((EmployeeDescription)another).HourlyRate;
            SumMinutesWorked = ((EmployeeDescription)another).SumMinutesWorked;
        }

        public override string ToString()
        {
            return "{Id: " + Id + ", Employee:" + Employee + ", HourlyRate:" + HourlyRate + ", Project: " + Project +
                ", SumMinutesWorked:" + SumMinutesWorked + "}";
        }
    }
}
