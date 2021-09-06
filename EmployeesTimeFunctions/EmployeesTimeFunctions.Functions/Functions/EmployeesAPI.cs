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

            if (employee.DateTime == DateTime.MinValue || string.IsNullOrEmpty(employee?.EmployeeId) || employee.Type == null || employee.Type > 1 || employee.Type < 0)
            {
                return new BadRequestObjectResult(new Responses
                {
                    IsSuccess = false,
                    Message = "The request must have Data time, Employee Id and Type filled"
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

        [FunctionName(nameof(UpdateTime))]
        public static async Task<IActionResult> UpdateTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "employeesTime/{id}")] HttpRequest req,
            [Table("employeesTime", Connection = "AzureWebJobsStorage")] CloudTable employeesTable,
            string id,
            ILogger log)
        {
            log.LogInformation($"Received an update, id: {id}");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Employee employee = JsonConvert.DeserializeObject<Employee>(requestBody);

            //Validate time entry

            TableOperation findOperation = TableOperation.Retrieve<EmployeeEntity>("Employee", id);
            TableResult findResult = await employeesTable.ExecuteAsync(findOperation);

            if (findResult.Result == null)
            {
                return new BadRequestObjectResult(new Responses
                {
                    IsSuccess = false,
                    Message = "Entry not found"
                });
            };

            //Update time entry 

            EmployeeEntity employeeEntity = (EmployeeEntity)findResult.Result;
            if (!string.IsNullOrEmpty(requestBody))
            {
                if (employee.IsConsolidated != employeeEntity.IsConsolidated || employee.IsConsolidated != null || employee.DateTime != DateTime.MinValue)
                {
                    employeeEntity.IsConsolidated = employee.IsConsolidated;
                    
                } else 
                {
                    return new BadRequestObjectResult(new Responses
                    {
                        IsSuccess = false,
                        Message = "Entry not found"
                    });
                }             
            }

            TableOperation addOperation = TableOperation.Replace(employeeEntity);
            await employeesTable.ExecuteAsync(addOperation);

            string message = $"Time entry with id: {id} was modifed";
            log.LogInformation(message);

            return new OkObjectResult(new Responses
            {
                IsSuccess = true,
                Message = message,
                Result = employeeEntity

            });
        }

        [FunctionName(nameof(GetAllTimes))]
            public static async Task<IActionResult> GetAllTimes(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employeesTime")] HttpRequest req,
            [Table("employeesTime", Connection = "AzureWebJobsStorage")] CloudTable employeesTable,
            ILogger log)
        {
            log.LogInformation("Retrieving all employees Time");

            TableQuery<EmployeeEntity> query = new TableQuery<EmployeeEntity>();
            TableQuerySegment<EmployeeEntity> times = await employeesTable.ExecuteQuerySegmentedAsync(query, null);

            string message = $"Got all employees time";
            log.LogInformation(message);

            return new OkObjectResult(new Responses
            {
                IsSuccess = true,
                Message = message,
                Result = times
            });
        }

        [FunctionName(nameof(GetTimeById))]
        public static IActionResult GetTimeById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employeesTime/{id}")] HttpRequest req,
            [Table("employeesTime", "Employee", "{id}", Connection = "AzureWebJobsStorage")] EmployeeEntity employeeEntity,
            string id,
            ILogger log)
        {

            log.LogInformation("Getting employee record by ID");

            if (employeeEntity == null)
            {
                return new BadRequestObjectResult(new Responses
                {
                    IsSuccess = false,
                    Message = "Employee record not found"
                });
            }

            string message = $"Employee record #{employeeEntity.EmployeeId} being shown";
            log.LogInformation(message);

            return new OkObjectResult(new Responses
            {
                IsSuccess = true,
                Message = message,
                Result = employeeEntity
            });
        }

        [FunctionName(nameof(DeleteRecord))]
        public static async Task<IActionResult> DeleteRecord(
    [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "employeesTime/{id}")] HttpRequest req,
    [Table("employeesTime", "Employee", "{id}", Connection = "AzureWebJobsStorage")] EmployeeEntity employeeEntity,
    [Table("employeesTime", Connection = "AzureWebJobsStorage")] CloudTable employeesTable,
    string id,
    ILogger log)
        {

            log.LogInformation($"Delete employee record {id}, received");

            if (employeeEntity == null)
            {
                return new BadRequestObjectResult(new Responses
                {
                    IsSuccess = false,
                    Message = "Employee record not found"
                });
            }

            await employeesTable.ExecuteAsync(TableOperation.Delete(employeeEntity));

            string message = $"Employee record #{employeeEntity.EmployeeId}, deleted";
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
