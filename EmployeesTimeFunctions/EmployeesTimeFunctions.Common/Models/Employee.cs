using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesTimeFunctions.Common.Models
{
    public class Employee
    {
        public Index EmployeeId { get; set; }
        public DateTime DateTime { get; set; }
        public int Type { get; set; }
        public Boolean IsConsolidated { get; set; }
    }
}
