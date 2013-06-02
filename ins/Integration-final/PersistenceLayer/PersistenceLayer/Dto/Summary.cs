using System;
using System.Runtime.Serialization;

namespace PersistenceLayer.Dto
{
    [DataContract]
    public class Summary : Persistable
    {
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public UInt32 MinutesWorked { get; set; }
        [DataMember]
        public UInt32 MoneyWorked { get; set; }
        [DataMember]
        public EmployeeDescription EmployeeDescription { get; set; }

        public override void Merge(Persistable other)
        {
            Date = ((Summary)other).Date;
            MinutesWorked = ((Summary)other).MinutesWorked;
            MoneyWorked = ((Summary)other).MoneyWorked;
        }

        public override string ToString()
        {
            return String.Format("{Id: {0}, Employee: {1}, MinutesWorked: {2}, MoneyWorked: {3}}",
                Id, EmployeeDescription, MinutesWorked, MoneyWorked);
        }
    }
}
