using System;

namespace EmployeesTimeFunctions.Common.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public DateTime DateTime { get; set; }
        public int Type { get; set; }
        public bool IsConsolidated { get; set; }
    }
}
