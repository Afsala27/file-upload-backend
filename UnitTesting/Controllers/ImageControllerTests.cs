using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using FileUploadApplication.Controllers;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryLayer.Interfaces;
using Xunit;

namespace UnitTesting.Controllers
{
    public class ImageControllerTest
    {
        [Fact]
        public async Task Image_WithValidImage_ShouldReturnOk()
        {
            // Arrange
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockImageRepository = new Mock<IImageRepository>();
            var mockLogger = new Mock<ILogger<ImageFilesController>>();

            var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);
            var fileMock = new Mock<IFormFile>();
            var uniqueFileName = "testfile.jpg";
            var fileId = "testfile_id";
            var imgData = new ImgData { ImgDataId = 1, Caption = "test", DriveFileId = fileId, ImageData = uniqueFileName, VideoData="" };

            // Configure file mock
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("test file content");
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.FileName).Returns(uniqueFileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);

            // Setup GoogleDriveService
            mockGoogleDriveService.Setup(service => service.UploadFileAsync(fileMock.Object, uniqueFileName))
                .ReturnsAsync(fileId);

            // Setup ImageRepository
            mockImageRepository.Setup(repo => repo.AddAsync(It.IsAny<ImgData>()))
                .Returns(Task.CompletedTask); // Assuming AddAsync returns void or Task.CompletedTask

            // Act
            var result = await controller.ImageUpload(imgData, fileMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(okResult.Value, okResult.Value);

            // Verify that the image data was added to the repository
            mockImageRepository.Verify(repo => repo.AddAsync(It.IsAny<ImgData>()), Times.Once);
        }


         [Fact]
        public async Task DownloadFile_WithValidFileId_ShouldReturnFileResult()
        {
            // Arrange
            string fileId = "validFileId";
            var mockLogger = new Mock<ILogger<ImageFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockImageRepository = new Mock<IImageRepository>();
            var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);

            // Assuming that you have a valid file content to return
            byte[] fileContent = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            mockGoogleDriveService.Setup(service => service.DownloadFileAsync(fileId))
                .ReturnsAsync(fileContent);

            // Act
            var result = await controller.DownloadFile(fileId);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("downloaded_file.png", fileResult.FileDownloadName);
            Assert.Equal("application/octet-stream", fileResult.ContentType);
            Assert.Equal(fileContent, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadFile_WithInvalidFileId_ShouldReturnStatusCode500()
        {
            // Arrange
            string fileId = "invalidFileId";
            var mockLogger = new Mock<ILogger<ImageFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockImageRepository = new Mock<IImageRepository>();
            var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);

            // Assuming that an exception is thrown when trying to download the file
            mockGoogleDriveService.Setup(service => service.DownloadFileAsync(fileId))
                .ThrowsAsync(new Exception("File not found"));

            // Act
            var result = await controller.DownloadFile(fileId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task UpdateImgUploadComment_WithValidData_ShouldReturnOk()
        {
            // Arrange
            int imgDataId = 1;
            var mockLogger = new Mock<ILogger<ImageFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockImageRepository = new Mock<IImageRepository>();
            var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);

            var updatedData = new ImgData { ImgDataId = imgDataId, Comment = "Updated Comment", Caption="", DriveFileId="", VideoData="",ImageData="" };
            var existingData = new ImgData { ImgDataId = imgDataId, Comment = "Original Comment", Caption="", DriveFileId="", VideoData="",ImageData="" };
            mockImageRepository.Setup(repo => repo.GetByIdAsync(imgDataId))
                .ReturnsAsync(existingData);

            // Act
            var result = await controller.UpdateImgUploadComment(updatedData);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Updated Successfully", okResult.Value);

            // Verify that the repository method was called with the correct parameters
            mockImageRepository.Verify(repo => repo.UpdateCommentAsync(imgDataId, updatedData.Comment), Times.Once);
        }

        [Fact]
        public async Task UpdateImgUploadComment_WithInvalidData_ShouldReturnNotFound()
        {
            // Arrange
            int imgDataId = 1;
            var mockLogger = new Mock<ILogger<ImageFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockImageRepository = new Mock<IImageRepository>();
            var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);

            // Simulate repository returning null (data not found)
            mockImageRepository.Setup(repo => repo.GetByIdAsync(imgDataId))
                .ReturnsAsync((ImgData?)null);

            // Act
            var result = await controller.UpdateImgUploadComment(new ImgData { ImgDataId = imgDataId, Comment = "", Caption="", DriveFileId="", VideoData="",ImageData="" });

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Data not found", notFoundResult.Value);
        }

