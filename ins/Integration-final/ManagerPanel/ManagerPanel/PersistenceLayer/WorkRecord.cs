using System;

namespace PersistenceLayer
{
    public class WorkRecord
    {
        public DateTime CreationDate { get; set; }
        public EmployeeDescription _employeeDescription;
        public Double HourlyRate { get; set; }
        public UInt32 MinutesWorked { get; set; }
        public DateTime WorkStartDate { get; set; }
        public bool isBonus { get; set; }
        public bool isCounted { get; set; }
        public String Description { get; set; }

        public EmployeeDescription EmployeeDescription
        {
            get { return _employeeDescription; }
            set { _employeeDescription = value; }
        }

        public WorkRecord() { }

        public WorkRecord(DateTime creationDate, EmployeeDescription emp, Double rate, UInt32 min, DateTime workStart)
        {
            this.CreationDate = creationDate;
            this.EmployeeDescription = emp;
            this.HourlyRate = rate;
            this.MinutesWorked = min;
            this.WorkStartDate = workStart;
            emp.SumMinutesWorked += min;
        }
    }
}
