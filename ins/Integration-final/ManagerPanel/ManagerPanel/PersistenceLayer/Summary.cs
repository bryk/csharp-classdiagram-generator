using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceLayer;

namespace ManagerPanel.PersistenceLayer
{
    public class Summary
    {
        public EmployeeDescription Employee {get; set;}
        public UInt32 MinutesWorked {get; set;}
        public Double MoneyWorked {get; set;}
        public DateTime SummaryDate { get; set; }
    }
}
