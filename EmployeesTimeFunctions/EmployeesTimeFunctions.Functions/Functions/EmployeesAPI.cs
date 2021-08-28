using EmployeesTimeFunctions.Common.Models;
using EmployeesTimeFunctions.Common.Responses;
using EmployeesTimeFunctions.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EmployeesTimeFunctions.Functions.Functions
{
    public static class EmployeesAPI
    {
        [FunctionName(nameof(CreateTime))]
        public static async Task<IActionResult> CreateTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employeesTime")] HttpRequest req,
            [Table("employeesTime", Connection = "AzureWebJobsStorage")] CloudTable employeesTable,
            ILogger log)
        {
            log.LogInformation("Received a new entry for the table");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Employee employee = JsonConvert.DeserializeObject<Employee>(requestBody);

            if (employee.EmployeeId < 1 || employee.Type < 0 || employee.Type > 1 || employee.DateTime == null || string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestObjectResult(new Responses
                {
                    IsSuccess = false,
                    Message = "The request must have a validate datetime"
                });
            }

            EmployeeEntity employeeEntity = new EmployeeEntity
            {
                EmployeeId = employee.EmployeeId,
                DateTime = employee.DateTime,
                Type = employee.Type,
                ETag = "*",
                IsConsolidated = false,
                PartitionKey = "Employee",
                RowKey = Guid.NewGuid().ToString()
            };

            TableOperation addOperation = TableOperation.Insert(employeeEntity);
            await employeesTable.ExecuteAsync(addOperation);

            string message = "New entry stored in table";
            log.LogInformation(message);

            return new OkObjectResult(new Responses
            {
                IsSuccess = true,
                Message = message,
                Result = employeeEntity

            });
        }
    }
}
