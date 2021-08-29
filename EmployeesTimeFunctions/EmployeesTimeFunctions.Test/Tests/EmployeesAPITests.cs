using EmployeesTimeFunctions.Common.Models;
using EmployeesTimeFunctions.Functions.Functions;
using EmployeesTimeFunctions.Test.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EmployeesTimeFunctions.Test.Tests
{
    public class EmployeesAPITests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();
        
        [Fact]
        public async void CreateTime_Should_Return_200()
        {
            //Arrange
            MockCloudTableEmployees mockEmployee = new MockCloudTableEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Employee employeeRequest = TestFactory.GetEmployeeRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(employeeRequest);

            //Act
            IActionResult response = await EmployeesAPI.CreateTime(request, mockEmployee, logger);

            //Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
