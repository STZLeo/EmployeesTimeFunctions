using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EmployeesTimeFunctions.Functions.Entities
{
    public class EmployeeEntity : TableEntity
    {
        public string EmployeeId { get; set; }
        public DateTime DateTime { get; set; }
        public int Type { get; set; }
        public bool IsConsolidated { get; set; }
    }
}
