using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using DataLayer.DTOs.User;
using DataLayer.Entities;
using FileUploadApplication.Controllers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryLayer.Interfaces;
using Xunit;

namespace UnitTesting.Controllers
{
    public class UserRegistrationTests
    {

    [Fact]
    public async Task UserRegistration_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockValidator = new Mock<IValidator<UserData>>();
        var mockLogger = new Mock<ILogger<UserRegistrationController>>();

        var controller = new UserRegistrationController(mockUserRepository.Object, mockValidator.Object, mockLogger.Object);
        var userData = new UserData { UserId = 1, Name = "test", Email = "test@example.com", Password = "Test@123" };

        // Act
        var result = await controller.Resgister(userData);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("Registration Succesfull", okResult.Value);

        // Verify that the UserRepository.AddAsync method was called once with the provided UserData
        mockUserRepository.Verify(repo => repo.AddAsync(userData), Times.Once);

        // Verify that the logger was called with the message "Registration Succesfull"
        //mockLogger.Verify(logger => logger.LogDebug("Registration Succesfull"));
    }

        [Fact]
        public async Task GetUsers_WithValidId_ShouldReturnOk()
        {
            // Arrange
            int userId = 1;
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserRegistrationController>>();
             var mockValidator = new Mock<IValidator<UserData>>();

            var controller = new UserRegistrationController(mockUserRepository.Object,mockValidator.Object, mockLogger.Object);
            var userDto = new UserData { UserId = userId, Name = "Test User", Email = "test@example.com", Password = "Pass@123" };
            mockUserRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new UserData { UserId = userId, Name="Test User", Email = "test@example.com", Password = "Pass@123"} ); // Return the UserDto directly

            // Act
            var result = await controller.GetUsers(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserDto = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(userId, returnedUserDto.UserId);
            // Add more assertions as needed for other properties
        }

        [Fact]
        public async Task GetUsers_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            int userId = 100;
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserRegistrationController>>();
            var mockValidator = new Mock<IValidator<UserData>>();

            var controller = new UserRegistrationController(mockUserRepository.Object, mockValidator.Object, mockLogger.Object);
            mockUserRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((UserData?)null);

            // Act
            var result = await controller.GetUsers(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Specific Data not found", notFoundResult.Value);
        }


    }
}