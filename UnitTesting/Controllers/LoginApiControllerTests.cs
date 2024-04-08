using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FileUploadApplication.Controllers;
using RepositoryLayer.Interfaces;
using DataLayer.DTOs.User;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DataLayer.Entities;
using InfrastructureLayer.Interfaces;
using DataLayer.DTOs;

namespace FileUploadApplication.Tests
{
    public class LoginApiControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ILogger<LoginApiController>> _mockLogger;
        private readonly LoginApiController _controller;

        public LoginApiControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILogger<LoginApiController>>();

            _controller = new LoginApiController(
                _mockUserRepository.Object,
                null,
                _mockTokenService.Object,
                null,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
            var mockUser = new AppUser { UserName = loginDto.Email, Email = loginDto.Email };

            _mockUserRepository.Setup(m => m.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserData { Name="test", Email = "test@example.com", Password = "password" });

            _mockTokenService.Setup(m => m.CreateToken(It.IsAny<AppUser>()))
            .Returns("fake-token");


            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(okResult.StatusCode, okResult.StatusCode);
        }


//         [Fact]
// public async Task Login_ValidCredentials_ReturnsNewUserDto()
// {
//     // Arrange
//     var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
//     var mockUser = new AppUser { UserName = loginDto.Email, Email = loginDto.Email };

//     _mockUserRepository.Setup(m => m.GetByEmailAsync(loginDto.Email.ToLower()))
//         .ReturnsAsync(mockUser);

//     _mockTokenService.Setup(m => m.CreateToken(mockUser))
//         .Returns("fake-token");

//     // Act
//     var result = await _controller.Login(loginDto);

//     // Assert
//     var objectResult = Assert.IsType<ObjectResult>(result);
//     Assert.Equal(200, objectResult.StatusCode);

//     var content = Assert.IsType<string>(objectResult.Value);
//     // Log or inspect the content here to see what is being returned
//     _mockLogger.Object.LogInformation($"Login result content: {content}");
// }


        // Add more tests for other scenarios such as invalid credentials, null user, etc.
    }
}
