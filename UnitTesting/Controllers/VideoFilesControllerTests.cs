using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using DataLayer.Entities;
using FileUploadApplication.Controllers;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryLayer.Interfaces;
using Xunit;

namespace UnitTesting.Controllers
{
    public class VideoFilesControllerTests
    {
        [Fact]
        public async Task VideoUpload_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var mockRepository = new Mock<IImageRepository>();
            var mockLogger = new Mock<ILogger<VideoFilesController>>();
            var mockGoogleDrive = new Mock<IGoogleDrive>();

            var controller = new VideoFilesController(mockRepository.Object, mockGoogleDrive.Object, mockLogger.Object);

            var videoFileMock = new Mock<IFormFile>();
            videoFileMock.SetupGet(file => file.Length).Returns(10); // Example file length
            videoFileMock.SetupGet(file => file.FileName).Returns("example.mp4"); // Example file name
            var fileId = "fileId";
            var uniqueFileName = "testfile.jpg";
            var imgData = new ImgData { ImgDataId = 1, Caption = "test", DriveFileId = fileId, ImageData = "", VideoData= uniqueFileName };

            mockGoogleDrive.Setup(service => service.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(fileId);

            // Act
            var result = await controller.VideoUpload(imgData, videoFileMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"File uploaded successfully. File ID: {fileId}", okResult.Value);

            // Verify that the repository's AddAsync method was called with the provided imgData
            mockRepository.Verify(repo => repo.AddAsync(imgData), Times.Once);
        }

        [Fact]
        public async Task VideoUpload_WithInvalidFile_ShouldReturnBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IImageRepository>();
            var mockLogger = new Mock<ILogger<VideoFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
             var videoFileMock = new Mock<IFormFile>();

            var controller = new VideoFilesController(mockRepository.Object, mockGoogleDriveService.Object, mockLogger.Object);

            // No file provided

            // Act
            var result = await controller.VideoUpload(new ImgData { ImgDataId = 0, Caption = "", DriveFileId = "", ImageData = "", VideoData= "" }, videoFileMock.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file uploaded.", badRequestResult.Value);

            // Verify that the repository's AddAsync method was not called
            mockRepository.Verify(repo => repo.AddAsync(It.IsAny<ImgData>()), Times.Never);
        }

        [Fact]
        public async Task DownloadVideoFile_WithValidFileId_ShouldReturnFileResult()
        {
            // Arrange
            string fileId = "validFileId";
            var mockLogger = new Mock<ILogger<VideoFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockRepository = new Mock<IImageRepository>();
            var controller = new VideoFilesController(mockRepository.Object, mockGoogleDriveService.Object, mockLogger.Object);

            byte[] fileContent = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            mockGoogleDriveService.Setup(service => service.DownloadFileAsync(fileId))
                .ReturnsAsync(fileContent);

            // Act
            var result = await controller.DownloadVideoFile(fileId);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("downloaded_file.mp4", fileResult.FileDownloadName);
            Assert.Equal("application/octet-stream", fileResult.ContentType);
            Assert.Equal(fileContent, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadVideoFile_WithInvalidFileId_ShouldReturnStatusCode500()
        {
            // Arrange
            string fileId = "invalidFileId";
            var mockLogger = new Mock<ILogger<VideoFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockRepository = new Mock<IImageRepository>();
            var controller = new VideoFilesController(mockRepository.Object, mockGoogleDriveService.Object, mockLogger.Object);

            mockGoogleDriveService.Setup(service => service.DownloadFileAsync(fileId))
                .ThrowsAsync(new Exception("File not found"));

            // Act
            var result = await controller.DownloadVideoFile(fileId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}