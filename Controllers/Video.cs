using Microsoft.AspNetCore.Mvc;
using FileUploadApplication.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using FileUploadApplication.Data;
using FileUploadApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileUploadApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoFilesController : ControllerBase
    {
        private readonly GoogleDriveService _googleDriveService;

         private readonly DataContext _context;

        public VideoFilesController(GoogleDriveService googleDriveService, DataContext context)
        {
            _context = context;
            _googleDriveService = googleDriveService;
        }

        [HttpPost("uploadvideo")]
        public async Task<ActionResult<List<ImgData>>> VideoUpload(
            //[FromBody] UserData userData, // Expecting user ID from the request body
            [FromForm] ImgData imgData,
            IFormFile videoFile
        )
        {
            if (videoFile == null || videoFile.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                string fileExtension = Path.GetExtension(videoFile.FileName).ToLowerInvariant();
                List<string> allowedExtensions = new List<string> { ".mp4", ".mov", ".wmv", ".mkv" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Invalid file type. Only images (.mp4, .mov, .wmv, .mkv) are allowed.");
                }

                // Generate a unique filename
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(videoFile.FileName);

                // Save the file to Google Drive
                var fileId = await _googleDriveService.UploadFileAsync(videoFile, uniqueFileName);

                // Store the file name (or file ID) in the database along with other data
                imgData.VideoData = uniqueFileName; // Store the unique file name in the database
                imgData.DriveFileId = fileId;
                _context.ImgDatas.Add(imgData);
                await _context.SaveChangesAsync();

                return Ok($"File uploaded successfully. File ID: {fileId}");
            }
                catch (Exception ex)
            {
                // Log the error including inner exception details
                Console.WriteLine($"An error occurred: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, "An error occurred while saving entity changes. See the inner exception for details.");
            }
        }

        [HttpGet("downloadvideo/{fileId}")]
        public async Task<IActionResult> DownloadVideoFile(string fileId)
        {
            try
            {
                var fileContent = await _googleDriveService.DownloadFileAsync(fileId);
                return File(fileContent, "application/octet-stream", "downloaded_file.mp4");
            }
            catch (Exception ex)
            {
                // Handle error
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}