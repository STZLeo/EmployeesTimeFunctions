using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using EmployeesTimeFunctions.Common.Models;
using EmployeesTimeFunctions.Common.Responses;
using EmployeesTimeFunctions.Functions.Entities;

namespace EmployeesTimeFunctions.Functions.Functions
{
    public static class Consolidate
    {
        [FunctionName(nameof(RunConsolidate))]
        public static async Task RunConsolidate(
            [TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
            [Table("employeesTime", Connection = "AzureWebJobsStorage")] CloudTable employeesTable,
            [Table("employeesConsolidate", Connection = "AzureWebJobsStorage")] CloudTable consolidateTable,
            ILogger log)
        {

            // log.LogInformation($"Consolidate function ran successfully, {} and executed at: {DateTime.Now}");
            string filter = TableQuery.GenerateFilterConditionForInt("Type", QueryComparisons.Equal, 1);
            TableQuery<EmployeeEntity> query = new TableQuery<EmployeeEntity>().Where(filter);
            TableQuerySegment<EmployeeEntity> consolidatedRecords = await employeesTable.ExecuteQuerySegmentedAsync(query, null);
            int consolidated = 0;

            foreach (var consolidatedRecord in consolidatedRecords)
            {
                await employeesTable.ExecuteAsync(TableOperation.Delete(consolidatedRecord));
                consolidated++;
            }

        }
    }
}
