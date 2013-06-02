using System;
using System.Runtime.Serialization;

namespace PersistenceLayer.Dto
{
    [DataContract]
    public class WorkRecord : Persistable
    {
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public EmployeeDescription EmployeeDescription { get; set; }
        [DataMember]
        public Double HourlyRate { get; set; }
        [DataMember]
        public UInt32 MinutesWorked { get; set; }
        [DataMember]
        public DateTime WorkStartDate { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public Boolean IsBonus { get; set; }

        public override void Merge(Persistable another)
        {
            CreationDate = ((WorkRecord)another).CreationDate;
            HourlyRate = ((WorkRecord)another).HourlyRate;
            MinutesWorked = ((WorkRecord)another).MinutesWorked;
            WorkStartDate = ((WorkRecord)another).WorkStartDate;
            Description = ((WorkRecord)another).Description;
            IsBonus = ((WorkRecord)another).IsBonus;
        }

        public override string ToString()
        {
            return "{Id:" + Id + ", Employee:" + EmployeeDescription.Employee +
                   ", Minutes:" + MinutesWorked + ", CreationDate:" + CreationDate + ", WorkStartDate:" + WorkStartDate
                   + ", Description: " + Description + ", IsBonus:" + IsBonus + "}";
        }
    }
}
