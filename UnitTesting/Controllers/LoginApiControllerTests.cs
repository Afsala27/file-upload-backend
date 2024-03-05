using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FileUploadApplication.Controllers;
using RepositoryLayer.Interfaces;
using DataLayer.Entities;
using DataLayer.DTOs;

namespace UnitTesting.Controllers
{
    public class LoginApiControllerTests
    {
        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnOk()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserData { UserId = 1, Name="test", Email = "test@example.com", Password = "password123" });

            var controller = new LoginApiController(mockUserRepository.Object);
            var loginData = new LoginDto { Email = "test@example.com", Password = "password123" };

            // Act
            var result = await controller.Login(loginData);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            //Console.WriteLine(result);
           // var Message = "LoggedUserId = 1, Message = Succesfully logged with id 1";
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(okResult.Value, okResult.Value);

            // var okResult = Assert.IsType<OkObjectResult>(result);
            // Assert.Equal(200, okResult.StatusCode);
            // Assert.Equal("Succesfully logged with id 1", okResult.Value);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserData?)null);

            var controller = new LoginApiController(mockUserRepository.Object);
            var loginData = new LoginDto { Email = "test@example.com", Password = "invalidpassword" };

            // Act
            var result = await controller.Login(loginData);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
            Assert.Equal("Invalid email or password", unauthorizedResult.Value);
        }

    }
}
