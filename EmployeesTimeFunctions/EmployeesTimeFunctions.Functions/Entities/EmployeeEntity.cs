using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesTimeFunctions.Functions.Entities
{
    public class EmployeeEntity : TableEntity
    {
        public Index EmployeeId { get; set; }
        public DateTime DateTime { get; set; }
        public int Type { get; set; }
        public Boolean IsConsolidated { get; set; }
    }
}
