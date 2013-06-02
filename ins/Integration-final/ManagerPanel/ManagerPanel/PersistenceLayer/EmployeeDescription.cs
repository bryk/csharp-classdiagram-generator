using System;

namespace PersistenceLayer
{
    public class EmployeeDescription
    {
        public PublicUserInfo Employee { get; set; }
        public Double HourlyRate { get; set; }
        public Project Project { get; set; }
        public UInt32 SumMinutesWorked { get; set; }

        public EmployeeDescription(PublicUserInfo info, Double rate, Project proj)
        {
            this.Employee = info;
            this.HourlyRate = rate;
            this.Project = proj;
        }

        public override string ToString()
        {
            return Employee.ToString();
        }
    }
}
