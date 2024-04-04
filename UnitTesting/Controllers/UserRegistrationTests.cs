using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using DataLayer.DTOs.User;
using DataLayer.Entities;
using FileUploadApplication.Controllers;
using FluentValidation;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        public async Task Register_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockValidator = new Mock<IValidator<UserData>>();
            var mockLogger = new Mock<ILogger<UserRegistrationController>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockTokenService = new Mock<ITokenService>();

            var controller = new UserRegistrationController(
                mockUserRepository.Object,
                mockValidator.Object,
                mockLogger.Object,
                mockUserManager.Object,
                mockTokenService.Object);

           var registerDto = new RegisterDto { Name = "TestUser", Email = "test@example.com", Password = "Test@123" };
            // Mock repository behavior
            // mockUserRepository.Setup(repo => repo.GetByEmailAsync(registerDto.Email))
            //     .ReturnsAsync((AppUser)null); // Simulate no existing user with provided email

           mockUserRepository.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((UserData)null); // Simulate no existing user with provided email

            // Mock Identity UserManager behavior
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success); // Simulate successful user creation

            mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<AppUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success); // Simulate successful role assignment

            mockTokenService.Setup(ts => ts.CreateToken(It.IsAny<AppUser>()))
                .Returns("test_token"); // Simulate token creation

            // Act
            var result = await controller.Resgister(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var newUserDto = Assert.IsType<NewUserDto>(okResult.Value);
            Assert.Equal(registerDto.Name, newUserDto.Name);
            Assert.Equal(registerDto.Email, newUserDto.Email);
            Assert.Equal("test_token", newUserDto.Token);
        }

        [Fact]
        public async Task Register_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockValidator = new Mock<IValidator<UserData>>();
            var mockLogger = new Mock<ILogger<UserRegistrationController>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockTokenService = new Mock<ITokenService>();

            var controller = new UserRegistrationController(
                mockUserRepository.Object,
                mockValidator.Object,
                mockLogger.Object,
                mockUserManager.Object,
                mockTokenService.Object);


            var registerDto = new RegisterDto { Name = "", Email = "", Password = "" };

            controller.ModelState.AddModelError("Email", "The Email field is required.");

            // Act
            var result = await controller.Resgister(registerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ShouldReturnConflict()
        {
            // Arrange
           var mockUserRepository = new Mock<IUserRepository>();
            var mockValidator = new Mock<IValidator<UserData>>();
            var mockLogger = new Mock<ILogger<UserRegistrationController>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockTokenService = new Mock<ITokenService>();

            var controller = new UserRegistrationController(
                mockUserRepository.Object,
                mockValidator.Object,
                mockLogger.Object,
                mockUserManager.Object,
                mockTokenService.Object);

            var registerDto = new RegisterDto {Name="test", Email = "existing@example.com", Password="Test@1234512"};
            var userdata = new UserData {Name="test", Email = "existing@example.com", Password="Test@1234512"};
            // Mock repository behavior to return existing user with provided email
            // mockUserRepository.Setup(repo => repo.GetByEmailAsync(userdata.Email))
            //     .ReturnsAsync((AppUser)null);

             mockUserRepository.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserData { UserId = 1, Name="test", Email = "test@example.com", Password = "password123" });


            // Act
            var result = await controller.Resgister(registerDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("User with the provided email already exists.", conflictResult.Value);
        }


        private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task GetUsers_WithValidId_ShouldReturnOk()
        {
            // Arrange
            int userId = 1;
            var mockUserRepository = new Mock<IUserRepository>();
            var mockValidator = new Mock<IValidator<UserData>>();
            var mockLogger = new Mock<ILogger<UserRegistrationController>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockTokenService = new Mock<ITokenService>();

            var controller = new UserRegistrationController(
                mockUserRepository.Object,
                mockValidator.Object,
                mockLogger.Object,
                mockUserManager.Object,
                mockTokenService.Object);
            //var userDto = new UserData { UserId = userId, Name = "Test User", Email = "test@example.com", Password = "Pass@123" };
            mockUserRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new UserData { UserId = userId, Name="Test User", Email = "test@example.com", Password = "Pass@123"} ); // Return the UserDto directly

            // Act
            var result = await controller.GetUsers(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserDto = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(userId, returnedUserDto.UserId);
          
        }

        [Fact]
        public async Task GetUsers_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            int userId = 100;
            var mockUserRepository = new Mock<IUserRepository>();
            var mockValidator = new Mock<IValidator<UserData>>();
            var mockLogger = new Mock<ILogger<UserRegistrationController>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockTokenService = new Mock<ITokenService>();

            var controller = new UserRegistrationController(
                mockUserRepository.Object,
                mockValidator.Object,
                mockLogger.Object,
                mockUserManager.Object,
                mockTokenService.Object);
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