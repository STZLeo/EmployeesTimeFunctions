using EmployeesTimeFunctions.Common.Models;
using EmployeesTimeFunctions.Functions.Entities;
using EmployeesTimeFunctions.Functions.Functions;
using EmployeesTimeFunctions.Test.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace EmployeesTimeFunctions.Test.Tests
{
    public class EmployeesAPITests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

       /* [Fact]
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
        }*/

        [Fact]
        public async void UpdateTime_Should_Return_200()
        {
            // Arrange
            MockCloudTableEmployees mockEmployee = new MockCloudTableEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Employee employeeRequest = TestFactory.GetEmployeeRequest();
            Guid employeeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(employeeId, employeeRequest);

            // Act
            IActionResult response = await EmployeesAPI.UpdateTime(request, mockEmployee, employeeId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }

        /*[Fact]
        public async void GetAllTimes_Should_Return_200()
        {
            // Arrange
            MockCloudTableEmployees mockEmployee = new MockCloudTableEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Employee employeeRequest = TestFactory.GetEmployeeRequest();
            Guid employeeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(employeeId, employeeRequest);

            // Act
            IActionResult response = await EmployeesAPI.GetAllTimes(request, mockEmployee, logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }*/

        [Fact]
        public void GetTimeById_Should_Return_200()
        {
            // Arrange
            Employee employeeRequest = TestFactory.GetEmployeeRequest();
            EmployeeEntity employeeEntity = TestFactory.GetEmployeeEntity();
            Guid employeeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(employeeId, employeeRequest);

            // Act
            IActionResult response = EmployeesAPI.GetTimeById(request, employeeEntity, employeeId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void DeleteTime_Should_Return_200()
        {
            // Arrange
            MockCloudTableEmployees mockEmployee = new MockCloudTableEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Employee employeeRequest = TestFactory.GetEmployeeRequest();
            EmployeeEntity employeeEntity = TestFactory.GetEmployeeEntity();
            Guid employeeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(employeeId, employeeRequest);

            // Act
            IActionResult response = await EmployeesAPI.DeleteRecord(request, employeeEntity, mockEmployee, employeeId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