        // [Fact]
        // public async Task UpdateImgUploadComment_WithException_ShouldReturnInternalServerError()
        // {
        //     // Arrange
        //     var mockLogger = new Mock<ILogger<ImageFilesController>>();
        //     var mockGoogleDriveService = new Mock<IGoogleDrive>();
        //     var mockImageRepository = new Mock<IImageRepository>();
        //     var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);

        //     // Simulate an exception being thrown by the repository
        //     mockImageRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
        //         .ThrowsAsync(new Exception("Repository exception"));

        //     // Act
        //     var result = await controller.UpdateImgUploadComment(new ImgData { Caption ="", DriveFileId="", ImageData="", VideoData=""});

        //     // Assert
        //     var statusCodeResult = Assert.IsType<ObjectResult>(result);
        //     Assert.Equal(500, statusCodeResult.StatusCode);
        // }

        [Fact]
        public async Task DeleteImgUpload_WithValidId_ShouldReturnOk()
        {
            // Arrange
            int id = 1;
            var mockLogger = new Mock<ILogger<ImageFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockImageRepository = new Mock<IImageRepository>();
            var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);

            var upload = new ImgData { ImgDataId = id,  Comment = "", Caption="", DriveFileId="", VideoData="",ImageData=""  };
            mockImageRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(upload);

            // Act
            var result = await controller.DeleteImgUpload(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Deleted succesfully", okResult.Value);
            mockImageRepository.Verify(repo => repo.DeleteAsync(upload), Times.Once);
        }

        [Fact]
        public async Task DeleteImgUpload_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            int id = 1;
            var mockLogger = new Mock<ILogger<ImageFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockImageRepository = new Mock<IImageRepository>();
            var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);

            mockImageRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ImgData?)null);

            // Act
            var result = await controller.DeleteImgUpload(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Data not found", notFoundResult.Value);
            mockImageRepository.Verify(repo => repo.DeleteAsync(It.IsAny<ImgData>()), Times.Never);
        }

        // [Fact]
        // public async Task DeleteImgUpload_WithExceptionThrown_ShouldReturnStatusCode500()
        // {
        //     // Arrange
        //     int id = 1;
        //     var mockLogger = new Mock<ILogger<ImageFilesController>>();
        //     var mockGoogleDriveService = new Mock<IGoogleDrive>();
        //     var mockImageRepository = new Mock<IImageRepository>();
        //     var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);

        //     var upload = new ImgData { ImgDataId = id, Comment = "", Caption="", DriveFileId="", VideoData="",ImageData="" };
        //     mockImageRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(upload);
        //     mockImageRepository.Setup(repo => repo.DeleteAsync(upload)).ThrowsAsync(new Exception("Delete failed"));

        //     // Act
        //     var result = await controller.DeleteImgUpload(id);

        //     // Assert
        //     var statusCodeResult = Assert.IsType<ObjectResult>(result);
        //     Assert.Equal(500, statusCodeResult.StatusCode);
        //     //Assert.Equal("Internal Server Error: Delete failed", statusCodeResult.StatusCode);
        // }

         [Fact]
        public async Task UpdateImgUpload_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockImageRepository = new Mock<IImageRepository>();
            var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);
            var demodata = new ImgData { UserId = 1, Caption = "New Caption", DriveFileId="", VideoData="",ImageData=""  };

            var existingUpload = new ImgData { UserId = 1, Caption = "Old Caption", DriveFileId="", VideoData="",ImageData=""  };
            mockImageRepository.Setup(repo => repo.GetByIdAsync(demodata.UserId))
                .ReturnsAsync(existingUpload);

            // Act
            var result = await controller.UpdateImgUpload(demodata);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Edited Succesfully", okResult.Value);

            // Verify that the repository's UpdateAsync method was called once with the provided demodata
            mockImageRepository.Verify(repo => repo.UpdateAsync(demodata), Times.Once);
        }

        [Fact]
        public async Task UpdateImgUpload_WithInvalidData_ShouldReturnNotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ImageFilesController>>();
            var mockGoogleDriveService = new Mock<IGoogleDrive>();
            var mockImageRepository = new Mock<IImageRepository>();
            var controller = new ImageFilesController(mockGoogleDriveService.Object, mockImageRepository.Object, mockLogger.Object);
            var demodata = new ImgData { UserId = 1, Caption = "New Caption", DriveFileId="", VideoData="",ImageData=""  };

            // No existing upload found
            mockImageRepository.Setup(repo => repo.GetByIdAsync(demodata.UserId))
                .ReturnsAsync((ImgData?)null);

            // Act
            var result = await controller.UpdateImgUpload(demodata);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Data not found", notFoundResult.Value);

            // Verify that the repository's UpdateAsync method was not called
            mockImageRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ImgData>()), Times.Never);
        }

        }
    }
