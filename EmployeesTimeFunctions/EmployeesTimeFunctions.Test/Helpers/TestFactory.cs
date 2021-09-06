using EmployeesTimeFunctions.Common.Models;
using EmployeesTimeFunctions.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EmployeesTimeFunctions.Test.Helpers
{
    public class TestFactory
    {
        public static EmployeeEntity GetEmployeeEntity()
        {
            return new EmployeeEntity
            {
                ETag = "*",
                PartitionKey = "Employee",
                RowKey = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now,
                IsConsolidated = false,
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid employeeId, Employee employeeRequest)
        {
            string request = JsonConvert.SerializeObject(employeeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{employeeId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid employeeId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{employeeId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Employee employeeRequest)
        {
            string request = JsonConvert.SerializeObject(employeeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
            };
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        public static Employee GetEmployeeRequest()
        {
            return new Employee
            {
                IsConsolidated = false,
            };
        }

        private static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter write = new StreamWriter(stream);
            write.Write(stringToConvert);
            stream.Flush();
            stream.Position = 0;
            return stream;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if (type == LoggerTypes.List)
            {
                logger = new ListLoggers();
            } else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }
            return logger;
        }
    }
}
